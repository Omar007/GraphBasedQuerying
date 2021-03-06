using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using DbTest.Core;
using EntityGraph.SQL;
using InheritanceDepth3Assoc1;

namespace DbTest.Merijn.Tests
{
    public class EFMultiQueryDepth3Assoc1 : EFMultiQuery<InheritanceDepth3Assoc1ModelContainer, E00>
    {
        public EFMultiQueryDepth3Assoc1(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }

        protected override int DoTest()
        {
            Context.E00Set.OfType<E20>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E21>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E22>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E23>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.OSet.Where(o => o.Id == SelectedIndex)
                .Join(Context.E00Set, o => o.Id, e00 => e00.OId, (o, e00) => e00)
                .Join(Context.A00Set.OfType<A10>(), e00 => e00.Id, a10 => a10.E00Id, (e00, a10) => a10)
                .Join(Context.B00Set, a10 => a10.Id, b00 => b00.A10Id, (a10, b00) => b00).ToList();

            return 0;
        }
    }

    public class GraphBasedQueryDepth3Assoc1 : GraphBasedQuery<InheritanceDepth3Assoc1ModelContainer, O>
    {
        public GraphBasedQueryDepth3Assoc1(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override O StubEntity
        {
            get { return new O { Id = SelectedIndex }; }
        }

        protected override EntityGraphShape4SQL Shape
        {
            get
            {
                return base.Shape
                    .Edge<O, E00>(x => x.E00Set)
                    .Edge<E00, A00>(x => x.A00Set)
                    .Edge<A10, B00>(x => x.B00Set);
            }
        }
    }
}
