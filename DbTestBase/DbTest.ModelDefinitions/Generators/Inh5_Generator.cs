using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh5;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh5_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh5Container(connection.CreateConnectionString("Inh5", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 16)
            {
                using (var context = GetDbContext<Inh5Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh5_E40>());
                    context.E00Set.Add(Make<Inh5_E41>());
                    context.E00Set.Add(Make<Inh5_E42>());
                    context.E00Set.Add(Make<Inh5_E43>());
                    context.E00Set.Add(Make<Inh5_E44>());
                    context.E00Set.Add(Make<Inh5_E45>());
                    context.E00Set.Add(Make<Inh5_E46>());
                    context.E00Set.Add(Make<Inh5_E47>());
                    context.E00Set.Add(Make<Inh5_E48>());
                    context.E00Set.Add(Make<Inh5_E49>());
                    context.E00Set.Add(Make<Inh5_E410>());
                    context.E00Set.Add(Make<Inh5_E411>());
                    context.E00Set.Add(Make<Inh5_E412>());
                    context.E00Set.Add(Make<Inh5_E413>());
                    context.E00Set.Add(Make<Inh5_E414>());
                    context.E00Set.Add(Make<Inh5_E415>());

                    context.SaveChanges();
                }
            }
        }

        new private T Make<T>()
            where T : Inh5_E00, new()
        {
            var e = base.Make<T>();
            e.O = base.Make<Inh5_O>();
            return e;
        }
    }
}
