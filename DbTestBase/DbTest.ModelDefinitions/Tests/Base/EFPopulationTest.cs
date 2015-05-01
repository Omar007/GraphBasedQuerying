using System.Data.Entity;
using DbTest.Core;
using DbTest.Core.Tests;

namespace DbTest.ModelDefinitions.Tests
{
    public abstract class EFPopulationTest<TContext> : PopulationTest<TContext>, IEFTest
        where TContext : DbContext
    {
        public EFPopulationTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }
    }
}
