using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbTest.Core;
using EntityGraph.SQL;
using InheritanceDepth3;

namespace DbTest.Merijn.Tests
{
    public class EFSingleQueryDepth3 : EFSingleQuery<InheritanceDepth3ModelContainer, E00>
    {
        public EFSingleQueryDepth3(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }
    }

    public class EFMultiQueryDepth3 : EFMultiQuery<InheritanceDepth3ModelContainer, E00>
    {
        public EFMultiQueryDepth3(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }

        protected override int DoTest()
        {
            Context.E00Set.OfType<E20>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E21>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E22>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E23>().Where(GetEntityKey).ToList();

            return 0;
        }
    }

    public class GraphBasedQueryDepth3 : GraphBasedQuery<InheritanceDepth3ModelContainer, O>
    {
        public GraphBasedQueryDepth3(IDatabaseConnection connection, Population population)
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
