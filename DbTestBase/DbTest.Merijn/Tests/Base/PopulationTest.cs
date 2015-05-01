using System;
using System.Data;
using System.Data.Objects;
using System.Linq;
using DbTest.Core;

namespace DbTest.Merijn.Tests
{
    public abstract class PopulationTest<TContext> : Core.Tests.PopulationTest
        where TContext : ObjectContext, new()
    {
        public override string ModelName
        {
            get
            {
                string prefix = "Inheritance";
                string typeName = GetType().Name;
                if (typeName.StartsWith("GraphBasedQuery"))
                {
                    return prefix + typeName.Remove(0, "GraphBasedQuery".Length);
                }
                else if (typeName.StartsWith("EFSingleQuery"))
                {
                    return prefix + typeName.Remove(0, "EFSingleQuery".Length);
                }
                else if (typeName.StartsWith("EFMultiQuery"))
                {
                    return prefix + typeName.Remove(0, "EFMultiQuery".Length);
                }
                throw new Exception("Unsupported query " + typeName);
            }
        }

        protected TContext Context { get; private set; }

        public PopulationTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override void Setup()
        {
            Context = new TContext();
            Context.Connection.ConnectionString = Connection.CreateConnectionString(ModelName, Population);
            Context.ContextOptions.LazyLoadingEnabled = false;
        }

        protected override void Cleanup()
        {
#if DEBUG
            Console.WriteLine("Loaded objects: " + Context.ObjectStateManager.GetObjectStateEntries(EntityState.Unchanged).Count());
#endif

            Context.Dispose();
            Context = null;
        }
    }
}
