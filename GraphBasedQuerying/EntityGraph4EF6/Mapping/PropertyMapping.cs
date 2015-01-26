using System.Data.Entity.Core.Metadata.Edm;
using System.Reflection;

namespace EntityGraph4EF6.Mapping
{
    internal class PropertyMapping
    {
        public EntitySetBase EntitySet { get; private set; }
        public EdmProperty ColumnProperty { get; private set; }
        public PropertyInfo Property { get; private set; }
        public bool IsPrimaryKey { get; private set; }

        public PropertyMapping(EntitySetBase entitySet, EdmProperty storeProperty, PropertyInfo property, bool isPrimaryKey)
        {
            EntitySet = entitySet;
            ColumnProperty = storeProperty;
            Property = property;
            IsPrimaryKey = isPrimaryKey;
        }
    }
}
