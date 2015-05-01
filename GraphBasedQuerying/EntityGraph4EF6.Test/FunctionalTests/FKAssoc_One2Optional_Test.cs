using System;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class FKAssoc_One2Optional_OptionalEnd
    {
        public int Id { get; set; }
        public string dOptionalEnd { get; set; }

        public virtual FKAssoc_One2Optional_OneEnd OneEnd { get; set; }
    }

    public class FKAssoc_One2Optional_OneEnd
    {
        public int Id { get; set; }
        public string dOneEnd { get; set; }

        public virtual FKAssoc_One2Optional_OptionalEnd OptionalEnd { get; set; }
    }

    public class FKAssoc_One2Optional_Context : DbContext
    {
        public virtual DbSet<FKAssoc_One2Optional_OptionalEnd> OptionalEnd_Set { get; set; }
        public virtual DbSet<FKAssoc_One2Optional_OneEnd> OneEnd_Set { get; set; }

        public FKAssoc_One2Optional_Context()
            : base("name=FKAssoc_One2Optional")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FKAssoc_One2Optional_OptionalEnd>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.OneEnd)
                .WithOptional(x => x.OptionalEnd);
            modelBuilder.Entity<FKAssoc_One2Optional_OneEnd>()
                .HasKey(x => x.Id)
                .HasOptional(x => x.OptionalEnd)
                .WithRequired(x => x.OneEnd);
        }
    }

    public class FKAssoc_One2Optional_Test : IDisposable
    {
        private const int Entries = 50;

        public FKAssoc_One2Optional_Test()
        {
            using (var generationContext = new FKAssoc_One2Optional_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    var oneEnd = new FKAssoc_One2Optional_OneEnd();
                    oneEnd.dOneEnd = Guid.NewGuid().ToString();

                    if (i % 2 == 0)
                    {
                        var optionalEnd = new FKAssoc_One2Optional_OptionalEnd();
                        optionalEnd.dOptionalEnd = Guid.NewGuid().ToString();
                        optionalEnd.OneEnd = oneEnd;
                        generationContext.OptionalEnd_Set.Add(optionalEnd);

                        oneEnd.OptionalEnd = optionalEnd;
                    }

                    generationContext.OneEnd_Set.Add(oneEnd);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void FKAssoc_One2Optional_EdgeOnOneEnd_LoadOneEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_One2Optional_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2Optional_OneEnd>(x => x.OptionalEnd);
                shape.Load<FKAssoc_One2Optional_OneEnd>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.OneEnd_Set.Local.Count);
                Assert.Equal((context.OneEnd_Set.Local.First().Id - 1) % 2 == 0 ? 1 : 0, context.OptionalEnd_Set.Local.Count);
                Assert.Equal(context.OneEnd_Set.Local.First().OptionalEnd, context.OptionalEnd_Set.Local.FirstOrDefault());
            }
        }

        [Fact]
        public void FKAssoc_One2Optional_EdgeOnOneEnd_LoadOptionalEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_One2Optional_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2Optional_OneEnd>(x => x.OptionalEnd);
                shape.Load<FKAssoc_One2Optional_OptionalEnd>(x => x.Id == (new Random().Next(1, (Entries / 2) + 1) * 2) - 1);

                Assert.Equal(1, context.OptionalEnd_Set.Local.Count);
                Assert.Equal(0, context.OneEnd_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_One2Optional_EdgeOnOptionalEnd_LoadOneEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_One2Optional_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2Optional_OptionalEnd>(x => x.OneEnd);
                shape.Load<FKAssoc_One2Optional_OneEnd>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.OneEnd_Set.Local.Count);
                Assert.Equal(0, context.OptionalEnd_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_One2Optional_EdgeOnOptionalEnd_LoadOptionalEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_One2Optional_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2Optional_OptionalEnd>(x => x.OneEnd);
                shape.Load<FKAssoc_One2Optional_OptionalEnd>(x => x.Id == (new Random().Next(1, (Entries / 2) + 1) * 2) - 1);

                Assert.Equal(1, context.OptionalEnd_Set.Local.Count);
                Assert.Equal(1, context.OneEnd_Set.Local.Count);
                Assert.Equal(context.OptionalEnd_Set.Local.First().OneEnd, context.OneEnd_Set.Local.First());
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new FKAssoc_One2Optional_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
