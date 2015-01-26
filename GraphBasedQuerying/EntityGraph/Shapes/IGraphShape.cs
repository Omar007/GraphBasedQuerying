using System;
using System.Collections.Generic;
using System.Reflection;

namespace EntityGraph
{
    public interface IGraphShape<TEntity>
        where TEntity : class
    {
        IEnumerable<PropertyInfo> OutEdges(TEntity entity);
        IEnumerable<PropertyInfo> OutEdges(Type entityType);

        bool IsEdge(PropertyInfo edge);

        TEntity GetNode(TEntity entity, PropertyInfo edge);
        IEnumerable<TEntity> GetNodes(TEntity entity, PropertyInfo edge);
    }
}
