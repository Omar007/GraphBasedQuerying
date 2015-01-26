using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EntityGraph;

namespace EntityGraph4EF6
{
    internal class GraphPath : IEnumerable<GraphPathNode>
    {
        private readonly IList<GraphPathNode> _nodes;

        public GraphPath()
        {
            _nodes = new List<GraphPathNode>();
        }

        internal GraphPathNode GetOwningNode(GraphPathNode node)
        {
            var owningNode = _nodes.First(o => o.Type == node.InEdge.EdgeInfo.DeclaringType);

            //TODO: Check the usefulness of this function as this assert is always passed.
            Debug.Assert(_nodes[_nodes.IndexOf(node) - 1] == owningNode);

            return owningNode;
        }

        internal bool IsMatchingForType(Type type)
        {
            return _nodes.Any(n => n.Type == type || n.Type.IsAssignableFrom(type));
        }

        public void Insert(GraphPathNode item)
        {
            _nodes.Insert(0, item);
        }

        public IEnumerator<GraphPathNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public GraphPathNode this[int index]
        {
            get { return _nodes[index]; }
            set { _nodes[index] = value; }
        }

        public override string ToString()
        {
            return String.Join("->", _nodes.Select(n => n.Type.Name));
        }

        public static GraphPath FromEdge<TEntity>(Edge edge, SqlGraphShape<TEntity> shape)
            where TEntity : class
        {
            var path = new GraphPath();
            path._nodes.Add(new GraphPathNode(null, edge, shape.GetTypeMapping(edge.FromType), shape.Context));
            path._nodes.Add(new GraphPathNode(edge, null, shape.GetTypeMapping(edge.ToType), shape.Context));

            if (edge.EdgeInfo.DeclaringType != edge.FromType)
            {
                var node = new GraphPathNode(null, path.First().OutEdge, shape.GetTypeMapping(edge.EdgeInfo.DeclaringType), shape.Context);
                path.Insert(node);
            }

            return path;
        }
    }
}
