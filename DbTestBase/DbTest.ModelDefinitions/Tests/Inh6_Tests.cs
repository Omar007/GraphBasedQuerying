using System.Data.Entity;
using System.Linq;
using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh6;
using EntityGraph4EF6;

namespace DbTest.ModelDefinitions.Tests
{
    public class Inh6_EFConcretesTest : EFConcretesPopulationTest<Inh6Container>
    {
        public Inh6_EFConcretesTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            var baseQuery = Context.E00Set.Include("O").Where(x => x.O.Id == SelectedIndex);
            baseQuery.OfType<Inh6_E50>().ToList();
            baseQuery.OfType<Inh6_E51>().ToList();
            baseQuery.OfType<Inh6_E52>().ToList();
            baseQuery.OfType<Inh6_E53>().ToList();
            baseQuery.OfType<Inh6_E54>().ToList();
            baseQuery.OfType<Inh6_E55>().ToList();
            baseQuery.OfType<Inh6_E56>().ToList();
            baseQuery.OfType<Inh6_E57>().ToList();
            baseQuery.OfType<Inh6_E58>().ToList();
            baseQuery.OfType<Inh6_E59>().ToList();
            baseQuery.OfType<Inh6_E510>().ToList();
            baseQuery.OfType<Inh6_E511>().ToList();
            baseQuery.OfType<Inh6_E512>().ToList();
            baseQuery.OfType<Inh6_E513>().ToList();
            baseQuery.OfType<Inh6_E514>().ToList();
            baseQuery.OfType<Inh6_E515>().ToList();
            baseQuery.OfType<Inh6_E516>().ToList();
            baseQuery.OfType<Inh6_E517>().ToList();
            baseQuery.OfType<Inh6_E518>().ToList();
            baseQuery.OfType<Inh6_E519>().ToList();
            baseQuery.OfType<Inh6_E520>().ToList();
            baseQuery.OfType<Inh6_E521>().ToList();
            baseQuery.OfType<Inh6_E522>().ToList();
            baseQuery.OfType<Inh6_E523>().ToList();
            baseQuery.OfType<Inh6_E524>().ToList();
            baseQuery.OfType<Inh6_E525>().ToList();
            baseQuery.OfType<Inh6_E526>().ToList();
            baseQuery.OfType<Inh6_E527>().ToList();
            baseQuery.OfType<Inh6_E528>().ToList();
            baseQuery.OfType<Inh6_E529>().ToList();
            baseQuery.OfType<Inh6_E530>().ToList();
            baseQuery.OfType<Inh6_E531>().ToList();

            return 0;
        }

        protected override Inh6Container GetDbContext()
        {
            return new Inh6Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh6"; }
        }
    }

    public class Inh6_EFTest : EFPopulationTest<Inh6Container>
    {
        public Inh6_EFTest(IDatabaseConnection connection, Population population)
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

        protected override Inh6Container GetDbContext()
        {
            return new Inh6Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh6"; }
        }
    }

    public class Inh6_GBQTest : GBQPopulationTest<Inh6Container>
    {
        public Inh6_GBQTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            new SqlGraphShape<object>(Context)
                .Edge<Inh6_O>(o => o.E00s)
                .Load<Inh6_O>(x => x.Id == SelectedIndex);

            return 0;
        }

        protected override Inh6Container GetDbContext()
        {
            return new Inh6Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh6"; }
        }
    }
}
