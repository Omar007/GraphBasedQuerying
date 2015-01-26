using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Graph4EF
{
    internal static class DbContextExtensions
    {
        internal static ObjectContext GetObjectContext(this DbContext context)
        {
            return ((IObjectContextAdapter)context).ObjectContext;
        }
    }
}
