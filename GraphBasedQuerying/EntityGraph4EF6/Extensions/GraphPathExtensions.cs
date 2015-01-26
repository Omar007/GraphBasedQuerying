using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using EntityGraph4EF6.Mapping;

namespace EntityGraph4EF6
{
    internal static class GraphPathExtensions
    {
        public static DbExpression ToDbExpression<T>(this GraphPath graphPath, TypeMapping whereTypeMapping, Expression<Func<T, bool>> whereExpr)
            where T : class
        {
            var startNode = graphPath.First();
            var endNode = graphPath.Last();

            Debug.Assert(startNode != endNode);

            DbExpressionBinding fullJoinSequence = null;
            for (int i = 1; i < graphPath.Count(); i++)
            {
                DbJoinExpression join = null;
                var node = graphPath[i];
                if (node.InEdge == null)
                {
                    join = Inheritance2DbExpression(graphPath[i - 1], node);
                }
                else
                {
                    join = Association2DbExpression(graphPath.GetOwningNode(node), node);
                }

                fullJoinSequence = fullJoinSequence == null
                    ? DbExpressionBuilder.Bind(join)
                    : DbExpressionBuilder.Bind(DbExpressionBuilder.InnerJoin(fullJoinSequence, join.Right, join.JoinCondition));
            }

            if (whereExpr != null)
            {
                var visitor = new Expression2DbExpressionVisitor(whereTypeMapping, fullJoinSequence.Variable);
                visitor.Visit(whereExpr);
                fullJoinSequence = DbExpressionBuilder.BindAs(DbExpressionBuilder.Filter(fullJoinSequence, visitor.DbExpression), fullJoinSequence.VariableName);
            }

            var columns = new Dictionary<string, DbExpression>();
            foreach (var pk in endNode.TypeMapping.PrimaryKeys)
            {
                columns.Add(pk.ColumnProperty.Name, DbExpressionBuilder.Property(
                    //fullJoinSequence.Variable,
                    DbExpressionBuilder.Variable(TypeUsage.CreateDefaultTypeUsage(pk.EntitySet.ElementType), pk.EntitySet.Table),
                    pk.ColumnProperty));
            }
            foreach (var sel in endNode.TypeMapping.GetHierarchyColumns())
            {
                columns.Add(sel.Key, sel.Value);
            }
            var whenThens = endNode.TypeMapping.GetHierarchyWhen(String.Empty);
            columns.Add(TypeMapping.TypeColumn, DbExpressionBuilder.Case(
                whenThens.Keys, whenThens.Values,
                DbExpressionBuilder.Null(TypeUsage.CreateStringTypeUsage(PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String), true, false))
            ));

            return DbExpressionBuilder.Project(fullJoinSequence, DbExpressionBuilder.NewRow(columns));
        }

        private static DbJoinExpression Inheritance2DbExpression(GraphPathNode leftNode, GraphPathNode rightNode)
        {
            var leftTableMapping = leftNode.TypeMapping.TableMappings.First();
            var rightTableMapping = rightNode.TypeMapping.TableMappings.First();

            var leftColumns = leftTableMapping.PrimaryKeyMappings.Select(pkm => pkm.ColumnProperty).ToList();
            var rightColumns = rightTableMapping.PrimaryKeyMappings.Select(pkm => pkm.ColumnProperty).ToList();

            var leftTableDbExpr = DbExpressionBuilder.Bind(DbExpressionBuilder.Scan(leftTableMapping.EntitySet));
            var rightTableDbExpr = DbExpressionBuilder.Bind(DbExpressionBuilder.Scan(rightTableMapping.EntitySet));

            DbExpression condition = null;
            for (int i = 0; i < leftColumns.Count; i++)
            {
                var expr = DbExpressionBuilder.Equal(
                    DbExpressionBuilder.Property(leftTableDbExpr.Variable, leftColumns[i]),
                    DbExpressionBuilder.Property(rightTableDbExpr.Variable, rightColumns[i])
                );

                condition = condition == null ? (DbExpression)expr : DbExpressionBuilder.And(condition, expr);
            }

            return DbExpressionBuilder.InnerJoin(leftTableDbExpr, rightTableDbExpr, condition);
        }

        private static DbJoinExpression Association2DbExpression(GraphPathNode leftNode, GraphPathNode rightNode)
        {
            var leftMapping = leftNode.TypeMapping;
            var rightMapping = rightNode.TypeMapping;
            var association = leftMapping.AssociationMappings.Single(am => am.Target == rightMapping);

            return association.GetJoin();
        }
    }
}
