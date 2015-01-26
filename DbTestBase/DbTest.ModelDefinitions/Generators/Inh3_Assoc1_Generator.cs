using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh3_Assoc1;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh3_Assoc1_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh3_Assoc1Container(connection.CreateConnectionString("Inh3_Assoc1", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 4)
            {
                using (var context = GetDbContext<Inh3_Assoc1Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh3_Assoc1_E20>(population));
                    context.E00Set.Add(Make<Inh3_Assoc1_E21>(population));
                    context.E00Set.Add(Make<Inh3_Assoc1_E22>(population));
                    context.E00Set.Add(Make<Inh3_Assoc1_E23>(population));

                    context.SaveChanges();
                }
            }
        }

        private T Make<T>(Population population)
            where T : Inh3_Assoc1_E00, new()
        {
            var e = Make<T>();
            e.O = Make<Inh3_Assoc1_O>();

            for (int ai = 0; ai < population.ACount; ai += 3)
            {
                var a10 = Make<Inh3_Assoc1_A10>();
                for (int bi = 0; bi < population.BCount; bi++)
                {
                    a10.B00s.Add(Make<Inh3_Assoc1_B00>());
                }
                e.A00s.Add(a10);
                e.A00s.Add(Make<Inh3_Assoc1_A11>());
                e.A00s.Add(Make<Inh3_Assoc1_A12>());
            }

            return e;
        }
    }
}
