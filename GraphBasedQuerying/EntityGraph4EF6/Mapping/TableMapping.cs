using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph4EF6.Mapping
{
    internal class TableMapping
    {
        public string TableName { get; private set; }
        public IEnumerable<PropertyMapping> PropertyMappings { get; private set; }

        public IEnumerable<PropertyMapping> PrimaryKeyMappings { get; private set; }
        public IEnumerable<PropertyMapping> NonPrimaryKeyMappings { get; private set; }

        public TableMapping(string tableName, IEnumerable<PropertyMapping> propertyMappings)
        {
            TableName = tableName;
            PropertyMappings = propertyMappings;

            PrimaryKeyMappings = PropertyMappings.Where(pm => pm.IsPrimaryKey).ToList();
            NonPrimaryKeyMappings = PropertyMappings.Where(pm => !pm.IsPrimaryKey).ToList();
        }

        public override string ToString()
        {
            return String.Format("SELECT {0} FROM {1}",
                String.Join(", ", PropertyMappings.Select(pm => pm.ToString())),
                TableName);
        }

        public override bool Equals(object obj)
        {
            TableMapping other;
            if ((other = obj as TableMapping) == null)
            {
                return false;
            }

            return other.TableName.Equals(TableName) && Enumerable.SequenceEqual(other.PropertyMappings, PropertyMappings);
        }

        public override int GetHashCode()
        {
            return TableName.GetHashCode() ^ PropertyMappings.Aggregate(0, (baseValue, pm) => baseValue ^ pm.GetHashCode());
        }
    }
}
