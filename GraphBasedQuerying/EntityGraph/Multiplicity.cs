using System.Collections;
using System.Reflection;

namespace EntityGraph
{
    public enum Multiplicity
    {
        Single,
        Multi
    }

    public static class MultiplicityFunctions
    {
        public static Multiplicity DetermineMultiplicity(PropertyInfo edge)
        {
            return typeof(IEnumerable).IsAssignableFrom(edge.PropertyType)
                ? Multiplicity.Multi
                : Multiplicity.Single;
        }
    }
}
