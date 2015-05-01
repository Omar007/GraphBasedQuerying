using System.Data.Entity;
using System.Linq;
using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh4;
using EntityGraph4EF6;

namespace DbTest.ModelDefinitions.Tests
{
    public class Inh4_EFConcretesTest : EFConcretesPopulationTest<Inh4Container>
    {
        public Inh4_EFConcretesTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            var baseQuery = Context.E00Set.Include("O").Where(x => x.O.Id == SelectedIndex);
            baseQuery.OfType<Inh4_E30>().ToList();
            baseQuery.OfType<Inh4_E31>().ToList();
            baseQuery.OfType<Inh4_E32>().ToList();
            baseQuery.OfType<Inh4_E33>().ToList();
            baseQuery.OfType<Inh4_E34>().ToList();
            baseQuery.OfType<Inh4_E35>().ToList();
            baseQuery.OfType<Inh4_E36>().ToList();
            baseQuery.OfType<Inh4_E37>().ToList();

            return 0;
        }

        protected override Inh4Container GetDbContext()
        {
            return new Inh4Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh4"; }
        }
    }

    public class Inh4_EFTest : EFPopulationTest<Inh4Container>
    {
        public Inh4_EFTest(IDatabaseConnection connection, Population population)
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

        protected override Inh4Container GetDbContext()
        {
            return new Inh4Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh4"; }
        }
    }

    public class Inh4_GBQTest : GBQPopulationTest<Inh4Container>
    {
        public Inh4_GBQTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            new SqlGraphShape<object>(Context)
                .Edge<Inh4_O>(o => o.E00s)
                .Load<Inh4_O>(x => x.Id == SelectedIndex);

            return 0;
        }

        protected override Inh4Container GetDbContext()
        {
            return new Inh4Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh4"; }
        }
    }
}
