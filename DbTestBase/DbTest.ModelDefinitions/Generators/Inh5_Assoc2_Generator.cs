using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh5_Assoc2;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh5_Assoc2_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh5_Assoc2Container(connection.CreateConnectionString("Inh5_Assoc2", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 16)
            {
                using (var context = GetDbContext<Inh5_Assoc2Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh5_Assoc2_E40>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E41>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E42>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E43>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E44>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E45>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E46>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E47>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E48>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E49>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E410>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E411>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E412>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E413>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E414>(population));
                    context.E00Set.Add(Make<Inh5_Assoc2_E415>(population));

                    context.SaveChanges();
                }
            }
        }

        private T Make<T>(Population population)
            where T : Inh5_Assoc2_E00, new()
        {
            var e = Make<T>();
            e.O = Make<Inh5_Assoc2_O>();

            for (int ai = 0; ai < population.ACount; ai += 3)
            {
                var a10 = Make<Inh5_Assoc2_A10>();
                for (int bi = 0; bi < population.BCount; bi++)
                {
                    a10.B00s.Add(Make<Inh5_Assoc2_B00>());
                }
                var a11 = Make<Inh5_Assoc2_A11>();
                for (int ci = 0; ci < population.BCount; ci++)
                {
                    a11.C00s.Add(Make<Inh5_Assoc2_C00>());
                }
                e.A00s.Add(a10);
                e.A00s.Add(a11);
                e.A00s.Add(Make<Inh5_Assoc2_A12>());
            }

            return e;
        }
    }
}
