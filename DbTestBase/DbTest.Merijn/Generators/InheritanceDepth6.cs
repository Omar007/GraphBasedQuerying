using DbTest.Core;
using InheritanceDepth6;

namespace DbTest.Merijn.Generators
{
    public class InheritanceDepth6 : PopulationGenerator<InheritanceDepth6ModelContainer>
    {
        private T Make<T>() where T : E00, new()
        {
            return new T { O = new O() };
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 32)
            {
                using (var objContext = CreateContext(connection, population))
                {
                    objContext.E00Set.AddObject(Make<E50>());
                    objContext.E00Set.AddObject(Make<E51>());
                    objContext.E00Set.AddObject(Make<E52>());
                    objContext.E00Set.AddObject(Make<E53>());
                    objContext.E00Set.AddObject(Make<E54>());
                    objContext.E00Set.AddObject(Make<E55>());
                    objContext.E00Set.AddObject(Make<E56>());
                    objContext.E00Set.AddObject(Make<E57>());
                    objContext.E00Set.AddObject(Make<E58>());
                    objContext.E00Set.AddObject(Make<E59>());
                    objContext.E00Set.AddObject(Make<E510>());
                    objContext.E00Set.AddObject(Make<E511>());
                    objContext.E00Set.AddObject(Make<E512>());
                    objContext.E00Set.AddObject(Make<E513>());
                    objContext.E00Set.AddObject(Make<E514>());
                    objContext.E00Set.AddObject(Make<E515>());
                    objContext.E00Set.AddObject(Make<E516>());
                    objContext.E00Set.AddObject(Make<E517>());
                    objContext.E00Set.AddObject(Make<E518>());
                    objContext.E00Set.AddObject(Make<E519>());
                    objContext.E00Set.AddObject(Make<E520>());
                    objContext.E00Set.AddObject(Make<E521>());
                    objContext.E00Set.AddObject(Make<E522>());
                    objContext.E00Set.AddObject(Make<E523>());
                    objContext.E00Set.AddObject(Make<E524>());
                    objContext.E00Set.AddObject(Make<E525>());
                    objContext.E00Set.AddObject(Make<E526>());
                    objContext.E00Set.AddObject(Make<E527>());
                    objContext.E00Set.AddObject(Make<E528>());
                    objContext.E00Set.AddObject(Make<E529>());
                    objContext.E00Set.AddObject(Make<E530>());
                    objContext.E00Set.AddObject(Make<E531>());
                    objContext.SaveChanges();
                }
            }
        }
    }
}
