using System.Data.Entity;
using System.Linq;
using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh5Wide;
using EntityGraph4EF6;

namespace DbTest.ModelDefinitions.Tests
{
    public class Inh5Wide_EFConcretesTest : EFConcretesPopulationTest<Inh5WideContainer>
    {
        public Inh5Wide_EFConcretesTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            var baseQuery = Context.E00Set.Include("O").Where(x => x.O.Id == SelectedIndex);
            baseQuery.OfType<Inh5Wide_E40>().ToList();
            baseQuery.OfType<Inh5Wide_E41>().ToList();
            baseQuery.OfType<Inh5Wide_E42>().ToList();
            baseQuery.OfType<Inh5Wide_E43>().ToList();
            baseQuery.OfType<Inh5Wide_E44>().ToList();
            baseQuery.OfType<Inh5Wide_E45>().ToList();
            baseQuery.OfType<Inh5Wide_E46>().ToList();
            baseQuery.OfType<Inh5Wide_E47>().ToList();
            baseQuery.OfType<Inh5Wide_E48>().ToList();
            baseQuery.OfType<Inh5Wide_E49>().ToList();
            baseQuery.OfType<Inh5Wide_E410>().ToList();
            baseQuery.OfType<Inh5Wide_E411>().ToList();
            baseQuery.OfType<Inh5Wide_E412>().ToList();
            baseQuery.OfType<Inh5Wide_E413>().ToList();
            baseQuery.OfType<Inh5Wide_E414>().ToList();
            baseQuery.OfType<Inh5Wide_E415>().ToList();
            baseQuery.OfType<Inh5Wide_E416>().ToList();
            baseQuery.OfType<Inh5Wide_E417>().ToList();
            baseQuery.OfType<Inh5Wide_E418>().ToList();
            baseQuery.OfType<Inh5Wide_E419>().ToList();
            baseQuery.OfType<Inh5Wide_E420>().ToList();
            baseQuery.OfType<Inh5Wide_E421>().ToList();
            baseQuery.OfType<Inh5Wide_E422>().ToList();
            baseQuery.OfType<Inh5Wide_E423>().ToList();

            return 0;
        }

        protected override Inh5WideContainer GetDbContext()
        {
            return new Inh5WideContainer(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh5Wide"; }
        }
    }

    public class Inh5Wide_EFTest : EFPopulationTest<Inh5WideContainer>
    {
        public Inh5Wide_EFTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            Context.OSet
                .Where(x => x.Id == SelectedIndex)
                .Include(o => o.E00s)
                .ToList();

            return 0;
        }

        protected override Inh5WideContainer GetDbContext()
        {
            return new Inh5WideContainer(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh5Wide"; }
        }
    }

    public class Inh5Wide_GBQTest : GBQPopulationTest<Inh5WideContainer>
    {
        public Inh5Wide_GBQTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            new SqlGraphShape<object>(Context)
                .Edge<Inh5Wide_O>(o => o.E00s)
                .Load<Inh5Wide_O>(x => x.Id == SelectedIndex);

            return 0;
        }

        protected override Inh5WideContainer GetDbContext()
        {
            return new Inh5WideContainer(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh5Wide"; }
        }
    }
}
