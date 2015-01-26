using System;
using System.Data.Entity;
using System.Linq;
using DbTest.Core;
using DbTest.Core.Tests;

namespace DbTest.ModelDefinitions.Tests
{
    public abstract class PopulationTest<TContext> : PopulationTest
        where TContext : DbContext
    {
        protected TContext Context { get; private set; }

        public PopulationTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected abstract TContext GetDbContext();

        protected override void Setup()
        {
            Context = GetDbContext();
            Context.Configuration.LazyLoadingEnabled = false;

#if DEBUG
            Context.Database.Log = Console.Out.WriteLine;
#endif
        }

        protected override void Cleanup()
        {
#if DEBUG
            Console.WriteLine("Loaded objects: " + Context.ChangeTracker.Entries().Where(x => x.State == EntityState.Unchanged).Count());
#endif

            Context.Dispose();
            Context = null;
        }
    }
}
