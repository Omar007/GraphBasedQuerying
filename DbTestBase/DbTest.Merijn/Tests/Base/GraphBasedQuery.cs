using System.Data.Objects;
using System.Data.Objects.DataClasses;
using DbTest.Core;
using DbTest.Core.Tests;
using EntityGraph.SQL;

namespace DbTest.Merijn.Tests
{
    public abstract class GraphBasedQuery<TContext, TBase> : PopulationTest<TContext>, IGBQTest
        where TContext : ObjectContext, new()
        where TBase : EntityObject
    {
        public override string Name { get { return "GraphBasedQuery"; } }

        public GraphBasedQuery(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected virtual EntityGraphShape4SQL Shape
        {
            get
            {
                return new EntityGraphShape4SQL(Context);
            }
        }

        protected abstract TBase StubEntity { get; }

        protected override int DoTest()
        {
            Shape.Load(StubEntity);

            return 0;
        }
    }
}
