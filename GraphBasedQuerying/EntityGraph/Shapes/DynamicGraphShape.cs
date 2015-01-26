using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntityGraph
{
    public class DynamicGraphShape<TEntity> : IGraphShape<TEntity>
        where TEntity : class
    {
        private readonly Func<PropertyInfo, bool> _isEdgeFilter;

        public DynamicGraphShape(Func<PropertyInfo, bool> isEdgeFilter)
        {
            _isEdgeFilter = (pi) => typeof(TEntity).IsAssignableFrom(pi.PropertyType) && isEdgeFilter(pi);
        }

        public IEnumerable<PropertyInfo> OutEdges(TEntity entity)
        {
            if (entity == null)
            {
                return Enumerable.Empty<PropertyInfo>();
            }
            return OutEdges(entity.GetType());
        }

        public IEnumerable<PropertyInfo> OutEdges(Type entityType)
        {
            return entityType.GetProperties().Where(_isEdgeFilter);
        }

        public bool IsEdge(PropertyInfo edge)
        {
            return _isEdgeFilter(edge);
        }

        public TEntity GetNode(TEntity entity, PropertyInfo edge)
        {
            return (TEntity)edge.GetValue(entity, null);
        }

        public IEnumerable<TEntity> GetNodes(TEntity entity, PropertyInfo edge)
        {
            var node = edge.GetValue(entity, null);
            if (node == null)
            {
                return Enumerable.Empty<TEntity>();
            }
            var nodes = node as IEnumerable<TEntity>;
            return nodes ?? new List<TEntity> { (TEntity)node };
        }
    }
}
