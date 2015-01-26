using DbTest.Core;
using DbTest.ModelDefinitions.Models.Inh6;

namespace DbTest.ModelDefinitions.Generators
{
    public class Inh6_Generator : Generator
    {
        protected override T GetDbContext<T>(IDatabaseConnection connection, Population population)
        {
            return new Inh6Container(connection.CreateConnectionString("Inh6", population)) as T;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 32)
            {
                using (var context = GetDbContext<Inh6Container>(connection, population))
                {
                    context.E00Set.Add(Make<Inh6_E50>());
                    context.E00Set.Add(Make<Inh6_E51>());
                    context.E00Set.Add(Make<Inh6_E52>());
                    context.E00Set.Add(Make<Inh6_E53>());
                    context.E00Set.Add(Make<Inh6_E54>());
                    context.E00Set.Add(Make<Inh6_E55>());
                    context.E00Set.Add(Make<Inh6_E56>());
                    context.E00Set.Add(Make<Inh6_E57>());
                    context.E00Set.Add(Make<Inh6_E58>());
                    context.E00Set.Add(Make<Inh6_E59>());
                    context.E00Set.Add(Make<Inh6_E510>());
                    context.E00Set.Add(Make<Inh6_E511>());
                    context.E00Set.Add(Make<Inh6_E512>());
                    context.E00Set.Add(Make<Inh6_E513>());
                    context.E00Set.Add(Make<Inh6_E514>());
                    context.E00Set.Add(Make<Inh6_E515>());
                    context.E00Set.Add(Make<Inh6_E516>());
                    context.E00Set.Add(Make<Inh6_E517>());
                    context.E00Set.Add(Make<Inh6_E518>());
                    context.E00Set.Add(Make<Inh6_E519>());
                    context.E00Set.Add(Make<Inh6_E520>());
                    context.E00Set.Add(Make<Inh6_E521>());
                    context.E00Set.Add(Make<Inh6_E522>());
                    context.E00Set.Add(Make<Inh6_E523>());
                    context.E00Set.Add(Make<Inh6_E524>());
                    context.E00Set.Add(Make<Inh6_E525>());
                    context.E00Set.Add(Make<Inh6_E526>());
                    context.E00Set.Add(Make<Inh6_E527>());
                    context.E00Set.Add(Make<Inh6_E528>());
                    context.E00Set.Add(Make<Inh6_E529>());
                    context.E00Set.Add(Make<Inh6_E530>());
                    context.E00Set.Add(Make<Inh6_E531>());

                    context.SaveChanges();
                }
            }
        }

        new private T Make<T>()
            where T : Inh6_E00, new()
        {
            var e = base.Make<T>();
            e.O = base.Make<Inh6_O>();
            return e;
        }
    }
}
