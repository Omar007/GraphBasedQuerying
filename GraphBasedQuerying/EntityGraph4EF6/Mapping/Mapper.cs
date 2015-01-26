using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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



            //var set = containerMapping.StoreEntityContainer.EntitySets.ElementAt(2);
            //var table = DbExpressionBuilder.Bind(DbExpressionBuilder.Scan(set));
            ////var where = DbExpressionBuilder.Equal(
            ////    DbExpressionBuilder.Property(table.Variable, set.ElementType.Properties.First()),
            ////    DbExpressionBuilder.Constant(23)
            ////);
            ////var whereOnTable = DbExpressionBuilder.Bind(DbExpressionBuilder.Filter(table, where));
            ////var project = DbExpressionBuilder.Project(whereOnTable, DbExpressionBuilder.NewRow(
            ////    set.ElementType.Properties.Select(x =>
            ////    {
            ////        var propDbExpr = DbExpressionBuilder.Property(whereOnTable.Variable, x);
            ////        return new KeyValuePair<string, DbExpression>(propDbExpr.Property.Name, propDbExpr);
            ////    })
            ////));


            //var joinSet = containerMapping.StoreEntityContainer.EntitySets.ElementAt(1);
            //var joinTable = DbExpressionBuilder.Bind(DbExpressionBuilder.Scan(joinSet));
            //var join = DbExpressionBuilder.Bind(
            //    DbExpressionBuilder.InnerJoin(
            //        table,
            //        joinTable,
            //        DbExpressionBuilder.Equal(
            //            DbExpressionBuilder.Property(table.Variable, set.ElementType.Properties.First()),
            //            DbExpressionBuilder.Property(joinTable.Variable, joinSet.ElementType.Properties.Last())
            //        )
            //    )
            //);
            //var where = DbExpressionBuilder.Equal(
            //    DbExpressionBuilder.Property(DbExpressionBuilder.Property(join.Variable, table.VariableName), set.ElementType.Properties.First()),
            //    DbExpressionBuilder.Constant(23)
            //);
            //var whereOnJoin = DbExpressionBuilder.Bind(DbExpressionBuilder.Filter(join, where));
            //var project = DbExpressionBuilder.Project(whereOnJoin, DbExpressionBuilder.NewRow(
            //    joinSet.ElementType.Properties.Select(x =>
            //    {
            //        var propDbExpr = DbExpressionBuilder.Property(DbExpressionBuilder.Property(whereOnJoin.Variable, joinTable.VariableName), x);
            //        return new KeyValuePair<string, DbExpression>(propDbExpr.Property.Name, propDbExpr);
            //    })
            //));

            //var commandTree = new DbQueryCommandTree(context.GetObjectContext().MetadataWorkspace, DataSpace.SSpace, project);
            //var commandDef = DbProviderServices.GetProviderServices(context.Database.Connection).CreateCommandDefinition(commandTree);
            //var command = commandDef.CreateCommand();
            //command.Connection = context.Database.Connection;
            //command.Connection.Open();
            //var er = command.ExecuteReader();



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
                var propMappings = ProcessFragmentPropertyMappings(type, mappingFragment.PropertyMappings.OfType<ScalarPropertyMapping>(), mappingFragment.StoreEntitySet);

                tableMappings.Add(new TableMapping(mappingFragment.StoreEntitySet, propMappings));
            }

            return tableMappings;
        }

        private static IEnumerable<PropertyMapping> ProcessFragmentPropertyMappings(Type type, IEnumerable<ScalarPropertyMapping> propertyMappings, EntitySetBase entitySet)
        {
            var propMappings = new List<PropertyMapping>();

            foreach (var propertyMapping in propertyMappings)
            {
                //TODO: Cleaner way to get this information?
                var isPrimaryKeyProperty = typeof(EdmProperty).GetProperty("IsPrimaryKeyColumn", BindingFlags.NonPublic | BindingFlags.Instance);
                bool isPrimaryKey = (bool)isPrimaryKeyProperty.GetValue(propertyMapping.Column);

                propMappings.Add(new PropertyMapping(entitySet, propertyMapping.Column, type.GetProperty(propertyMapping.Property.Name), isPrimaryKey));
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

            var sourcePropMappings = new List<EdmProperty>();
            foreach (var prop in constraint.FromProperties)
            {
                var propMapping = sourceTypeMapping.Properties.Single(p => p.Property != null && p.Property.Name == prop.Name);
                sourcePropMappings.Add(propMapping.ColumnProperty);
            }

            var targetPropMappings = new List<EdmProperty>();
            foreach (var prop in constraint.ToProperties)
            {
                var propMapping = targetTypeMapping.Properties.Single(p => p.Property != null && p.Property.Name == prop.Name);
                targetPropMappings.Add(propMapping.ColumnProperty);
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

            var propMappingsSource = new Dictionary<EdmProperty, EdmProperty>();
            foreach (var propertyMapping in sourceEndMapping.PropertyMappings)
            {
                var propMapping = sourceTypeMapping.Properties.Single(p => p.Property != null && p.Property.Name == propertyMapping.Property.Name);
                propMappingsSource.Add(propertyMapping.Column, propMapping.ColumnProperty);
            }
            var propMappingsTarget = new Dictionary<EdmProperty, EdmProperty>();
            foreach (var propertyMapping in targetEndMapping.PropertyMappings)
            {
                var propMapping = targetTypeMapping.Properties.Single(p => p.Property != null && p.Property.Name == propertyMapping.Property.Name);
                propMappingsTarget.Add(propertyMapping.Column, propMapping.ColumnProperty);
            }

            var mappedTable = associationMapping.StoreEntitySet;
            var mapsToSource = sourceTypeMapping.TableMappings.Any(spm => spm.EntitySet == mappedTable);
            var mapsToTarget = targetTypeMapping.TableMappings.Any(spm => spm.EntitySet == mappedTable);

            Debug.Assert((!mapsToSource && !mapsToTarget) || (!mapsToSource && mapsToTarget) || (mapsToSource && mapsToTarget));

            //M2M relation
            if (!mapsToSource && !mapsToTarget)
            {
                var assocMapping = new AssociationMapping_M2M(sourceTypeMapping, propMappingsSource.Values.ToList(),
                    targetTypeMapping, propMappingsTarget.Values.ToList(), mappedTable,
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
