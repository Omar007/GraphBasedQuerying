using System.Data.Objects;
using DbTest.Core;
using InheritanceDepth3Assoc1;

namespace DbTest.Merijn.Generators
{
    public class InheritanceDepth3Assoc1 : PopulationGenerator<InheritanceDepth3Assoc1ModelContainer>
    {
        private T Make<T>(Population population) where T : E00, new()
        {
            var e = new T { O = new O() };
            for (int ai = 0; ai < population.ACount; ai += 3)
            {
                var a10 = new A10();
                for (int bi = 0; bi < population.BCount; bi++)
                {
                    a10.B00Set.Add(new B00 { dB00 = "dB00" });
                }
                e.A00Set.Add(a10);
                e.A00Set.Add(new A11());
                e.A00Set.Add(new A12());
            }
            return e;
        }

        public override void PopulateDatabase(IDatabaseConnection connection, Population population)
        {
            for (int i = 0; i < population.ECount; i += 4)
            {
                using (var objContext = CreateContext(connection, population))
                {
                    objContext.E00Set.AddObject(Make<E20>(population));
                    objContext.E00Set.AddObject(Make<E21>(population));
                    objContext.E00Set.AddObject(Make<E22>(population));
                    objContext.E00Set.AddObject(Make<E23>(population));
                    objContext.SaveChanges();
                }
            }
        }
    }
}
