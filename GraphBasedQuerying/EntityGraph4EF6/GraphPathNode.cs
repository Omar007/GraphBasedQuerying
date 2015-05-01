using System;
using System.Data.Entity;
using EntityGraph;
using EntityGraph4EF6.Mapping;

namespace EntityGraph4EF6
{
    internal class GraphPathNode
    {
        //TODO: readonly?
        public Edge InEdge;
        public readonly Edge OutEdge;
        public readonly TypeMapping TypeMapping;

        public Type Type { get { return TypeMapping.Type; } }

        public GraphPathNode(Edge inEdge, Edge outEdge, TypeMapping typeMapping, DbContext context)
        {
            InEdge = inEdge;
            OutEdge = outEdge;
            TypeMapping = typeMapping;
        }

        public override string ToString()
        {
            return Type.Name;
        }
    }
}
