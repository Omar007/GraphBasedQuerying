using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph4EF6
{
    internal class GraphPathCollection<TEntity> : IEnumerable<GraphPath>
        where TEntity : class
    {
        private readonly IList<GraphPath> _paths;

        public GraphPathCollection(SqlGraphShape<TEntity> shape)
        {
            _paths = shape.Select(e => GraphPath.FromEdge(e, shape)).ToList();
            BuildPaths(shape);

            //TODO: Fix transitive filtering
            //Disabled. See remarks at method FilterTransitivePaths
            //FilterTransitivePaths();
        }

        //TODO: Prevent paths from building infinitly when a->b, b->a
        private void BuildPaths(SqlGraphShape<TEntity> shape)
        {
            bool progress = false;
            do
            {
                progress = false;
                foreach (var edge in shape)
                {
                    //Create paths according to a transitivity relations a->b, c->d ==> a->c->d iff c is assignable from b
                    foreach (var path in _paths.Where(p => edge.ToType.IsAssignableFrom(p.First().Type)))
                    {
                        var node = path.First();
                        if (node.Type != edge.ToType)
                        {
                            node = new GraphPathNode(null, null, shape.GetTypeMapping(edge.ToType), shape.Context);
                            path.Insert(node);
                        }
                        node.InEdge = edge;
                        path.Insert(new GraphPathNode(null, edge, shape.GetTypeMapping(edge.FromType), shape.Context));

                        progress = true;
                    }
                }
            }
            while (progress);
        }

        public IEnumerator<GraphPath> GetEnumerator()
        {
            return _paths.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }



        // The idea is that using transitivity we can reduce the number of paths as follows:
        // If a path A -> B -> C exists, then
        // a path A -> B -> X .. -> C can be removed
        // The idea is the the collection of Cs in the first path is always a superset of the
        // Cs resulting from the second path. Now, this is only true iff C holds the id of B (and _not_
        // the other way around. That is, iff B has a many-2-one relation to C (C holds many Bs).
        // This heuristic seems not to be valid.
        //private void FilterTransitivePaths()
        //{
        //    foreach(var path in _paths.ToList())
        //    {
        //        if(ShortestPath(path) != path)
        //        {
        //            _paths.Remove(path);
        //        }
        //    }
        //}

        //private GraphPath ShortestPath(GraphPath path)
        //{
        //    var found = path;
        //    foreach(var p in _paths.Where(p => p.Count() < path.Count() && p.Last().Type.IsAssignableFrom(path.Last().Type)))
        //    {
        //        if(StartsWith(path, p))
        //        {
        //            if(p.Count() < found.Count())
        //            {
        //                found = p;
        //            }
        //        }
        //    }
        //    return found;
        //}

        //public bool StartsWith(GraphPath path1, GraphPath path2)
        //{
        //    var count = path2.Count();
        //    if(path1.Count() <= count)
        //    {
        //        return false;
        //    }

        //    for(int i = 0; i + 1 < count; i++)
        //    {
        //        if(!path1[i].Type.IsAssignableFrom(path2[i].Type))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
}
