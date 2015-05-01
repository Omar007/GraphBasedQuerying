using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh4_Assoc1;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh4_Assoc1_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh4_Assoc1Container(connection.CreateConnectionString("Inh4_Assoc1", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 8)
            {
                using (var context = GetDbContext<Inh4_Assoc1Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh4_Assoc1_E30>(population));
                    context.E00Set.Add(Make<Inh4_Assoc1_E31>(population));
                    context.E00Set.Add(Make<Inh4_Assoc1_E32>(population));
                    context.E00Set.Add(Make<Inh4_Assoc1_E33>(population));
                    context.E00Set.Add(Make<Inh4_Assoc1_E34>(population));
                    context.E00Set.Add(Make<Inh4_Assoc1_E35>(population));
                    context.E00Set.Add(Make<Inh4_Assoc1_E36>(population));
                    context.E00Set.Add(Make<Inh4_Assoc1_E37>(population));

                    context.SaveChanges();
                }
            }
        }

        private T Make<T>(Population population)
            where T : Inh4_Assoc1_E00, new()
        {
            var e = Make<T>();
            e.O = Make<Inh4_Assoc1_O>();

            for (int ai = 0; ai < population.ACount; ai += 3)
            {
                var a10 = Make<Inh4_Assoc1_A10>();
                for (int bi = 0; bi < population.BCount; bi++)
                {
                    a10.B00s.Add(Make<Inh4_Assoc1_B00>());
                }
                e.A00s.Add(a10);
                e.A00s.Add(Make<Inh4_Assoc1_A11>());
                e.A00s.Add(Make<Inh4_Assoc1_A12>());
            }

            return e;
        }
    }
}
