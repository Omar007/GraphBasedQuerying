using System;
using System.Collections.Generic;
using System.Linq;
using EntityGraph4EF6.SQL;

namespace EntityGraph4EF6.Mapping
{
    internal class AssociationMapping_M2M : AssociationMapping
    {
        public string LinkTableName { get; private set; }
        public IEnumerable<string> LinkTableSourceColumns { get; private set; }
        public IEnumerable<string> LinkTableTargetColumns { get; private set; }

        public AssociationMapping_M2M(TypeMapping source, IEnumerable<string> sourceColumns,
            TypeMapping target, IEnumerable<string> targetColumns, string linkTableName,
            IEnumerable<string> linkTableSourceColumns, IEnumerable<string> linkTableTargetColumns)
            : base(source, sourceColumns, target, targetColumns)
        {
            LinkTableName = linkTableName;
            LinkTableSourceColumns = linkTableSourceColumns;
            LinkTableTargetColumns = linkTableTargetColumns;
        }

        public override InnerJoin GetJoin()
        {
            var sourcePropsTableName = Source.TableMappings.First().TableName;
            var targetPropsTableName = Target.TableMappings.First().TableName;

            BinaryExpr condition = null;
            for (int i = 0; i < SourceColumns.Count(); i++)
            {
                var sourceProp = SourceColumns.ElementAt(i);
                var linkSourceProp = LinkTableSourceColumns.ElementAt(i);

                EqualExpr expr = new EqualExpr(
                    new ColumnExpr(new Column(sourcePropsTableName, sourceProp)),
                    new ColumnExpr(new Column(LinkTableName, linkSourceProp)));

                condition = condition == null ? (BinaryExpr)expr : new AndExpr(condition, expr);
            }
            var join = new InnerJoin(new Table(sourcePropsTableName), new Table(LinkTableName), condition);

            condition = null;
            for (int i = 0; i < TargetColumns.Count(); i++)
            {
                var targetProp = TargetColumns.ElementAt(i);
                var linkTargetProp = LinkTableTargetColumns.ElementAt(i);

                EqualExpr expr = new EqualExpr(
                    new ColumnExpr(new Column(LinkTableName, linkTargetProp)),
                    new ColumnExpr(new Column(targetPropsTableName, targetProp)));

                condition = condition == null ? (BinaryExpr)expr : new AndExpr(condition, expr);
            }
            return new InnerJoin(join, new Table(targetPropsTableName), condition);
        }

        public override AssociationMapping Reverse()
        {
            return new AssociationMapping_M2M(Target, TargetColumns, Source, SourceColumns,
                LinkTableName, LinkTableTargetColumns, LinkTableSourceColumns);
        }

        public override string ToString()
        {
            return String.Format("{0}({1}) -> {4}({5}|{6}) -> {2}({3})", Source.Type.Name,
                String.Join(", ", SourceColumns), Target.Type.Name,
                String.Join(", ", TargetColumns), LinkTableName,
                String.Join(", ", LinkTableSourceColumns), String.Join(", ", LinkTableTargetColumns));
        }

        public override bool Equals(object obj)
        {
            AssociationMapping_M2M other;
            if ((other = obj as AssociationMapping_M2M) == null)
            {
                return false;
            }

            return other.LinkTableName.Equals(LinkTableName)
                && Enumerable.SequenceEqual(other.LinkTableSourceColumns, LinkTableSourceColumns)
                && Enumerable.SequenceEqual(other.LinkTableTargetColumns, LinkTableTargetColumns)
                && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return LinkTableName.GetHashCode()
                ^ LinkTableSourceColumns.Aggregate(0, (baseValue, sc) => baseValue ^ sc.GetHashCode())
                ^ LinkTableTargetColumns.Aggregate(0, (baseValue, tc) => baseValue ^ tc.GetHashCode())
                ^ base.GetHashCode();
        }
    }
}
