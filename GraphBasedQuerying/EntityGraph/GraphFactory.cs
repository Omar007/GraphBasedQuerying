using System;
using System.Collections.Generic;

namespace EntityGraph
{
    public static class GraphFactory
    {
        private static readonly Dictionary<int, WeakReference> _graphs = new Dictionary<int, WeakReference>();

        public static Graph<TEntity> GetInstance<TEntity>(TEntity entity, IGraphShape<TEntity> shape)
            where TEntity : class
        {
            var key = entity.GetHashCode() ^ shape.GetHashCode();

            WeakReference reference;
            if (!_graphs.TryGetValue(key, out reference))
            {
                reference = new WeakReference(new Graph<TEntity>(entity, shape));
                _graphs.Add(key, reference);
            }

            return reference.Target as Graph<TEntity>;
        }
    }
}
