using System.Data.Objects;
using DbTest.Core;
using InheritanceDepth5WideAssoc1;

namespace DbTest.Merijn.Generators
{
    public class InheritanceDepth5WideAssoc1 : PopulationGenerator<InheritanceDepth5WideAssoc1ModelContainer>
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
            for (int i = 0; i < population.ECount; i += 24)
            {
                using (var objContext = CreateContext(connection, population))
                {
                    objContext.E00Set.AddObject(Make<E40>(population));
                    objContext.E00Set.AddObject(Make<E41>(population));
                    objContext.E00Set.AddObject(Make<E42>(population));
                    objContext.E00Set.AddObject(Make<E43>(population));
                    objContext.E00Set.AddObject(Make<E44>(population));
                    objContext.E00Set.AddObject(Make<E45>(population));
                    objContext.E00Set.AddObject(Make<E46>(population));
                    objContext.E00Set.AddObject(Make<E47>(population));
                    objContext.E00Set.AddObject(Make<E48>(population));
                    objContext.E00Set.AddObject(Make<E49>(population));
                    objContext.E00Set.AddObject(Make<E410>(population));
                    objContext.E00Set.AddObject(Make<E411>(population));
                    objContext.E00Set.AddObject(Make<E412>(population));
                    objContext.E00Set.AddObject(Make<E413>(population));
                    objContext.E00Set.AddObject(Make<E414>(population));
                    objContext.E00Set.AddObject(Make<E415>(population));
                    objContext.E00Set.AddObject(Make<E416>(population));
                    objContext.E00Set.AddObject(Make<E417>(population));
                    objContext.E00Set.AddObject(Make<E418>(population));
                    objContext.E00Set.AddObject(Make<E419>(population));
                    objContext.E00Set.AddObject(Make<E420>(population));
                    objContext.E00Set.AddObject(Make<E421>(population));
                    objContext.E00Set.AddObject(Make<E422>(population));
                    objContext.E00Set.AddObject(Make<E423>(population));
                    objContext.SaveChanges();
                }
            }
        }
    }
}
