using System.Data.Entity;
using System.Linq;
using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh3_Assoc2;
using EntityGraph4EF6;

namespace DbTest.ModelDefinitions.Tests
{
    public class Inh3_Assoc2_EFTest : EFPopulationTest<Inh3_Assoc2Container>
    {
        public Inh3_Assoc2_EFTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            Context.OSet
                .Where(x => x.Id == SelectedIndex)
                .Include(o => o.E00s.Select(e => e.A00s))
                .ToList();

            Context.OSet
                .Where(o => o.Id == SelectedIndex)
                .SelectMany(o => o.E00s)
                .SelectMany(e => e.A00s).OfType<Inh3_Assoc2_A10>()
                .SelectMany(a => a.B00s)
                .ToList();
            Context.OSet
                .Where(o => o.Id == SelectedIndex)
                .SelectMany(o => o.E00s)
                .SelectMany(e => e.A00s).OfType<Inh3_Assoc2_A11>()
                .SelectMany(a => a.C00s)
                .ToList();

            return 0;
        }

        protected override Inh3_Assoc2Container GetDbContext()
        {
            return new Inh3_Assoc2Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh3_Assoc2"; }
        }
    }

    public class Inh3_Assoc2_GBQTest : GBQPopulationTest<Inh3_Assoc2Container>
    {
        public Inh3_Assoc2_GBQTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            new SqlGraphShape<object>(Context)
                .Edge<Inh3_Assoc2_O>(o => o.E00s)
                .Edge<Inh3_Assoc2_E00>(e => e.A00s)
                .Edge<Inh3_Assoc2_A10>(a => a.B00s)
                .Edge<Inh3_Assoc2_A11>(a => a.C00s)
                .Load<Inh3_Assoc2_O>(x => x.Id == SelectedIndex);

            return 0;
        }

        protected override Inh3_Assoc2Container GetDbContext()
        {
            return new Inh3_Assoc2Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh3_Assoc2"; }
        }
    }
}
