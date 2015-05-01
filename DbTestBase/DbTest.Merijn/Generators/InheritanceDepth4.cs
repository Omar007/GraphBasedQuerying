using DbTest.Core;
using InheritanceDepth4;

namespace DbTest.Merijn.Generators
{
    public class InheritanceDepth4 : PopulationGenerator<InheritanceDepth4ModelContainer>
    {
        private T Make<T>() where T : E00, new()
        {
            return new T { O = new O() };
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 8)
            {
                using (var objContext = CreateContext(connection, population))
                {
                    objContext.E00Set.AddObject(Make<E30>());
                    objContext.E00Set.AddObject(Make<E31>());
                    objContext.E00Set.AddObject(Make<E32>());
                    objContext.E00Set.AddObject(Make<E33>());
                    objContext.E00Set.AddObject(Make<E34>());
                    objContext.E00Set.AddObject(Make<E35>());
                    objContext.E00Set.AddObject(Make<E36>());
                    objContext.E00Set.AddObject(Make<E37>());
                    objContext.SaveChanges();
                }
            }
        }
    }
}
