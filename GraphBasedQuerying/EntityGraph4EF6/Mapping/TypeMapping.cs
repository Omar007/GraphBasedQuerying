using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityGraph4EF6.SQL;

namespace EntityGraph4EF6.Mapping
{
    internal class TypeMapping
    {
        internal const string TypeColumn = "_typeId_";

        public Type Type { get; private set; }
        public IEnumerable<TableMapping> TableMappings { get; private set; }
        public IEnumerable<AssociationMapping> AssociationMappings { get { return _associationMappings; } }

        public TypeMapping BaseTypeMapping { get; internal set; }
        public IEnumerable<TypeMapping> DirectSubTypeMappings { get; internal set; }

        public IEnumerable<PropertyMapping> Properties { get; private set; }
        public IEnumerable<PropertyMapping> PrimaryKeys { get; private set; }
        public IEnumerable<PropertyMapping> NonPrimaryKeyProperties { get; private set; }

        private readonly ICollection<AssociationMapping> _associationMappings;
        
        public TypeMapping(Type entityType, IEnumerable<TableMapping> tableMappings)
        {
            Type = entityType;
            TableMappings = tableMappings;

            Properties = TableMappings.SelectMany(tm => tm.PropertyMappings).GroupBy(pm => pm.Property, (k, pm) => pm.First()).ToList();
            PrimaryKeys = Properties.Where(m => m.IsPrimaryKey).ToList();
            NonPrimaryKeyProperties = Properties.Where(m => !m.IsPrimaryKey).ToList();

            _associationMappings = new List<AssociationMapping>();
        }

        public void AddAssociationMapping(AssociationMapping mapping)
        {
            _associationMappings.Add(mapping);
        }

        private IEnumerable<When> GetHierarchyWhen(string typeId)
        {
            var whens = new List<When>();
            if (!Type.IsAbstract)
            {
                Expr condition = null;
                foreach (var key in PrimaryKeys)
                {
                    Expr notNullExpr = new IsNotNull(new ColumnExpr(new Column(key.ContainingTableName, key.ColumnName)));

                    condition = condition == null ? notNullExpr : new AndExpr(condition, notNullExpr);
                }
                var when = new When(condition, new ConstantExpr(typeId));

                whens.Add(when);
            }

            whens.AddRange(DirectSubTypeMappings.SelectMany((stm, i) => stm.GetHierarchyWhen(typeId + i + "X")));
            return whens;
        }

        private IEnumerable<ColumnSelect> GetHierarchyColumnSelect()
        {
            var columns = new List<ColumnSelect>();
            columns.AddRange(NonPrimaryKeyProperties.Select(pk => new ColumnSelect(
                pk.ContainingTableName, pk.ColumnName, pk.PropertyName)));
            columns.AddRange(DirectSubTypeMappings.SelectMany(stm => stm.GetHierarchyColumnSelect()));
            return columns;
        }

        private Table GetTable()
        {
            var firstMapping = TableMappings.First();
            var table = new Table(firstMapping.TableName);

            foreach (var tableMapping in TableMappings.Skip(1))
            {
                BinaryExpr condition = null;
                for (int i = 0; i < tableMapping.PrimaryKeyMappings.Count(); i++)
                {
                    BinaryExpr expr = new EqualExpr(
                        new ColumnExpr(new Column(tableMapping.TableName, tableMapping.PrimaryKeyMappings.ElementAt(i).ColumnName)),
                        new ColumnExpr(new Column(firstMapping.TableName, firstMapping.PrimaryKeyMappings.ElementAt(i).ColumnName)));

                    condition = condition == null ? expr : new AndExpr(condition, expr);
                }

                table = new InnerJoin(table, new Table(tableMapping.TableName), condition);
            }

            return table;
        }

        private Table GetHierarchyTable(Table leftOverride)
        {
            var myMapping = TableMappings.First();
            var table = leftOverride ?? GetTable();

            foreach (var subTypeMapping in DirectSubTypeMappings)
            {
                var subTypeTableMapping = subTypeMapping.TableMappings.First();
                BinaryExpr condition = null;
                for (int i = 0; i < subTypeTableMapping.PrimaryKeyMappings.Count(); i++)
                {
                    var subPrimaryKey = subTypeTableMapping.PrimaryKeyMappings.ElementAt(i);

                    BinaryExpr expr = new EqualExpr(
                        new ColumnExpr(new Column(subTypeTableMapping.TableName, subPrimaryKey.ColumnName)),
                        new ColumnExpr(new Column(myMapping.TableName, myMapping.PrimaryKeyMappings.ElementAt(i).ColumnName)));

                    condition = condition == null ? expr : new AndExpr(condition, expr);
                }

                table = new LeftOuterJoin(table, subTypeMapping.GetHierarchyTable(null), condition);
            }

            return table;
        }

        public SelectColumns GetSelectColumns(WhereExpr whereExpr, Table fromOverride = null)
        {
            var columns = new List<ColumnSelect>();
            columns.AddRange(PrimaryKeys.Select(pk => new ColumnSelect(pk.ContainingTableName,
                pk.ColumnName, pk.PropertyName)));
            columns.AddRange(GetHierarchyColumnSelect());
            columns.Add(new Case(TypeColumn, GetHierarchyWhen(String.Empty)));
            return new SelectColumns(columns, GetHierarchyTable(fromOverride), whereExpr);
        }

        public TypeMapping GetFor(string typeId)
        {
            if (String.IsNullOrEmpty(typeId))
            {
                return this;
            }

            var xIndex = typeId.IndexOf('X');
            var select = Int32.Parse(typeId.Substring(0, xIndex));
            var nextTypeId = xIndex + 1 < typeId.Length ? typeId.Substring(xIndex + 1) : null;
            return DirectSubTypeMappings.ElementAt(select).GetFor(nextTypeId);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("SELECT {0}", String.Join(", ", Properties
                .Select(pm => String.Format("{0}.{1} AS {2}", pm.ContainingTableName, pm.ColumnName, pm.PropertyName))));
            sb.AppendLine();

            var baseTableMapping = TableMappings.First();
            sb.AppendFormat("FROM {0}", baseTableMapping.TableName);
            sb.AppendLine();

            foreach (var tableMapping in TableMappings.Skip(1))
            {
                sb.AppendFormat("INNER JOIN {0} ON ", tableMapping.TableName);

                for (int i = 0; i < tableMapping.PrimaryKeyMappings.Count(); i++)
                {
                    if (i > 0)
                    {
                        sb.Append(" AND ");
                    }

                    sb.AppendFormat("{0}.{1} = {2}.{3}",
                        tableMapping.TableName, tableMapping.PrimaryKeyMappings.ElementAt(i).ColumnName,
                        baseTableMapping.TableName, baseTableMapping.PrimaryKeyMappings.ElementAt(i).ColumnName);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            TypeMapping other;
            if ((other = obj as TypeMapping) == null)
            {
                return false;
            }

            return other.Type.Equals(Type) && Enumerable.SequenceEqual(other.TableMappings, TableMappings)
                && other.BaseTypeMapping.Equals(BaseTypeMapping)
                && Enumerable.SequenceEqual(other.DirectSubTypeMappings, DirectSubTypeMappings);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ TableMappings.Aggregate(0, (baseValue, tm) => baseValue ^ tm.GetHashCode())
                ^ BaseTypeMapping.GetHashCode() ^ DirectSubTypeMappings.Aggregate(0, (baseValue, dst) => baseValue ^ dst.GetHashCode());
        }
    }
}
