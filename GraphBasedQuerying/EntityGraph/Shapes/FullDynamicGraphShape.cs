using System.Reflection;

namespace EntityGraph
{
    //TODO: Sealed?
    public class FullDynamicGraphShape<TEntity> : DynamicGraphShape<TEntity>
        where TEntity : class
    {
        public FullDynamicGraphShape()
            : base(x => (x.PropertyType.IsClass || x.PropertyType.IsInterface)
                && !x.PropertyType.IsAssignableFrom(typeof(string))
                && !IsIEnumerableOfValueTypes(x))
        {

        }

        private static bool IsIEnumerableOfValueTypes(PropertyInfo edge)
        {
            if (MultiplicityFunctions.DetermineMultiplicity(edge) == Multiplicity.Multi
                && edge.PropertyType.IsGenericType)
            {
                var elementType = edge.PropertyType.GetGenericArguments()[0];
                return elementType.IsValueType;
            }
            return false;
        }
    }
}
