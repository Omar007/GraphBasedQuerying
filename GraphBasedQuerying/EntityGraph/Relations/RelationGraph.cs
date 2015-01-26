using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public class RelationGraph<TEntity>
        where TEntity : class
    {
        private readonly ICollection<RelationNode<TEntity>> _relationNodes;

        public IEnumerable<IRelation<TEntity>> Relations { get { return _relationNodes.SelectMany(x => x); } }
        public IEnumerable<TEntity> Nodes { get { return _relationNodes.Select(x => x.Node); } }

        private RelationGraph()
        {
            _relationNodes = new HashSet<RelationNode<TEntity>>();
        }

        public RelationGraph(TEntity entity, IGraphShape<TEntity> shape)
            : this()
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "The constructor argument 'entity' can't be null");
            }
            if (shape == null)
            {
                throw new ArgumentNullException("shape", "The constructor argument 'shape' can't be null");
            }

            BuildRelationGraph(entity, shape);
        }

        public RelationGraph(IGraph<TEntity> graph)
            : this()
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph", "The constructor argument 'graph' can't be null");
            }

            BuildRelationGraph(graph.Source, graph.GraphShape);
        }
        
        private void BuildRelationGraph(TEntity entity, IGraphShape<TEntity> shape)
        {
            if (_relationNodes.Any(x => x.Node == entity))
            {
                return;
            }

            var relationNode = new RelationNode<TEntity>(entity);
            _relationNodes.Add(relationNode);

            foreach (var outEdge in shape.OutEdges(entity))
            {
                if (MultiplicityFunctions.DetermineMultiplicity(outEdge) == Multiplicity.Multi)
                {
                    var assocList = shape.GetNodes(entity, outEdge);
                    if (assocList == null)
                    {
                        continue;
                    }

                    relationNode.Add(new MultiRelation<TEntity>(assocList));

                    foreach (TEntity e in assocList.Where(x => x != null))
                    {
                        BuildRelationGraph(e, shape);
                    }
                }
                else
                {
                    var node = shape.GetNode(entity, outEdge);
                    if (node != null)
                    {
                        relationNode.Add(new SingleRelation<TEntity>(node));
                        BuildRelationGraph(node, shape);
                    }
                }
            }
        }
    }
}
