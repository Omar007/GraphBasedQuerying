using System.Diagnostics;
using System.Linq;
using EntityGraph4EF6.SQL;

namespace EntityGraph4EF6
{
    internal static class GraphPathExtensions
    {
        public static SelectColumns ToSql(this GraphPath graphPath, WhereExpr whereExpr)
        {
            var startNode = graphPath.First();
            var endNode = graphPath.Last();

            Debug.Assert(startNode != endNode);

            InnerJoin fullJoinSequence = null;
            for (int i = 1; i < graphPath.Count(); i++)
            {
                InnerJoin join = null;
                var node = graphPath[i];
                if (node.InEdge == null)
                {
                    join = Inheritance2Sql(graphPath[i - 1], node);
                }
                else
                {
                    join = Association2Sql(graphPath.GetOwningNode(node), node);
                }

                fullJoinSequence = fullJoinSequence == null ? join : new InnerJoin(fullJoinSequence, join.JoinTable, join.OnExpression);
            }

            return endNode.TypeMapping.GetSelectColumns(whereExpr, fullJoinSequence);
        }

        private static InnerJoin Inheritance2Sql(GraphPathNode leftNode, GraphPathNode rightNode)
        {
            var left = leftNode.TypeMapping;
            var right = rightNode.TypeMapping;

            var leftTableMapping = left.TableMappings.First();
            var rightTableMapping = right.TableMappings.First();

            var leftColumns = leftTableMapping.PrimaryKeyMappings.Select(pkm => pkm.ColumnName).ToList();
            var rightColumns = rightTableMapping.PrimaryKeyMappings.Select(pkm => pkm.ColumnName).ToList();

            BinaryExpr condition = null;
            for (int i = 0; i < leftColumns.Count; i++)
            {
                var leftColumn = leftColumns[i];
                var rightColumn = rightColumns[i];

                EqualExpr expr = new EqualExpr(
                    new ColumnExpr(new Column(leftTableMapping.TableName, leftColumn)),
                    new ColumnExpr(new Column(rightTableMapping.TableName, rightColumn)));

                condition = condition == null ? (BinaryExpr)expr : new AndExpr(condition, expr);
            }

            var leftTable = new Table(leftTableMapping.TableName);
            var rightTable = new Table(rightTableMapping.TableName);
            return new InnerJoin(leftTable, rightTable, condition);
        }

        private static InnerJoin Association2Sql(GraphPathNode leftNode, GraphPathNode rightNode)
        {
            var leftMapping = leftNode.TypeMapping;
            var rightMapping = rightNode.TypeMapping;

            var association = leftMapping.AssociationMappings.Single(am => am.Target == rightMapping);

            return association.GetJoin();
        }
    }
}
