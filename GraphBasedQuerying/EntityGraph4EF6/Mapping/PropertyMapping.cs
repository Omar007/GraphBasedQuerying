using System;
using System.Diagnostics;
using System.Reflection;

namespace EntityGraph4EF6.Mapping
{
    internal class PropertyMapping
    {
        public PropertyInfo Property { get; private set; }
        public string ColumnName { get; private set; }
        public bool IsPrimaryKey { get; private set; }
        public string ContainingTableName { get; private set; }

        public string PropertyName { get { return Property == null ? ColumnName : Property.Name; } }

        public PropertyMapping(PropertyInfo property, string columnName, bool isPrimaryKey, string containingTableName)
        {
            Property = property;
            ColumnName = columnName;
            IsPrimaryKey = isPrimaryKey;
            ContainingTableName = containingTableName;
        }

        public override string ToString()
        {
            return String.Format("{0} AS {1}", ColumnName, PropertyName);
        }

        public override bool Equals(object obj)
        {
            PropertyMapping other;
            if ((other = obj as PropertyMapping) == null)
            {
                return false;
            }

            Debug.Assert(other.PropertyName.Equals(PropertyName) ? other.Property.Equals(Property) : true);

            return other.ColumnName.Equals(ColumnName) && other.IsPrimaryKey.Equals(IsPrimaryKey) && other.PropertyName.Equals(PropertyName);
        }

        public override int GetHashCode()
        {
            return ColumnName.GetHashCode() ^ IsPrimaryKey.GetHashCode() ^ PropertyName.GetHashCode();
        }
    }
}
