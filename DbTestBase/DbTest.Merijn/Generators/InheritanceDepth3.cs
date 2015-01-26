using DbTest.Core;
using InheritanceDepth3;

namespace DbTest.Merijn.Generators
{
    public class InheritanceDepth3 : PopulationGenerator<InheritanceDepth3ModelContainer>
    {
        private T Make<T>() where T : E00, new()
        {
            return new T { O = new O() };
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 4)
            {
                using (var objContext = CreateContext(connection, population))
                {
                    objContext.E00Set.AddObject(Make<E20>());
                    objContext.E00Set.AddObject(Make<E21>());
                    objContext.E00Set.AddObject(Make<E22>());
                    objContext.E00Set.AddObject(Make<E23>());
                    objContext.SaveChanges();
                }
            }
        }
    }
}
