using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EntityGraph4EF6.SQL;

namespace EntityGraph4EF6.Mapping
{
    internal class AssociationMapping
    {
        public TypeMapping Source { get; private set; }
        public IEnumerable<string> SourceColumns { get; private set; }

        public TypeMapping Target { get; private set; }
        public IEnumerable<string> TargetColumns { get; private set; }

        //Source is the type with the PK. Target is the type with the FK.
        public AssociationMapping(TypeMapping source, IEnumerable<string> sourceColumns,
            TypeMapping target, IEnumerable<string> targetColumns)
        {
            Source = source;
            SourceColumns = sourceColumns;

            Target = target;
            TargetColumns = targetColumns;

            Debug.Assert(SourceColumns.Count() == TargetColumns.Count());
        }

        //TODO: Account for self-reference associations
        public virtual InnerJoin GetJoin()
        {
            var sourcePropsTableName = Source.TableMappings.First().TableName;
            var targetPropsTableName = Target.TableMappings.First().TableName;

            BinaryExpr condition = null;
            for (int i = 0; i < SourceColumns.Count(); i++)
            {
                var sourceProp = SourceColumns.ElementAt(i);
                var targetProp = TargetColumns.ElementAt(i);

                BinaryExpr expr = new EqualExpr(
                    new ColumnExpr(new Column(sourcePropsTableName, sourceProp)),
                    new ColumnExpr(new Column(targetPropsTableName, targetProp)));

                condition = condition == null ? expr : new AndExpr(condition, expr);
            }

            return new InnerJoin(new Table(sourcePropsTableName), new Table(targetPropsTableName), condition);
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
