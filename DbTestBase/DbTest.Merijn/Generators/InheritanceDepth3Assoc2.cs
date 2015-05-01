using System.Data.Objects;
using DbTest.Core;
using InheritanceDepth3Assoc2;

namespace DbTest.Merijn.Generators
{
    public class InheritanceDepth3Assoc2 : PopulationGenerator<InheritanceDepth3Assoc2ModelContainer>
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
                var a11 = new A11();
                for (int ci = 0; ci < population.BCount; ci++)
                {
                    a11.C00Set.Add(new C00 { dC00 = "dC00" });
                }
                e.A00Set.Add(a10);
                e.A00Set.Add(a11);
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
