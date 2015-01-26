using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Graph4EF
{
    internal class QueryPlan
    {
        private readonly IList<GraphPath> _paths;

        public QueryPlan(IEnumerable<KeyValuePair<Type, IDictionary<Type, Expression>>> edges)
        {
            _paths = edges.SelectMany(edge => edge.Value.Select(e => new GraphPath(edge.Key, e.Key, e.Value))).ToList();
            BuildPaths(edges);
        }

        //TODO: Prevent paths from building infinitly when a->b, b->a
        private void BuildPaths(IEnumerable<KeyValuePair<Type, IDictionary<Type, Expression>>> edges)
        {
            bool progress = false;
            do
            {
                progress = false;
                foreach (var edge in edges.SelectMany(edge => edge.Value.Select(e => new { From = edge.Key, To = e.Key, Expr = e.Value })))
                {
                    //Create paths according to a transitivity relations a->b, c->d ==> a->c->d iff c is assignable from b
                    foreach (var path in _paths)
                    {
                        if (edge.To.IsAssignableFrom(path.First.Type))
                        {
                            if (path.First.Type != edge.To)
                            {
                                path.IsInheritancePath = true;
                                path.Add(null, edge.To);
                            }
                            path.Add(edge.Expr, edge.From);

                            progress = true;
                        }
                    }
                }
            }
            while (progress);
            
            //Remove sub-path dupes
            for (int i = 0; i + 1 < _paths.Count; i++)
            {
                for (int j = 0; j < _paths.Count; j++)
                {
                    if (_paths[i].IsSubPath(_paths[j]))
                    {
                        _paths.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        public IEnumerable<T> Execute<T>(DbContext context, IQueryable<T> baseQuery)
        {
            GraphPath resultPath = null;

            foreach (var path in _paths)
            {
                path.BuildQuery(baseQuery);
                if (resultPath == null)
                {
                    if ((path.IsInheritancePath ? path.Last.Type : path.First.Type) == typeof(T))
                    {
                        resultPath = path;
                    }
                }
            }

            using (var dbCommand = ConstructDbCommand(context))
            {
                dbCommand.Connection.Open();
                using (var reader = dbCommand.ExecuteReader())
                {
                    foreach (var path in _paths)
                    {
                        path.SetResult(context, reader);

                        reader.NextResult();
                    }
                }
                dbCommand.Connection.Close();
            }

            return (IEnumerable<T>)resultPath.Result;
        }

        private DbCommand ConstructDbCommand(DbContext context)
        {
            DbConnection dbConnection = context.Database.Connection;

            var entityConnection = dbConnection as EntityConnection;
            //Make sure we have a direct store connection.
            var command = entityConnection == null
                ? dbConnection.CreateCommand()
                : entityConnection.StoreConnection.CreateCommand();

            var commandText = new StringBuilder();
            var i = 0;

            foreach (var path in _paths)
            {
                string sql = path.ObjectQuery.ToTraceString();

                foreach (var parameter in path.ObjectQuery.Parameters)
                {
                    string oldParamName = parameter.Name;
                    string newParamName = String.Format("f{0}_{1}", i++, oldParamName);
                    sql = sql.Replace("@" + oldParamName, "@" + newParamName);

                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = newParamName;
                    dbParameter.Value = parameter.Value == null ? DBNull.Value : parameter.Value;

                    command.Parameters.Add(dbParameter);
                }

                commandText.Append(sql.Trim());
                commandText.AppendLine(";");
            }

            command.CommandText = commandText.ToString();
            return command;
        }
    }
}
