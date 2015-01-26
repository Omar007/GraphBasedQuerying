using System;
using System.Reflection;

namespace EntityGraph
{
    public abstract class Edge
    {
        public readonly PropertyInfo EdgeInfo;
        public readonly Type FromType;
        public readonly Type ToType;

        protected Edge(Type fromType, Type toType, PropertyInfo edge)
        {
            FromType = fromType;
            ToType = toType;
            EdgeInfo = edge;
        }

        public override string ToString()
        {
            return String.Format("{0}->{1}", FromType.Name, ToType.Name);
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, this))
            {
                return true;
            }

            var edge = obj as Edge;
            if (obj != null)
            {
                return FromType == edge.FromType && EdgeInfo == edge.EdgeInfo;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            if (FromType != null)
            {
                hashCode ^= FromType.GetHashCode();
            }
            if (EdgeInfo != null)
            {
                hashCode ^= EdgeInfo.GetHashCode();
            }
            return hashCode;
        }
    }

    public class Edge<TFrom> : Edge
    {
        protected Edge(Type toType, PropertyInfo edge)
            : base(typeof(TFrom), toType, edge)
        {

        }

        public Edge(PropertyInfo edge)
            : this(DetermineToType(edge), edge)
        {

        }

        protected static Type DetermineToType(PropertyInfo edge)
        {
            if (MultiplicityFunctions.DetermineMultiplicity(edge) == Multiplicity.Multi
                && edge.PropertyType.IsGenericType)
            {
                return edge.PropertyType.GetGenericArguments()[0];
            }
            else
            {
                return edge.PropertyType;
            }
        }
    }
}
