using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace EntityGraph
{
    //TODO: Cleanup shape copying
    public static partial class ShapeExtensions
    {
        public static TTo CopyTo<TFrom, TTo>(this IGraphShape<TFrom> shape, TFrom fromEntity, ITypeMapper typeMapper)
            where TFrom : class
            where TTo : class
        {
            if (shape == null)
            {
                throw new ArgumentNullException("shape");
            }
            if (fromEntity == null)
            {
                throw new ArgumentNullException("fromEntity");
            }
            if (typeMapper == null)
            {
                throw new ArgumentNullException("typeMapper");
            }

            return CopyTo<TFrom, TTo>(shape, fromEntity, typeMapper, new Dictionary<TFrom, TTo>());
        }

        //TODO: Is this useful?
        //public static IEnumerable<KeyValuePair<TFrom, TTo>> CopyTo<TFrom, TTo>(this IGraphShape<TFrom> shape,
        //    IEnumerable<TFrom> fromEntities, ITypeMapper typeMapper)
        //    where TFrom : class
        //    where TTo : class
        //{
        //    if (shape == null)
        //    {
        //        throw new ArgumentNullException("shape");
        //    }
        //    if (fromEntities == null)
        //    {
        //        throw new ArgumentNullException("fromEntities");
        //    }
        //    if (typeMapper == null)
        //    {
        //        throw new ArgumentNullException("typeMapper");
        //    }

        //    var visited = new Dictionary<TFrom, TTo>();
        //    foreach (var fromEntity in fromEntities)
        //    {
        //        TTo toEntity;
        //        if (!visited.TryGetValue(fromEntity, out toEntity))
        //        {
        //            toEntity = CopyTo<TFrom, TTo>(shape, fromEntity, typeMapper, visited);
        //        }

        //        yield return new KeyValuePair<TFrom, TTo>(fromEntity, toEntity);
        //    }
        //}

        private static TTo CopyTo<TFrom, TTo>(this IGraphShape<TFrom> shape, TFrom fromEntity, ITypeMapper typeMapper,
            IDictionary<TFrom, TTo> visited)
            where TFrom : class
            where TTo : class
        {
            var toEntity = CopyFromTo<TFrom, TTo>(fromEntity, typeMapper);
            Debug.Assert(!visited.ContainsKey(fromEntity));
            visited[fromEntity] = toEntity;

            var fromType = fromEntity.GetType();
            var toType = toEntity.GetType();

            //TODO: Ensure shape is never defined using toType objects?
            var outEdges = shape.OutEdges(fromEntity);
            if (!outEdges.Any())
            {
                outEdges = shape.OutEdges(toEntity as TFrom);
            }

            foreach (var edge in outEdges)
            {
                var fromPropInfo = fromType.GetProperty(edge.Name);
                var toPropInfo = toType.GetProperty(edge.Name);
                if (fromPropInfo == null || toPropInfo == null)
                {
                    continue;
                }
                var fromPropvalue = fromPropInfo.GetValue(fromEntity, null);
                if (fromPropvalue == null)
                {
                    continue;
                }

                if (MultiplicityFunctions.DetermineMultiplicity(fromPropInfo) == Multiplicity.Multi)
                {
                    var toList = (IEnumerable)toPropInfo.GetValue(toEntity, null);
                    // If the IEnumerable is null, lets try to allocate one
                    if (toList == null)
                    {
                        var constr = toPropInfo.PropertyType.GetConstructor(new Type[] { });
                        if (constr == null)
                        {
                            throw new ArgumentNullException("Could not get parameterless constructor for type " + toPropInfo.PropertyType.Name);
                        }
                        toList = (IEnumerable)constr.Invoke(new object[] { });
                        toPropInfo.SetValue(toEntity, toList, null);
                    }
                    var addMethod = toPropInfo.PropertyType.GetMethod("Add");
                    foreach (var fromChild in ((IEnumerable)fromPropvalue).Cast<TFrom>())
                    {
                        if (!visited.ContainsKey(fromChild))
                        {
                            var toChild = shape.CopyTo<TFrom, TTo>((TFrom)fromChild, typeMapper, visited);
                            addMethod.Invoke(toList, new object[] { toChild });
                        }
                    }
                }
                else
                {
                    var fromChild = (TFrom)fromPropvalue;

                    TTo toChild;
                    if (!visited.TryGetValue(fromChild, out toChild))
                    {
                        toChild = shape.CopyTo<TFrom, TTo>(fromChild, typeMapper, visited);
                    }

                    toPropInfo.SetValue(toEntity, toChild, null);
                }
            }
            return toEntity;
        }

        private static IEnumerable<PropertyInfo> GetDataMembers(Type fromObjectType, Type toObjectType)
        {
            //Removed (includeKeys || KeyAttribute) checks as:
            // 1) includeKeys was always true
            // 2) KeyAttribute is not available in WP8
            // 3) KeyAttribute does not need to be set on keys so if includeKeys was false but the attribute wasn't set, it would still have been included.

            const BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance;
            var qryToObject = (from p in toObjectType.GetProperties(bindingAttr)
                               where p.IsDefined(typeof(DataMemberAttribute), true) && p.CanWrite
                               select p).ToArray();

            var qryFromObject = (from p in fromObjectType.GetProperties(bindingAttr)
                                 where p.IsDefined(typeof(DataMemberAttribute), true) && p.CanRead
                                 select p).ToArray();

            var result = from propToObject in qryToObject
                         from propFromObject in qryFromObject
                         where propToObject.Name == propFromObject.Name
                         select propToObject;

            return result.ToArray();
        }

        private static TTo CopyFromTo<TFrom, TTo>(TFrom fromEntity, ITypeMapper typeMapper)
            where TFrom : class
            where TTo : class
        {
            var fromEntityType = fromEntity.GetType();
            var toEntityType = typeMapper.Map(fromEntityType);

            if (toEntityType == null)
            {
                throw new Exception(String.Format("EntityGraphShape.Copy: Can't find a mapping for type {0}.",
                    fromEntityType.FullName));
            }
            var toEntity = (TTo)Activator.CreateInstance(toEntityType);

            CopyDataMembers(fromEntity, toEntity);
            return toEntity;
        }

        private static void CopyDataMembers(object fromObject, object toObject)
        {
            var fromObjectType = fromObject.GetType();
            foreach (var toProperty in GetDataMembers(fromObjectType, toObject.GetType()))
            {
                var fromProperty = fromObjectType.GetProperty(toProperty.Name);
                var fromValue = fromProperty.GetValue(fromObject, null);
                if (fromValue == null)
                {
                    continue;
                }
                var toValue = CopyDataMember(toProperty.PropertyType, fromProperty.PropertyType, fromValue);
                toProperty.SetValue(toObject, toValue, null);
            }
        }

        private static object CopyDataMember(Type toType, Type fromType, object fromValue)
        {
            if (toType.IsAssignableFrom(fromType))
            {
                return fromValue;
            }
            if (typeof(Enum).IsAssignableFrom(toType) && typeof(Enum).IsAssignableFrom(fromType))
            {
                var toEnumPropertyType = Enum.GetUnderlyingType(toType);
                var fromEnumPropertyType = Enum.GetUnderlyingType(fromType);
                if (!toEnumPropertyType.IsAssignableFrom(fromEnumPropertyType))
                {
                    throw new Exception("Incompatible enum types encountered: " + toType.Name + " " + fromType.Name);
                }

                return Convert.ChangeType(fromValue, toEnumPropertyType, null);
            }
            if (IsGenericCollection(typeof(ICollection<>), toType) &&
               IsGenericCollection(typeof(IEnumerable<>), fromType))
            {
                var constr = toType.GetConstructor(new Type[] { });
                if (constr == null)
                {
                    throw new InvalidCastException("No parameterless constructor defined for type: " +
                                                   toType.FullName);
                }

                var fromElementType = fromType.GetGenericArguments()[0];
                var toElementType = toType.GetGenericArguments()[0];
                var toList = (IEnumerable)constr.Invoke(new object[] { });
                var addMethod = toType.GetMethod("Add");

                foreach (var fromEnumValue in (IEnumerable)fromValue)
                {
                    var value = CopyDataMember(toElementType, fromElementType, fromEnumValue);
                    addMethod.Invoke(toList, new[] { value });
                }
                return toList;
            }
            var propValue = Activator.CreateInstance(toType);
            CopyDataMembers(fromValue, propValue);
            return propValue;
        }

        private static bool IsGenericCollection(Type genericType, Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition().IsAssignableFrom(genericType)
                || type.GetInterfaces().Where(x => x.IsGenericType).Any(x => x.GetGenericTypeDefinition()
                    .IsAssignableFrom(genericType))
            );
        }
    }
}
