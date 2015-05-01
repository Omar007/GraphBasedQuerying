using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using EntityGraph4EF6.Mapping;

namespace EntityGraph4EF6
{
    internal static class DbContextExtensions
    {
        internal static ObjectContext GetObjectContext(this DbContext context)
        {
            return ((IObjectContextAdapter)context).ObjectContext;
        }

        internal static void ExecuteQueryPlan(this DbContext context, QueryPlan queryPlan)
        {
            //Disable change detection; we are just reading in.
            //Also, if we don't do this, DbContext is way slower then ObjectContext.
            var oldDetectValue = context.Configuration.AutoDetectChangesEnabled;
            context.Configuration.AutoDetectChangesEnabled = false;

            using (var reader = ExecuteQuery(context, queryPlan.CommandText))
            {
                foreach (var rootTypeMapping in queryPlan.TypeMappings)
                {
                    while (reader.Read())
                    {
                        var typeId = reader[TypeMapping.TypeColumn];
                        if (typeId == DBNull.Value)
                        {
                            return;
                        }

                        var typeMapping = rootTypeMapping.GetFor((string)typeId);
                        var dbSet = context.Set(typeMapping.Type);
                        var entity = FillProperties(typeMapping, reader, dbSet.Create(typeMapping.Type));

                        try
                        {
                            dbSet.Attach(entity);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                        }
                    }
                    reader.NextResult();
                }
            }

            context.Configuration.AutoDetectChangesEnabled = oldDetectValue;
            context.ChangeTracker.DetectChanges();
        }

        private static DbDataReader ExecuteQuery(DbContext context, string query)
        {
            var dbCommand = context.Database.Connection.CreateCommand();
            dbCommand.CommandText = query;

            Debug.WriteLine(dbCommand.CommandText);

            //TODO: Create bindings in Expression2Sql instead of assigning values?
            //foreach (var binding in queryPlan.Bindings)
            //{
            //    var param = dbCommand.CreateParameter();
            //    param.ParameterName = binding.Parameter.ToString();
            //    param.Value = binding.Value.ToString();
            //    dbCommand.Parameters.Add(param);
            //}

            dbCommand.Connection.Open();
            return dbCommand.ExecuteReader();
        }

        private static object FillProperties(TypeMapping typeMapping, DbDataReader reader, object entity)
        {
            while (typeMapping != null)
            {
                foreach (var propInfo in typeMapping.Properties.Select(pm => pm.Property).Where(p => p != null))
                {
                    var value = reader[propInfo.Name];
                    if (value != DBNull.Value)
                    {
                        propInfo.SetValue(entity, value, null);
                    }
                }
                typeMapping = typeMapping.BaseTypeMapping;
            }
            return entity;
        }
    }
}
