using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbTest.Core;
using EntityGraph.SQL;
using InheritanceDepth5;

namespace DbTest.Merijn.Tests
{
    public class EFSingleQueryDepth5 : EFSingleQuery<InheritanceDepth5ModelContainer, E00>
    {
        public EFSingleQueryDepth5(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }
    }

    public class EFMultiQueryDepth5 : EFMultiQuery<InheritanceDepth5ModelContainer,E00>
    {
        public EFMultiQueryDepth5(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }

        protected override int DoTest()
        {
            Context.E00Set.OfType<E40>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E41>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E42>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E43>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E44>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E45>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E46>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E47>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E48>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E49>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E410>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E411>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E412>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E413>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E414>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E415>().Where(GetEntityKey).ToList();

            return 0;
        }
    }

    public class GraphBasedQueryDepth5 : GraphBasedQuery<InheritanceDepth5ModelContainer, O>
    {
        
        public GraphBasedQueryDepth5(IDatabaseConnection connection, Population population)
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
