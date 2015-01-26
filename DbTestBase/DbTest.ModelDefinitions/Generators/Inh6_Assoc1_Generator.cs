using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh6_Assoc1;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh6_Assoc1_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh6_Assoc1Container(connection.CreateConnectionString("Inh6_Assoc1", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 32)
            {
                using (var context = GetDbContext<Inh6_Assoc1Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh6_Assoc1_E50>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E51>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E52>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E53>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E54>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E55>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E56>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E57>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E58>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E59>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E510>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E511>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E512>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E513>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E514>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E515>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E516>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E517>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E518>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E519>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E520>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E521>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E522>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E523>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E524>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E525>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E526>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E527>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E528>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E529>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E530>(population));
                    context.E00Set.Add(Make<Inh6_Assoc1_E531>(population));

                    context.SaveChanges();
                }
            }
        }

        private T Make<T>(Population population)
            where T : Inh6_Assoc1_E00, new()
        {
            var e = Make<T>();
            e.O = Make<Inh6_Assoc1_O>();

            for (int ai = 0; ai < population.ACount; ai += 3)
            {
                var a10 = Make<Inh6_Assoc1_A10>();
                for (int bi = 0; bi < population.BCount; bi++)
                {
                    a10.B00s.Add(Make<Inh6_Assoc1_B00>());
                }
                e.A00s.Add(a10);
                e.A00s.Add(Make<Inh6_Assoc1_A11>());
                e.A00s.Add(Make<Inh6_Assoc1_A12>());
            }

            return e;
        }
    }
}
