using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EntityGraph;
using EntityGraph4EF6.Mapping;
using EntityGraph4EF6.SQL;

namespace EntityGraph4EF6
{
    public class SqlGraphShape<TEntity> : GraphShape<TEntity>
        where TEntity : class
    {
        //TODO: Decouple WHERE from plan so we can reuse the whole query and only rebuild the WHERE.
        private IDictionary<Type, QueryPlan> _cache = new Dictionary<Type, QueryPlan>();

        private GraphPathCollection<TEntity> _graphPathCollection;
        private GraphPathCollection<TEntity> GraphPathCollection
        {
            get
            {
                if (_graphPathCollection == null)
                {
                    _graphPathCollection = new GraphPathCollection<TEntity>(this);
                }
                return _graphPathCollection;
            }
        }
        private readonly IEnumerable<TypeMapping> _typeMappings;
        
        internal readonly DbContext Context;

        public SqlGraphShape(DbContext context)
        {
            Context = context;

            _typeMappings = Mapper.GetTypeMappings(Context);
        }

        internal TypeMapping GetTypeMapping(Type type)
        {
            return _typeMappings.SingleOrDefault(m => m.Type == type);
        }

        //TODO: Check these. Path loading using stub entities has been removed.
        //public override TEntity GetNode(TEntity entity, PropertyInfo edge)
        //{
        //    var paths = GraphPathCollection.Where(p => p[0].OutEdge.EdgeInfo == edge).ToList();
        //    if (paths.Any())
        //    {
        //        LoadPaths(entity, paths);
        //    }
        //    return base.GetNode(entity, edge);
        //}

        //public override IEnumerable<TEntity> GetNodes(TEntity entity, PropertyInfo edge)
        //{
        //    var paths = GraphPathCollection.Where(p => p[0].OutEdge.EdgeInfo == edge).ToList();
        //    if (paths.Any())
        //    {
        //        LoadPaths(entity, paths);
        //    }
        //    return (IEnumerable<TEntity>)edge.GetValue(entity, null);
        //}

        public void Load<T>()
            where T : TEntity
        {
            var type = typeof(T);
            if (!_cache.ContainsKey(type))
            {
                var paths = GraphPathCollection.Where(p => p.IsMatchingForType(type)).ToList();
                LoadPaths<T>(paths);
            }

            Context.ExecuteQueryPlan(_cache[type]);
        }

        private void LoadPaths<T>(IEnumerable<GraphPath> paths)
        {
            var typeMapping = GetTypeMapping(typeof(T));
            var typeMappings = new List<TypeMapping>() { typeMapping };
            var queries = new List<SelectColumns>() { typeMapping.GetSelectColumns(null) };

            foreach (var path in paths)
            {
                queries.Add(path.ToSql(null));
                typeMappings.Add(path.Last().TypeMapping);
            }

            var queryPlan = new QueryPlan(queries, typeMappings);
            _cache.Add(typeof(T), queryPlan);
        }

        public void Load<T>(Expression<Func<T, bool>> expr)
            where T : class, TEntity
        {
            var type = typeof(T);
            var whereExpr = new LoadWhereExpr<T>(_typeMappings.Single(tm => tm.Type == type), expr);

            if (!_cache.ContainsKey(type))
            {
                var paths = GraphPathCollection.Where(p => p.IsMatchingForType(type)).ToList();
                if (paths.Any())
                {
                    LoadPaths(whereExpr, paths);
                }
            }

            Context.ExecuteQueryPlan(_cache[type]);
        }

        private void LoadPaths<T>(LoadWhereExpr<T> expr, IEnumerable<GraphPath> paths)
            where T : class, TEntity
        {
            var typeMapping = GetTypeMapping(typeof(T));
            var typeTrees = new List<TypeMapping>() { typeMapping };
            var queries = new List<SelectColumns>() { typeMapping.GetSelectColumns(expr.Condition) };

            foreach (var path in paths)
            {
                queries.Add(path.ToSql(expr.Condition));
                typeTrees.Add(path.Last().TypeMapping);
            }

            var queryPlan = new QueryPlan(queries, typeTrees);
            _cache.Add(typeof(T), queryPlan);
        }

        new public SqlGraphShape<TEntity> Edge<TFrom>(Expression<Func<TFrom, TEntity>> edge)
            where TFrom : TEntity
        {
            base.Edge(edge);
            return this;
        }

        new public SqlGraphShape<TEntity> Edge<TFrom>(Expression<Func<TFrom, IEnumerable<TEntity>>> edge)
            where TFrom : TEntity
        {
            base.Edge(edge);
            return this;
        }
    }
}
