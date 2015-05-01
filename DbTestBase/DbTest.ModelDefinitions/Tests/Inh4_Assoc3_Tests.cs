using System.Data.Entity;
using System.Linq;
using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh4_Assoc3;
using EntityGraph4EF6;

namespace DbTest.ModelDefinitions.Tests
{
    public class Inh4_Assoc3_EFTest : EFPopulationTest<Inh4_Assoc3Container>
    {
        public Inh4_Assoc3_EFTest(IDatabaseConnection connection, Population population)
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
                .SelectMany(e => e.A00s).OfType<Inh4_Assoc3_A10>()
                .SelectMany(a => a.B00s)
                .ToList();
            Context.OSet
                .Where(o => o.Id == SelectedIndex)
                .SelectMany(o => o.E00s)
                .SelectMany(e => e.A00s).OfType<Inh4_Assoc3_A11>()
                .SelectMany(a => a.C00s)
                .ToList();
            Context.OSet
                .Where(o => o.Id == SelectedIndex)
                .SelectMany(o => o.E00s)
                .SelectMany(e => e.A00s).OfType<Inh4_Assoc3_A12>()
                .SelectMany(a => a.D00s)
                .ToList();

            return 0;
        }

        protected override Inh4_Assoc3Container GetDbContext()
        {
            return new Inh4_Assoc3Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh4_Assoc3"; }
        }
    }

    public class Inh4_Assoc3_GBQTest : GBQPopulationTest<Inh4_Assoc3Container>
    {
        public Inh4_Assoc3_GBQTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            new SqlGraphShape<object>(Context)
                .Edge<Inh4_Assoc3_O>(o => o.E00s)
                .Edge<Inh4_Assoc3_E00>(e => e.A00s)
                .Edge<Inh4_Assoc3_A10>(a => a.B00s)
                .Edge<Inh4_Assoc3_A11>(a => a.C00s)
                .Edge<Inh4_Assoc3_A12>(a => a.D00s)
                .Load<Inh4_Assoc3_O>(x => x.Id == SelectedIndex);

            return 0;
        }

        protected override Inh4_Assoc3Container GetDbContext()
        {
            return new Inh4_Assoc3Container(Connection.CreateConnectionString(ModelName, Population));
        }

        public override string ModelName
        {
            get { return "Inh4_Assoc3"; }
        }
    }
}
