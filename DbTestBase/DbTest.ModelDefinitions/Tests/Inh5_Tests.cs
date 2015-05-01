using System.Data.Entity;
using System.Linq;
using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh5;
using EntityGraph4EF6;

namespace DbTest.ModelDefinitions.Tests
{
    public class Inh5_EFConcretesTest : EFConcretesPopulationTest<Inh5Container>
    {
        public Inh5_EFConcretesTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            var baseQuery = Context.E00Set.Include("O").Where(x => x.O.Id == SelectedIndex);
            baseQuery.OfType<Inh5_E40>().ToList();
            baseQuery.OfType<Inh5_E41>().ToList();
            baseQuery.OfType<Inh5_E42>().ToList();
            baseQuery.OfType<Inh5_E43>().ToList();
            baseQuery.OfType<Inh5_E44>().ToList();
            baseQuery.OfType<Inh5_E45>().ToList();
            baseQuery.OfType<Inh5_E46>().ToList();
            baseQuery.OfType<Inh5_E47>().ToList();
            baseQuery.OfType<Inh5_E48>().ToList();
            baseQuery.OfType<Inh5_E49>().ToList();
            baseQuery.OfType<Inh5_E410>().ToList();
            baseQuery.OfType<Inh5_E411>().ToList();
            baseQuery.OfType<Inh5_E412>().ToList();
            baseQuery.OfType<Inh5_E413>().ToList();
            baseQuery.OfType<Inh5_E414>().ToList();
            baseQuery.OfType<Inh5_E415>().ToList();

            return 0;
        }

        protected override Inh5Container GetDbContext()
        {
            return new Inh5Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh5"; }
        }
    }

    public class Inh5_EFTest : EFPopulationTest<Inh5Container>
    {
        public Inh5_EFTest(IDatabaseConnection connection, Population population)
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

        protected override Inh5Container GetDbContext()
        {
            return new Inh5Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh5"; }
        }
    }

    public class Inh5_GBQTest : GBQPopulationTest<Inh5Container>
    {
        public Inh5_GBQTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            new SqlGraphShape<object>(Context)
                .Edge<Inh5_O>(o => o.E00s)
                .Load<Inh5_O>(x => x.Id == SelectedIndex);

            return 0;
        }

        protected override Inh5Container GetDbContext()
        {
            return new Inh5Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh5"; }
        }
    }
}
