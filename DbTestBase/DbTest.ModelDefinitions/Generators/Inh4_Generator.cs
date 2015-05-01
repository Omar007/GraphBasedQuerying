using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh4;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh4_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh4Container(connection.CreateConnectionString("Inh4", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 8)
            {
                using (var context = GetDbContext<Inh4Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh4_E30>());
                    context.E00Set.Add(Make<Inh4_E31>());
                    context.E00Set.Add(Make<Inh4_E32>());
                    context.E00Set.Add(Make<Inh4_E33>());
                    context.E00Set.Add(Make<Inh4_E34>());
                    context.E00Set.Add(Make<Inh4_E35>());
                    context.E00Set.Add(Make<Inh4_E36>());
                    context.E00Set.Add(Make<Inh4_E37>());

                    context.SaveChanges();
                }
            }
        }

        new private T Make<T>()
            where T : Inh4_E00, new()
        {
            var e = base.Make<T>();
            e.O = base.Make<Inh4_O>();
            return e;
        }
    }
}
