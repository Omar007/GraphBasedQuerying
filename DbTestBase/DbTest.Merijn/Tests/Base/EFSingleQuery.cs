using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using DbTest.Core;
using DbTest.Core.Tests;

namespace DbTest.Merijn.Tests
{
    public abstract class EFSingleQuery<TContext, TBase> : PopulationTest<TContext>, IEFConcretesTest
        where TContext : ObjectContext, new()
        where TBase : EntityObject
    {
        public override string Name { get { return "EFSingleQuery"; } }

        public EFSingleQuery(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected virtual ObjectQuery<TBase> ObjectQuery
        {
            get
            {
                return Context.CreateObjectSet<TBase>() as ObjectQuery<TBase>;
            }
        }

        protected abstract Expression<Func<TBase, bool>> GetEntityKey { get; }

        protected override int DoTest()
        {
            ObjectQuery.Where(GetEntityKey).ToList();

            return 0;
        }
    }
}
