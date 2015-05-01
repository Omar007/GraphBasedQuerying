using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public static partial class ShapeExtensions
    {
        public static IEnumerable<TEntity> Flatten<TEntity>(this IGraphShape<TEntity> shape, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            var toVisit = new Queue<TEntity>(entities);
            var visited = new HashSet<TEntity>();
            
            while (toVisit.Count > 0)
            {
                var entity = toVisit.Dequeue();
                visited.Add(entity);

                foreach (var nodes in shape.OutEdges(entity).Select(x => shape.GetNodes(entity, x)))
                {
                    foreach (var node in nodes.Where(x => !visited.Contains(x) && !toVisit.Contains(x)))
                    {
                        toVisit.Enqueue(node);
                    }
                }
            }
            return visited;
        }

        public static GraphShape<TEntity> Union<TEntity>(this GraphShape<TEntity> shape1, GraphShape<TEntity> shape2)
            where TEntity : class
        {
            var edges = new List<Edge>(shape1);
            edges.AddRange(shape2.Where(e => !edges.Contains(e)));
            return new GraphShape<TEntity>(edges);
        }
    }
}
