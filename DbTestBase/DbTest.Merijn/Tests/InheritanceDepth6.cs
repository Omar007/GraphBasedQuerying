using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbTest.Core;
using EntityGraph.SQL;
using InheritanceDepth6;

namespace DbTest.Merijn.Tests
{
    public class EFSingleQueryDepth6 : EFSingleQuery<InheritanceDepth6ModelContainer, E00>
    {
        public EFSingleQueryDepth6(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }
    }

    public class EFMultiQueryDepth6 : EFMultiQuery<InheritanceDepth6ModelContainer,E00>
    {
        public EFMultiQueryDepth6(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }

        protected override int DoTest()
        {
            Context.E00Set.OfType<E50>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E51>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E52>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E53>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E54>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E55>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E56>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E57>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E58>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E59>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E510>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E511>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E512>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E513>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E514>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E515>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E516>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E517>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E518>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E519>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E520>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E521>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E522>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E523>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E524>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E525>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E526>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E527>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E528>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E529>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E530>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E531>().Where(GetEntityKey).ToList();

            return 0;
        }
    }

    public class GraphBasedQueryDepth6 : GraphBasedQuery<InheritanceDepth6ModelContainer, O>
    {
        public GraphBasedQueryDepth6(IDatabaseConnection connection, Population population)
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
                return base.Shape.Edge<O, E00>(x => x.E00Set);
            }
        }
    }
}
