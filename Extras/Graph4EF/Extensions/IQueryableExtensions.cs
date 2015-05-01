using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EntityFramework.Reflection;

namespace Graph4EF
{
    internal static class IQueryableExtensions
    {
        internal static ObjectQuery<TEntity> ToObjectQuery<TEntity>(this IQueryable<TEntity> query)
            where TEntity : class
        {
            var objectQuery = query as ObjectQuery<TEntity>;
            if (objectQuery != null)
                return objectQuery;

            var dbQuery = query as DbQuery<TEntity>;
            if (dbQuery == null)
                return null;

            dynamic dbQueryProxy = new DynamicProxy(dbQuery);
            dynamic internalQuery = dbQueryProxy.InternalQuery;
            if (internalQuery == null)
                return null;

            dynamic objectQueryProxy = internalQuery.ObjectQuery;
            if (objectQueryProxy == null)
                return null;

            return objectQueryProxy;
        }
    }
}
