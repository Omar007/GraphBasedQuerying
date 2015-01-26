using System;
using EntityGraph;

namespace EntityGraph4EF6
{
    internal class TypeTreeEdge
    {
        //TODO: readonly?
        public TypeTree From;
        public TypeTree To;
        public readonly Edge Edge;

        public TypeTreeEdge(TypeTree from, TypeTree to, Edge edge)
        {
            From = from;
            To = to;
            Edge = edge;
        }

        public override string ToString()
        {
            return String.Format("{0}->{1}", From, To);
        }
    }
}
