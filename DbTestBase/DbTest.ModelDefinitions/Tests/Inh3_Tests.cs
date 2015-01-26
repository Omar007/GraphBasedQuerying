using System.Data.Entity;
using System.Linq;
using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh3;
using EntityGraph4EF6;

namespace DbTest.ModelDefinitions.Tests
{
    public class Inh3_EFConcretesTest : EFConcretesPopulationTest<Inh3Container>
    {
        public Inh3_EFConcretesTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            var baseQuery = Context.E00Set.Include("O").Where(x => x.O.Id == SelectedIndex);
            baseQuery.OfType<Inh3_E20>().ToList();
            baseQuery.OfType<Inh3_E21>().ToList();
            baseQuery.OfType<Inh3_E22>().ToList();
            baseQuery.OfType<Inh3_E23>().ToList();

            return 0;
        }

        protected override Inh3Container GetDbContext()
        {
            return new Inh3Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh3"; }
        }
    }

    public class Inh3_EFTest : EFPopulationTest<Inh3Container>
    {
        public Inh3_EFTest(IDatabaseConnection connection, Population population)
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

        protected override Inh3Container GetDbContext()
        {
            return new Inh3Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh3"; }
        }
    }

    public class Inh3_GBQTest : GBQPopulationTest<Inh3Container>
    {
        public Inh3_GBQTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            new SqlGraphShape<object>(Context)
                .Edge<Inh3_O>(o => o.E00s)
                .Load<Inh3_O>(x => x.Id == SelectedIndex);

            return 0;
        }

        protected override Inh3Container GetDbContext()
        {
            return new Inh3Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh3"; }
        }
    }
}
