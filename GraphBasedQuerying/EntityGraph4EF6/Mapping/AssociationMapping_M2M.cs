using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;

namespace EntityGraph4EF6.Mapping
{
    internal class AssociationMapping_M2M : AssociationMapping
    {
        public EntitySetBase LinkTable { get; private set; }
        public IEnumerable<EdmProperty> LinkTableSourceColumns { get; private set; }
        public IEnumerable<EdmProperty> LinkTableTargetColumns { get; private set; }

        public AssociationMapping_M2M(TypeMapping source, IEnumerable<EdmProperty> sourceColumns,
            TypeMapping target, IEnumerable<EdmProperty> targetColumns, EntitySetBase linkTable,
            IEnumerable<EdmProperty> linkTableSourceColumns, IEnumerable<EdmProperty> linkTableTargetColumns)
            : base(source, sourceColumns, target, targetColumns)
        {
            LinkTable = linkTable;
            LinkTableSourceColumns = linkTableSourceColumns;
            LinkTableTargetColumns = linkTableTargetColumns;
        }

        public override DbJoinExpression GetJoin()
        {
            Debug.Assert(SourceColumns.Count() == LinkTableSourceColumns.Count());
            Debug.Assert(TargetColumns.Count() == LinkTableTargetColumns.Count());

            var sourcePropsTable = Source.TableMappings.First().EntitySet;
            var targetPropsTable = Target.TableMappings.First().EntitySet;

            var sourceTableDbExpr = DbExpressionBuilder.Bind(DbExpressionBuilder.Scan(sourcePropsTable));
            var linkTableDbExpr = DbExpressionBuilder.Bind(DbExpressionBuilder.Scan(LinkTable));
            var targetTableDbExpr = DbExpressionBuilder.Bind(DbExpressionBuilder.Scan(targetPropsTable));

            DbExpression condition = null;
            for (int i = 0; i < SourceColumns.Count(); i++)
            {
                var expr = DbExpressionBuilder.Equal(
                    DbExpressionBuilder.Property(sourceTableDbExpr.Variable, SourceColumns.ElementAt(i)),
                    DbExpressionBuilder.Property(linkTableDbExpr.Variable, LinkTableSourceColumns.ElementAt(i))
                );

                condition = condition == null ? (DbExpression)expr : DbExpressionBuilder.And(condition, expr);
            }
            var join = DbExpressionBuilder.InnerJoin(sourceTableDbExpr, linkTableDbExpr, condition);

            condition = null;
            for (int i = 0; i < TargetColumns.Count(); i++)
            {
                var expr = DbExpressionBuilder.Equal(
                    DbExpressionBuilder.Property(linkTableDbExpr.Variable, LinkTableTargetColumns.ElementAt(i)),
                    DbExpressionBuilder.Property(targetTableDbExpr.Variable, TargetColumns.ElementAt(i))
                );

                condition = condition == null ? (DbExpression)expr : DbExpressionBuilder.And(condition, expr);
            }

            return DbExpressionBuilder.InnerJoin(DbExpressionBuilder.Bind(join), targetTableDbExpr, condition);
        }

        public override AssociationMapping Reverse()
        {
            return new AssociationMapping_M2M(Target, TargetColumns, Source, SourceColumns,
                LinkTable, LinkTableTargetColumns, LinkTableSourceColumns);
        }

        public override string ToString()
        {
            return String.Format("{0}({1}) -> {4}({5}|{6}) -> {2}({3})", Source.Type.Name,
                String.Join(", ", SourceColumns), Target.Type.Name,
                String.Join(", ", TargetColumns), LinkTable,
                String.Join(", ", LinkTableSourceColumns), String.Join(", ", LinkTableTargetColumns));
        }

        public override bool Equals(object obj)
        {
            AssociationMapping_M2M other;
            if ((other = obj as AssociationMapping_M2M) == null)
            {
                return false;
            }

            return other.LinkTable.Equals(LinkTable)
                && Enumerable.SequenceEqual(other.LinkTableSourceColumns, LinkTableSourceColumns)
                && Enumerable.SequenceEqual(other.LinkTableTargetColumns, LinkTableTargetColumns)
                && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return LinkTable.GetHashCode()
                ^ LinkTableSourceColumns.Aggregate(0, (baseValue, sc) => baseValue ^ sc.GetHashCode())
                ^ LinkTableTargetColumns.Aggregate(0, (baseValue, tc) => baseValue ^ tc.GetHashCode())
                ^ base.GetHashCode();
        }
    }
}
