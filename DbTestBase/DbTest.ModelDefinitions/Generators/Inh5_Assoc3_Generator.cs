using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh5_Assoc3;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh5_Assoc3_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh5_Assoc3Container(connection.CreateConnectionString("Inh5_Assoc3", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 16)
            {
                using (var context = GetDbContext<Inh5_Assoc3Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh5_Assoc3_E40>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E41>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E42>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E43>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E44>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E45>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E46>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E47>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E48>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E49>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E410>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E411>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E412>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E413>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E414>(population));
                    context.E00Set.Add(Make<Inh5_Assoc3_E415>(population));

                    context.SaveChanges();
                }
            }
        }

        private T Make<T>(Population population)
            where T : Inh5_Assoc3_E00, new()
        {
            var e = Make<T>();
            e.O = Make<Inh5_Assoc3_O>();

            for (int ai = 0; ai < population.ACount; ai += 3)
            {
                var a10 = Make<Inh5_Assoc3_A10>();
                for (int bi = 0; bi < population.BCount; bi++)
                {
                    a10.B00s.Add(Make<Inh5_Assoc3_B00>());
                }
                var a11 = Make<Inh5_Assoc3_A11>();
                for (int ci = 0; ci < population.CCount; ci++)
                {
                    a11.C00s.Add(Make<Inh5_Assoc3_C00>());
                }
                var a12 = Make<Inh5_Assoc3_A12>();
                for (int di = 0; di < population.DCount; di++)
                {
                    a12.D00s.Add(Make<Inh5_Assoc3_D00>());
                }
                e.A00s.Add(a10);
                e.A00s.Add(a11);
                e.A00s.Add(a12);
            }

            return e;
        }
    }
}
