using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq.Expressions;
using DbTest.Core;
using DbTest.Core.Tests;

namespace DbTest.Merijn.Tests
{
    public abstract class EFMultiQuery<TContext, TBase> : PopulationTest<TContext>, IEFTest
        where TContext : ObjectContext, new()
        where TBase : EntityObject
    {
        public override string Name { get { return "EFMultiQuery"; } }

        public EFMultiQuery(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected abstract Expression<Func<TBase, bool>> GetEntityKey { get; }
    }
}
