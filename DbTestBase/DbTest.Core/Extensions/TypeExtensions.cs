using System;

namespace DbTest.Core
{
    public static class TypeExtensions
    {
        public static bool IsSubclassOfGeneric(this Type type, Type genericType)
        {
            Type baseType = type.BaseType;
            if (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }

                return baseType.IsSubclassOfGeneric(genericType);
            }

            return false;
        }
    }
}
