using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh5_Assoc1;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh5_Assoc1_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh5_Assoc1Container(connection.CreateConnectionString("Inh5_Assoc1", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 16)
            {
                using (var context = GetDbContext<Inh5_Assoc1Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh5_Assoc1_E40>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E41>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E42>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E43>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E44>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E45>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E46>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E47>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E48>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E49>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E410>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E411>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E412>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E413>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E414>(population));
                    context.E00Set.Add(Make<Inh5_Assoc1_E415>(population));

                    context.SaveChanges();
                }
            }
        }

        private T Make<T>(Population population)
            where T : Inh5_Assoc1_E00, new()
        {
            var e = Make<T>();
            e.O = Make<Inh5_Assoc1_O>();

            for (int ai = 0; ai < population.ACount; ai += 3)
            {
                var a10 = Make<Inh5_Assoc1_A10>();
                for (int bi = 0; bi < population.BCount; bi++)
                {
                    a10.B00s.Add(Make<Inh5_Assoc1_B00>());
                }
                e.A00s.Add(a10);
                e.A00s.Add(Make<Inh5_Assoc1_A11>());
                e.A00s.Add(Make<Inh5_Assoc1_A12>());
            }

            return e;
        }
    }
}
