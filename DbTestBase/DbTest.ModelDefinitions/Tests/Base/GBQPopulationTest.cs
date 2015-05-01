using System.Data.Entity;
using DbTest.Core;
using DbTest.Core.Tests;

namespace DbTest.ModelDefinitions.Tests
{
    public abstract class GBQPopulationTest<TContext> : PopulationTest<TContext>, IGBQTest
        where TContext : DbContext
    {
        public GBQPopulationTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }
    }
}
