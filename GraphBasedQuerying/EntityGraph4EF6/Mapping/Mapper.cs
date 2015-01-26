using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace EntityGraph4EF6.Mapping
{
    internal static class Mapper
    {
        public static IEnumerable<TypeMapping> GetTypeMappings(DbContext context)
        {
            var metadataWorkspace = context.GetObjectContext().MetadataWorkspace;

            var objectItemCollection = (ObjectItemCollection)metadataWorkspace.GetItemCollection(DataSpace.OSpace);
            var clrTypes = metadataWorkspace.GetItems<EntityType>(DataSpace.OSpace)
                .Select(x => objectItemCollection.GetClrType(x)).ToList();
            Func<EntityType, Type> getClrType = (eType) =>
            {
                return clrTypes.Single(e => e.FullName == eType.FullName);
            };

            var containerMapping = metadataWorkspace.GetItems<EntityContainerMapping>(DataSpace.CSSpace).Single();

            var typeMappings = ProcessEntitySets(containerMapping.EntitySetMappings, getClrType);

            //Set up inheritance relations
            foreach (var typeMapping in typeMappings)
            {
                typeMapping.BaseTypeMapping = typeMappings.SingleOrDefault(m => m.Type == typeMapping.Type.BaseType);
                typeMapping.DirectSubTypeMappings = typeMappings.Where(m => m.Type.BaseType == typeMapping.Type).ToList();
            }

            ProcessAssociationSets(containerMapping.ConceptualEntityContainer.AssociationSets, containerMapping.AssociationSetMappings, getClrType, typeMappings);

            return typeMappings;
        }

        private static List<TypeMapping> ProcessEntitySets(IEnumerable<EntitySetMapping> entitySetMappings, Func<EntityType, Type> getClrType)
        {
            var typeMappings = new List<TypeMapping>();

            foreach (var entitySetMapping in entitySetMappings)
            {
                foreach (var entityTypeMapping in entitySetMapping.EntityTypeMappings)
                {
                    var entityType = entityTypeMapping.IsHierarchyMapping ? (EntityType)entityTypeMapping.IsOfEntityTypes.Single() : entityTypeMapping.EntityType;
                    var clrType = getClrType(entityType);

                    //Fragments.Count > 1 if Entity Splitting is used.
                    var tableMappings = ProcessFragments(clrType, entityTypeMapping.Fragments);

                    typeMappings.Add(new TypeMapping(clrType, tableMappings));
                }
            }

            return typeMappings;
        }

        private static IEnumerable<TableMapping> ProcessFragments(Type type, IEnumerable<MappingFragment> fragments)
        {
            var tableMappings = new List<TableMapping>();

            foreach (var mappingFragment in fragments)
            {
                var tableName = mappingFragment.StoreEntitySet.Table ?? mappingFragment.StoreEntitySet.Name;
                var propMappings = ProcessFragmentPropertyMappings(type, mappingFragment.PropertyMappings.OfType<ScalarPropertyMapping>(), tableName);

                tableMappings.Add(new TableMapping(tableName, propMappings));
            }

            return tableMappings;
        }

        private static IEnumerable<PropertyMapping> ProcessFragmentPropertyMappings(Type type, IEnumerable<ScalarPropertyMapping> propertyMappings, string tableName)
        {
            var propMappings = new List<PropertyMapping>();

            foreach (var propertyMapping in propertyMappings)
            {
                var propertyName = propertyMapping.Property.Name;
                var columnName = propertyMapping.Column.Name;

                //TODO: Cleaner way to get this information?
                var isPrimaryKeyProperty = typeof(EdmProperty).GetProperty("IsPrimaryKeyColumn", BindingFlags.NonPublic | BindingFlags.Instance);
                bool isPrimaryKey = (bool)isPrimaryKeyProperty.GetValue(propertyMapping.Column);

                propMappings.Add(new PropertyMapping(type.GetProperty(propertyName), columnName, isPrimaryKey, tableName));
            }

            return propMappings;
        }

        private static void ProcessAssociationSets(IEnumerable<AssociationSet> associationSets, IEnumerable<AssociationSetMapping> associationSetMappings,
            Func<EntityType, Type> getClrType, IEnumerable<TypeMapping> typeMappings)
        {
            foreach (var associationSet in associationSets)
            {
                var associationMapping = associationSetMappings.SingleOrDefault(m => m.AssociationSet == associationSet);
                if (associationMapping == null)
                {
                    //FK Association
                    ProcessReferentialConstraint(associationSet, getClrType, typeMappings);
                }
                else
                {
                    //Independent Association
                    ProcessAssociationSetMapping(associationMapping, getClrType, typeMappings);
                }
            }
        }

        private static void ProcessReferentialConstraint(AssociationSet associationSet, Func<EntityType, Type> getClrType, IEnumerable<TypeMapping> typeMappings)
        {
            var assocType = associationSet.ElementType;
            var constraint = assocType.Constraint;

            var sourceType = getClrType(constraint.FromRole.GetEntityType());
            var targetType = getClrType(constraint.ToRole.GetEntityType());

            var sourceTypeMapping = typeMappings.Single(tm => tm.Type == sourceType);
            var targetTypeMapping = typeMappings.Single(tm => tm.Type == targetType);

            var sourcePropMappings = new List<string>();
            foreach (var prop in constraint.FromProperties)
            {
                var propMapping = sourceTypeMapping.Properties.Single(p => p.Property != null && p.Property.Name == prop.Name);
                sourcePropMappings.Add(propMapping.ColumnName);
            }

            var targetPropMappings = new List<string>();
            foreach (var prop in constraint.ToProperties)
            {
                var propMapping = targetTypeMapping.Properties.Single(p => p.Property != null && p.Property.Name == prop.Name);
                targetPropMappings.Add(propMapping.ColumnName);
            }

            var assocMapping = new AssociationMapping(sourceTypeMapping, sourcePropMappings, targetTypeMapping, targetPropMappings);
            sourceTypeMapping.AddAssociationMapping(assocMapping);
            targetTypeMapping.AddAssociationMapping(assocMapping.Reverse());
        }

        private static void ProcessAssociationSetMapping(AssociationSetMapping associationMapping, Func<EntityType, Type> getClrType, IEnumerable<TypeMapping> typeMappings)
        {
            var sourceEndMapping = associationMapping.SourceEndMapping;
            var targetEndMapping = associationMapping.TargetEndMapping;

            var sourceType = getClrType(sourceEndMapping.AssociationEnd.GetEntityType());
            var targetType = getClrType(targetEndMapping.AssociationEnd.GetEntityType());

            var sourceTypeMapping = typeMappings.Single(tm => tm.Type == sourceType);
            var targetTypeMapping = typeMappings.Single(tm => tm.Type == targetType);

            var propMappingsSource = new Dictionary<string, string>();
            foreach (var propertyMapping in sourceEndMapping.PropertyMappings)
            {
                var propMapping = sourceTypeMapping.Properties.Single(p => p.Property != null && p.Property.Name == propertyMapping.Property.Name);
                propMappingsSource.Add(propertyMapping.Column.Name, propMapping.ColumnName);
            }
            var propMappingsTarget = new Dictionary<string, string>();
            foreach (var propertyMapping in targetEndMapping.PropertyMappings)
            {
                var propMapping = targetTypeMapping.Properties.Single(p => p.Property != null && p.Property.Name == propertyMapping.Property.Name);
                propMappingsTarget.Add(propertyMapping.Column.Name, propMapping.ColumnName);
            }

            var mappedTableName = associationMapping.StoreEntitySet.Table ?? associationMapping.StoreEntitySet.Name;
            var mapsToSource = sourceTypeMapping.TableMappings.Any(spm => spm.TableName == mappedTableName);
            var mapsToTarget = targetTypeMapping.TableMappings.Any(spm => spm.TableName == mappedTableName);

            Debug.Assert((!mapsToSource && !mapsToTarget) || (!mapsToSource && mapsToTarget) || (mapsToSource && mapsToTarget));

            //M2M relation
            if (!mapsToSource && !mapsToTarget)
            {
                var assocMapping = new AssociationMapping_M2M(sourceTypeMapping, propMappingsSource.Values.ToList(),
                    targetTypeMapping, propMappingsTarget.Values.ToList(), mappedTableName,
                    propMappingsSource.Keys.ToList(), propMappingsTarget.Keys.ToList());
                sourceTypeMapping.AddAssociationMapping(assocMapping);
                targetTypeMapping.AddAssociationMapping(assocMapping.Reverse());
            }
            else
            {
                var assocMapping = new AssociationMapping(sourceTypeMapping, propMappingsSource.Values.ToList(),
                    targetTypeMapping, propMappingsSource.Keys.ToList());
                sourceTypeMapping.AddAssociationMapping(assocMapping);
                targetTypeMapping.AddAssociationMapping(assocMapping.Reverse());
            }
        }
    }
}
