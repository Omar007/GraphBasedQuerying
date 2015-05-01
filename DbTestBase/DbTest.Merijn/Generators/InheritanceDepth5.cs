using DbTest.Core;
using InheritanceDepth5;

namespace DbTest.Merijn.Generators
{
    public class InheritanceDepth5 : PopulationGenerator<InheritanceDepth5ModelContainer>
    {
        private T Make<T>() where T : E00, new()
        {
            return new T { O = new O() };
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 16)
            {
                using (var objContext = CreateContext(connection, population))
                {
                    objContext.E00Set.AddObject(Make<E40>());
                    objContext.E00Set.AddObject(Make<E41>());
                    objContext.E00Set.AddObject(Make<E42>());
                    objContext.E00Set.AddObject(Make<E43>());
                    objContext.E00Set.AddObject(Make<E44>());
                    objContext.E00Set.AddObject(Make<E45>());
                    objContext.E00Set.AddObject(Make<E46>());
                    objContext.E00Set.AddObject(Make<E47>());
                    objContext.E00Set.AddObject(Make<E48>());
                    objContext.E00Set.AddObject(Make<E49>());
                    objContext.E00Set.AddObject(Make<E410>());
                    objContext.E00Set.AddObject(Make<E411>());
                    objContext.E00Set.AddObject(Make<E412>());
                    objContext.E00Set.AddObject(Make<E413>());
                    objContext.E00Set.AddObject(Make<E414>());
                    objContext.E00Set.AddObject(Make<E415>());
                    objContext.SaveChanges();
                }
            }
        }
    }
}
