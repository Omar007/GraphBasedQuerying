using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh6_Assoc3;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh6_Assoc3_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh6_Assoc3Container(connection.CreateConnectionString("Inh6_Assoc3", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 32)
            {
                using (var context = GetDbContext<Inh6_Assoc3Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh6_Assoc3_E50>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E51>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E52>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E53>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E54>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E55>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E56>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E57>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E58>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E59>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E510>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E511>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E512>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E513>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E514>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E515>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E516>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E517>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E518>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E519>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E520>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E521>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E522>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E523>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E524>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E525>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E526>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E527>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E528>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E529>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E530>(population));
                    context.E00Set.Add(Make<Inh6_Assoc3_E531>(population));

                    context.SaveChanges();
                }
            }
        }

        private T Make<T>(Population population)
            where T : Inh6_Assoc3_E00, new()
        {
            var e = Make<T>();
            e.O = Make<Inh6_Assoc3_O>();

            for (int ai = 0; ai < population.ACount; ai += 3)
            {
                var a10 = Make<Inh6_Assoc3_A10>();
                for (int bi = 0; bi < population.BCount; bi++)
                {
                    a10.B00s.Add(Make<Inh6_Assoc3_B00>());
                }
                var a11 = Make<Inh6_Assoc3_A11>();
                for (int ci = 0; ci < population.CCount; ci++)
                {
                    a11.C00s.Add(Make<Inh6_Assoc3_C00>());
                }
                var a12 = Make<Inh6_Assoc3_A12>();
                for (int di = 0; di < population.DCount; di++)
                {
                    a12.D00s.Add(Make<Inh6_Assoc3_D00>());
                }
                e.A00s.Add(a10);
                e.A00s.Add(a11);
                e.A00s.Add(a12);
            }

            return e;
        }
    }
}
