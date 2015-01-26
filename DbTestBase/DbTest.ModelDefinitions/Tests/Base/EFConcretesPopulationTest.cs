using System.Data.Entity;
using DbTest.Core;
using DbTest.Core.Tests;

namespace DbTest.ModelDefinitions.Tests
{
    public abstract class EFConcretesPopulationTest<TContext> : PopulationTest<TContext>, IEFConcretesTest
        where TContext : DbContext
    {
        public EFConcretesPopulationTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }
    }
}
