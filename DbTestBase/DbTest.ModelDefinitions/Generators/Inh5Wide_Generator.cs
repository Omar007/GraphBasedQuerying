using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh5Wide;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh5Wide_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh5WideContainer(connection.CreateConnectionString("Inh5Wide", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 24)
            {
                using (var context = GetDbContext<Inh5WideContainer>(connection, population))
                {
                    context.E00Set.Add(Make<Inh5Wide_E40>());
                    context.E00Set.Add(Make<Inh5Wide_E41>());
                    context.E00Set.Add(Make<Inh5Wide_E42>());
                    context.E00Set.Add(Make<Inh5Wide_E43>());
                    context.E00Set.Add(Make<Inh5Wide_E44>());
                    context.E00Set.Add(Make<Inh5Wide_E45>());
                    context.E00Set.Add(Make<Inh5Wide_E46>());
                    context.E00Set.Add(Make<Inh5Wide_E47>());
                    context.E00Set.Add(Make<Inh5Wide_E48>());
                    context.E00Set.Add(Make<Inh5Wide_E49>());
                    context.E00Set.Add(Make<Inh5Wide_E410>());
                    context.E00Set.Add(Make<Inh5Wide_E411>());
                    context.E00Set.Add(Make<Inh5Wide_E412>());
                    context.E00Set.Add(Make<Inh5Wide_E413>());
                    context.E00Set.Add(Make<Inh5Wide_E414>());
                    context.E00Set.Add(Make<Inh5Wide_E415>());
                    context.E00Set.Add(Make<Inh5Wide_E416>());
                    context.E00Set.Add(Make<Inh5Wide_E417>());
                    context.E00Set.Add(Make<Inh5Wide_E418>());
                    context.E00Set.Add(Make<Inh5Wide_E419>());
                    context.E00Set.Add(Make<Inh5Wide_E420>());
                    context.E00Set.Add(Make<Inh5Wide_E421>());
                    context.E00Set.Add(Make<Inh5Wide_E422>());
                    context.E00Set.Add(Make<Inh5Wide_E423>());

                    context.SaveChanges();
                }
            }
        }

        new private T Make<T>()
            where T : Inh5Wide_E00, new()
        {
            var e = base.Make<T>();
            e.O = base.Make<Inh5Wide_O>();
            return e;
        }
    }
}
