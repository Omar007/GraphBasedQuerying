using System.Data.Objects;
using DbTest.Core;
using InheritanceDepth6Assoc2;

namespace DbTest.Merijn.Generators
{
    public class InheritanceDepth6Assoc2 : PopulationGenerator<InheritanceDepth6Assoc2ModelContainer>
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
                for (int ci = 0; ci < population.CCount; ci++)
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
            for (int i = 0; i < population.ECount; i += 32)
            {
                using (var objContext = CreateContext(connection, population))
                {
                    objContext.E00Set.AddObject(Make<E50>(population));
                    objContext.E00Set.AddObject(Make<E51>(population));
                    objContext.E00Set.AddObject(Make<E52>(population));
                    objContext.E00Set.AddObject(Make<E53>(population));
                    objContext.E00Set.AddObject(Make<E54>(population));
                    objContext.E00Set.AddObject(Make<E55>(population));
                    objContext.E00Set.AddObject(Make<E56>(population));
                    objContext.E00Set.AddObject(Make<E57>(population));
                    objContext.E00Set.AddObject(Make<E58>(population));
                    objContext.E00Set.AddObject(Make<E59>(population));
                    objContext.E00Set.AddObject(Make<E510>(population));
                    objContext.E00Set.AddObject(Make<E511>(population));
                    objContext.E00Set.AddObject(Make<E512>(population));
                    objContext.E00Set.AddObject(Make<E513>(population));
                    objContext.E00Set.AddObject(Make<E514>(population));
                    objContext.E00Set.AddObject(Make<E515>(population));
                    objContext.E00Set.AddObject(Make<E516>(population));
                    objContext.E00Set.AddObject(Make<E517>(population));
                    objContext.E00Set.AddObject(Make<E518>(population));
                    objContext.E00Set.AddObject(Make<E519>(population));
                    objContext.E00Set.AddObject(Make<E520>(population));
                    objContext.E00Set.AddObject(Make<E521>(population));
                    objContext.E00Set.AddObject(Make<E522>(population));
                    objContext.E00Set.AddObject(Make<E523>(population));
                    objContext.E00Set.AddObject(Make<E524>(population));
                    objContext.E00Set.AddObject(Make<E525>(population));
                    objContext.E00Set.AddObject(Make<E526>(population));
                    objContext.E00Set.AddObject(Make<E527>(population));
                    objContext.E00Set.AddObject(Make<E528>(population));
                    objContext.E00Set.AddObject(Make<E529>(population));
                    objContext.E00Set.AddObject(Make<E530>(population));
                    objContext.E00Set.AddObject(Make<E531>(population));
                    objContext.SaveChanges();
                }
            }
        }
    }
}
