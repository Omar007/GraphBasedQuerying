using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbTest.Core;
using EntityGraph.SQL;
using InheritanceDepth4;

namespace DbTest.Merijn.Tests
{
    public class EFSingleQueryDepth4 : EFSingleQuery<InheritanceDepth4ModelContainer, E00>
    {
        public EFSingleQueryDepth4(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }
    }

    public class EFMultiQueryDepth4 : EFMultiQuery<InheritanceDepth4ModelContainer, E00>
    {
        public EFMultiQueryDepth4(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }

        protected override int DoTest()
        {
            Context.E00Set.OfType<E30>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E31>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E32>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E33>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E34>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E35>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E36>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E37>().Where(GetEntityKey).ToList();

            return 0;
        }
    }

    public class GraphBasedQueryDepth4 : GraphBasedQuery<InheritanceDepth4ModelContainer, O>
    {
        public GraphBasedQueryDepth4(IDatabaseConnection connection, Population population)
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
