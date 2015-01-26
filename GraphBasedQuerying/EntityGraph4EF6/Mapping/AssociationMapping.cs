using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;

namespace EntityGraph4EF6.Mapping
{
    internal class AssociationMapping
    {
        public TypeMapping Source { get; private set; }
        public IEnumerable<EdmProperty> SourceColumns { get; private set; }

        public TypeMapping Target { get; private set; }
        public IEnumerable<EdmProperty> TargetColumns { get; private set; }

        //Source is the type with the PK. Target is the type with the FK.
        public AssociationMapping(TypeMapping source, IEnumerable<EdmProperty> sourceColumns,
            TypeMapping target, IEnumerable<EdmProperty> targetColumns)
        {
            Source = source;
            SourceColumns = sourceColumns;

            Target = target;
            TargetColumns = targetColumns;

            Debug.Assert(SourceColumns.Count() == TargetColumns.Count());
        }

        //TODO: Account for self-reference associations
        public virtual DbJoinExpression GetJoin()
        {
            var sourcePropsTable = Source.TableMappings.First().EntitySet;
            var targetPropsTable = Target.TableMappings.First().EntitySet;

            var sourceTableDbExpr = DbExpressionBuilder.Bind(DbExpressionBuilder.Scan(sourcePropsTable));
            var targetTableDbExpr = DbExpressionBuilder.Bind(DbExpressionBuilder.Scan(targetPropsTable));

            DbExpression condition = null;
            for (int i = 0; i < SourceColumns.Count(); i++)
            {
                var expr = DbExpressionBuilder.Equal(
                    DbExpressionBuilder.Property(sourceTableDbExpr.Variable, SourceColumns.ElementAt(i)),
                    DbExpressionBuilder.Property(targetTableDbExpr.Variable, TargetColumns.ElementAt(i))
                );

                condition = condition == null ? (DbExpression)expr : DbExpressionBuilder.And(condition, expr);
            }

            return DbExpressionBuilder.InnerJoin(sourceTableDbExpr, targetTableDbExpr, condition);
        }

        public virtual AssociationMapping Reverse()
        {
            return new AssociationMapping(Target, TargetColumns, Source, SourceColumns);
        }

        public override string ToString()
        {
            return String.Format("{0}({1}) -> {2}({3})", Source.Type.Name, String.Join(", ", SourceColumns),
                Target.Type.Name, String.Join(", ", TargetColumns));
        }

        public override bool Equals(object obj)
        {
            AssociationMapping other;
            if ((other = obj as AssociationMapping) == null)
            {
                return false;
            }

            return other.Source.Equals(Source) && other.Target.Equals(Target)
                && Enumerable.SequenceEqual(other.SourceColumns, SourceColumns)
                && Enumerable.SequenceEqual(other.TargetColumns, TargetColumns);
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode() ^ Target.GetHashCode()
                ^ SourceColumns.Aggregate(0, (baseValue, sc) => baseValue ^ sc.GetHashCode())
                ^ TargetColumns.Aggregate(0, (baseValue, tc) => baseValue ^ tc.GetHashCode());
        }
    }
}
