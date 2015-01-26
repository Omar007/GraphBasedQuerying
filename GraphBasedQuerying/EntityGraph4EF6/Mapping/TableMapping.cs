using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace EntityGraph4EF6.Mapping
{
    internal class TableMapping
    {
        public EntitySetBase EntitySet { get; private set; }

        public IEnumerable<PropertyMapping> PropertyMappings { get; private set; }
        public IEnumerable<PropertyMapping> PrimaryKeyMappings { get; private set; }
        public IEnumerable<PropertyMapping> NonPrimaryKeyMappings { get; private set; }

        public TableMapping(EntitySetBase entitySet, IEnumerable<PropertyMapping> propertyMappings)
        {
            EntitySet = entitySet;
            PropertyMappings = propertyMappings;

            PrimaryKeyMappings = PropertyMappings.Where(pm => pm.IsPrimaryKey).ToList();
            NonPrimaryKeyMappings = PropertyMappings.Where(pm => !pm.IsPrimaryKey).ToList();
        }

        public override bool Equals(object obj)
        {
            TableMapping other;
            if ((other = obj as TableMapping) == null)
            {
                return false;
            }

            return other.EntitySet.Equals(EntitySet) && Enumerable.SequenceEqual(other.PropertyMappings, PropertyMappings);
        }

        public override int GetHashCode()
        {
            return EntitySet.GetHashCode() ^ PropertyMappings.Aggregate(0, (baseValue, pm) => baseValue ^ pm.GetHashCode());
        }
    }
}
