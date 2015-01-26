using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using EntityGraph4EF6.Mapping;

namespace EntityGraph4EF6
{
    internal static class TypeMappingExtensions
    {
        private static DbExpressionBinding GetTable(this TypeMapping typeMapping)
        {
            DbExpressionBinding table = null;
            TableMapping prevTableMapping = null;
            foreach (var tableMapping in typeMapping.TableMappings)
            {
                var nextTable = DbExpressionBuilder.Bind(DbExpressionBuilder.Scan(tableMapping.EntitySet));
                if (table == null)
                {
                    table = nextTable;
                    prevTableMapping = tableMapping;
                    continue;
                }

                DbExpression condition = null;
                for (int i = 0; i < tableMapping.PrimaryKeyMappings.Count(); i++)
                {
                    var expr = DbExpressionBuilder.Equal(
                        DbExpressionBuilder.Property(table.Variable, prevTableMapping.PrimaryKeyMappings.ElementAt(i).ColumnProperty),
                        DbExpressionBuilder.Property(nextTable.Variable, tableMapping.PrimaryKeyMappings.ElementAt(i).ColumnProperty)
                    );

                    condition = condition == null ? (DbExpression)expr : DbExpressionBuilder.And(condition, expr);
                }

                table = DbExpressionBuilder.Bind(DbExpressionBuilder.InnerJoin(table, nextTable, condition));
                prevTableMapping = tableMapping;
            }

            return table;
        }

        private static DbExpressionBinding GetHierarchyTable(this TypeMapping typeMapping)
        {
            var baseTable = typeMapping.GetTable();
            DbExpressionBinding table = null;
            foreach (var subTypeMapping in typeMapping.DirectSubTypeMappings)
            {
                DbExpressionBinding subTable = subTypeMapping.GetHierarchyTable();
                if (table == null)
                {
                    table = subTable;
                }

                DbExpression condition = null;
                var subTypeTableMapping = subTypeMapping.TableMappings.First();
                for (int i = 0; i < subTypeTableMapping.PrimaryKeyMappings.Count(); i++)
                {
                    var expr = DbExpressionBuilder.Equal(
                        DbExpressionBuilder.Property(subTable.Variable, subTypeTableMapping.PrimaryKeyMappings.ElementAt(i).ColumnProperty),
                        DbExpressionBuilder.Property(baseTable.Variable, typeMapping.PrimaryKeys.ElementAt(i).ColumnProperty)
                    );

                    condition = condition == null ? (DbExpression)expr : DbExpressionBuilder.And(condition, expr);
                }

                table = DbExpressionBuilder.Bind(DbExpressionBuilder.LeftOuterJoin(table, baseTable, condition));
            }

            if (table == null)
            {
                return baseTable;
            }

            return table;
        }

        public static IDictionary<string, DbExpression> GetHierarchyColumns(this TypeMapping typeMapping)
        {
            var columns = new Dictionary<string, DbExpression>();
            foreach (var npk in typeMapping.NonPrimaryKeyProperties)
            {
                columns.Add(npk.ColumnProperty.Name, DbExpressionBuilder.Property(
                    DbExpressionBuilder.Variable(TypeUsage.CreateDefaultTypeUsage(npk.EntitySet.ElementType), npk.EntitySet.Table),
                    npk.ColumnProperty
                ));
            }
            foreach (var sub in typeMapping.DirectSubTypeMappings.SelectMany(stm => stm.GetHierarchyColumns()))
            {
                columns.Add(sub.Key, sub.Value);
            }
            return columns;
        }

        public static IDictionary<DbExpression, DbConstantExpression> GetHierarchyWhen(this TypeMapping typeMapping, string typeId)
        {
            var whens = new Dictionary<DbExpression, DbConstantExpression>();
            if (!typeMapping.Type.IsAbstract)
            {
                DbExpression condition = null;
                foreach (var key in typeMapping.PrimaryKeys)
                {
                    var notNullExpr = DbExpressionBuilder.Not(
                        DbExpressionBuilder.IsNull(
                            DbExpressionBuilder.Property(
                                DbExpressionBuilder.Variable(TypeUsage.CreateDefaultTypeUsage(key.EntitySet.ElementType), key.EntitySet.Table),
                                key.ColumnProperty
                            )
                        )
                    );

                    condition = condition == null ? (DbExpression)notNullExpr : DbExpressionBuilder.And(condition, notNullExpr);
                }
                whens.Add(condition, DbExpressionBuilder.Constant(typeId));
            }

            foreach (var when in typeMapping.DirectSubTypeMappings.SelectMany((stm, i) => stm.GetHierarchyWhen(typeId + i + "X")))
            {
                whens.Add(when.Key, when.Value);
            }
            return whens;
        }

        public static DbExpression ToDbExpression<T>(this TypeMapping typeMapping, Expression<Func<T, bool>> whereExpr)
            where T : class
        {
            var table = typeMapping.GetHierarchyTable();
            if (whereExpr != null)
            {
                var visitor = new Expression2DbExpressionVisitor(typeMapping, table.Variable);
                visitor.Visit(whereExpr);
                table = DbExpressionBuilder.Bind(DbExpressionBuilder.Filter(table, visitor.DbExpression));
            }

            var columns = new Dictionary<string, DbExpression>();
            foreach (var pk in typeMapping.PrimaryKeys)
            {
                columns.Add(pk.ColumnProperty.Name, DbExpressionBuilder.Property(
                    table.Variable,
                    pk.ColumnProperty));
            }
            foreach (var sel in typeMapping.GetHierarchyColumns())
            {
                columns.Add(sel.Key, sel.Value);
            }
            var whenThens = typeMapping.GetHierarchyWhen(String.Empty);
            columns.Add(TypeMapping.TypeColumn, DbExpressionBuilder.Case(
                whenThens.Keys, whenThens.Values,
                DbExpressionBuilder.Null(TypeUsage.CreateStringTypeUsage(PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String), true, false))
            ));

            return DbExpressionBuilder.Project(table, DbExpressionBuilder.NewRow(columns));
        }
    }
}
