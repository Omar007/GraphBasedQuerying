using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbTest.Core;
using EntityGraph.SQL;
using InheritanceDepth5Wide;

namespace DbTest.Merijn.Tests
{
    public class EFSingleQueryDepth5Wide : EFSingleQuery<InheritanceDepth5WideModelContainer, E00>
    {
        public EFSingleQueryDepth5Wide(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Expression<Func<E00, bool>> GetEntityKey
        {
            get { return x => x.OId == SelectedIndex; }
        }
    }

    public class EFMultiQueryDepth5Wide : EFMultiQuery<InheritanceDepth5WideModelContainer, E00>
    {
        public EFMultiQueryDepth5Wide(IDatabaseConnection connection, Population population)
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
            Context.E00Set.OfType<E416>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E417>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E418>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E419>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E420>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E421>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E422>().Where(GetEntityKey).ToList();
            Context.E00Set.OfType<E423>().Where(GetEntityKey).ToList();

            return 0;
        }
    }

    public class GraphBasedQueryDepth5Wide : GraphBasedQuery<InheritanceDepth5WideModelContainer, O>
    {
        public GraphBasedQueryDepth5Wide(IDatabaseConnection connection, Population population)
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
