using System.Data.Entity;
using System.Linq;
using System.Reflection;
using DbTest.ModelDefinitions.Generators;

namespace Postgresql.DbHelpers
{
    public class PostgresqlConfiguration : DbConfiguration
    {
        public PostgresqlConfiguration()
        {
            var assembly = Assembly.GetAssembly(typeof(Generator));
            var contextTypes = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(DbContext)));

            MethodInfo method = typeof(DbConfiguration).GetMethod("SetDatabaseInitializer", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var contextType in contextTypes)
            {
                MethodInfo genericMethod = method.MakeGenericMethod(contextType);
                genericMethod.Invoke(this, new object[]{ null });
            }
        }
    }
}
