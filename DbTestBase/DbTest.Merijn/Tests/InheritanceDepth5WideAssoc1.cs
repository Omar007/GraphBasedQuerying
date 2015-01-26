using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using DbTest.Core;
using EntityGraph.SQL;
using InheritanceDepth5WideAssoc1;

namespace DbTest.Merijn.Tests
{
    public class EFMultiQueryDepth5WideAssoc1 : EFMultiQuery<InheritanceDepth5WideAssoc1ModelContainer, E00>
    {
        public EFMultiQueryDepth5WideAssoc1(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }

        protected override int DoTest()
        {
            Context.E00Set.OfType<E40>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E41>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E42>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E43>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E44>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E45>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E46>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E47>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E48>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E49>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E410>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E411>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E412>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E413>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E414>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E415>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E416>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E417>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E418>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E419>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E420>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E421>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E422>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E423>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.OSet.Where(o => o.Id == SelectedIndex)
                .Join(Context.E00Set, o => o.Id, e00 => e00.OId, (o, e00) => e00)
                .Join(Context.A00Set.OfType<A10>(), e00 => e00.Id, a10 => a10.E00Id, (e00, a10) => a10)
                .Join(Context.B00Set, a10 => a10.Id, b00 => b00.A10Id, (a10, b00) => b00).ToList();

            return 0;
        }
    }

    public class GraphBasedQueryDepth5WideAssoc1 : GraphBasedQuery<InheritanceDepth5WideAssoc1ModelContainer, O>
    {
        public GraphBasedQueryDepth5WideAssoc1(IDatabaseConnection connection, Population population)
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
