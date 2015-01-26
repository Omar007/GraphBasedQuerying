using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using DbTest.Core;
using EntityGraph.SQL;
using InheritanceDepth6Assoc2;

namespace DbTest.Merijn.Tests
{
    public class EFMultiQueryDepth6Assoc2 : EFMultiQuery<InheritanceDepth6Assoc2ModelContainer, E00>
    {
        public EFMultiQueryDepth6Assoc2(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }

        protected override int DoTest()
        {
            Context.E00Set.OfType<E50>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E51>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E52>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E53>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E54>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E55>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E56>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E57>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E58>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E59>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E510>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E511>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E512>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E513>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E514>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E515>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E516>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E517>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E518>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E519>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E520>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E521>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E522>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E523>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E524>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E525>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E526>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E527>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E528>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E529>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E530>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E531>().Include("A00Set").Where(GetEntityKey).ToList();
            Context.OSet.Where(o => o.Id == SelectedIndex)
                .Join(Context.E00Set, o => o.Id, e00 => e00.OId, (o, e00) => e00)
                .Join(Context.A00Set.OfType<A10>(), e00 => e00.Id, a10 => a10.E00Id, (e00, a10) => a10)
                .Join(Context.B00Set, a10 => a10.Id, b00 => b00.A10Id, (a10, b00) => b00).ToList();
            Context.OSet.Where(o => o.Id == SelectedIndex)
                .Join(Context.E00Set, o => o.Id, e00 => e00.OId, (o, e00) => e00)
                .Join(Context.A00Set.OfType<A11>(), e00 => e00.Id, a11 => a11.E00Id, (e00, a11) => a11)
                .Join(Context.C00Set, a11 => a11.Id, c00 => c00.A11Id, (a11, c00) => c00).ToList();

            return 0;
        }
    }

    public class GraphBasedQueryDepth6Assoc2 : GraphBasedQuery<InheritanceDepth6Assoc2ModelContainer, O>
    {
        public GraphBasedQueryDepth6Assoc2(IDatabaseConnection connection, Population population)
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
                    .Edge<A10, B00>(x => x.B00Set)
                    .Edge<A11, C00>(x => x.C00Set);
            }
        }
    }
}
