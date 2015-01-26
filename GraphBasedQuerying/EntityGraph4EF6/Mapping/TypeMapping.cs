using System;
using System.Collections.Generic;
using System.Linq;

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
