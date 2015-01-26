using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh3;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh3_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh3Container(connection.CreateConnectionString("Inh3", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 4)
            {
                using (var context = GetDbContext<Inh3Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh3_E20>());
                    context.E00Set.Add(Make<Inh3_E21>());
                    context.E00Set.Add(Make<Inh3_E22>());
                    context.E00Set.Add(Make<Inh3_E23>());

                    context.SaveChanges();
                }
            }
        }

        new private T Make<T>()
            where T : Inh3_E00, new()
        {
            var e = base.Make<T>();
            e.O = base.Make<Inh3_O>();
            return e;
        }
    }
}
