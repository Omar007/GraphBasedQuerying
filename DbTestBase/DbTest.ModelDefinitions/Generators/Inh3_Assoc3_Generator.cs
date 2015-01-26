﻿using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh3_Assoc3;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh3_Assoc3_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh3_Assoc3Container(connection.CreateConnectionString("Inh3_Assoc3", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 4)
            {
                using (var context = GetDbContext<Inh3_Assoc3Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh3_Assoc3_E20>(population));
                    context.E00Set.Add(Make<Inh3_Assoc3_E21>(population));
                    context.E00Set.Add(Make<Inh3_Assoc3_E22>(population));
                    context.E00Set.Add(Make<Inh3_Assoc3_E23>(population));

                    context.SaveChanges();
                }
            }
        }

        private T Make<T>(Population population)
            where T : Inh3_Assoc3_E00, new()
        {
            var e = Make<T>();
            e.O = Make<Inh3_Assoc3_O>();

            for (int ai = 0; ai < population.ACount; ai += 3)
            {
                var a10 = Make<Inh3_Assoc3_A10>();
                for (int bi = 0; bi < population.BCount; bi++)
                {
                    a10.B00s.Add(Make<Inh3_Assoc3_B00>());
                }
                var a11 = Make<Inh3_Assoc3_A11>();
                for (int ci = 0; ci < population.CCount; ci++)
                {
                    a11.C00s.Add(Make<Inh3_Assoc3_C00>());
                }
                var a12 = Make<Inh3_Assoc3_A12>();
                for (int di = 0; di < population.DCount; di++)
                {
                    a12.D00s.Add(Make<Inh3_Assoc3_D00>());
                }
                e.A00s.Add(a10);
                e.A00s.Add(a11);
                e.A00s.Add(a12);
            }

            return e;
        }
    }
}
