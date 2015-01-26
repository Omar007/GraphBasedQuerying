using System;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class FKAssoc_Optional2Optional_Optional2End
    {
        public int Id { get; set; }
        public string dOptional2End { get; set; }

        public virtual FKAssoc_Optional2Optional_Optional1End Optional1End { get; set; }
    }

    public class FKAssoc_Optional2Optional_Optional1End
    {
        public int Id { get; set; }
        public string dOptional1End { get; set; }

        public Nullable<int> Optional2EndId { get; set; }
        public virtual FKAssoc_Optional2Optional_Optional2End Optional2End { get; set; }
    }

    public class FKAssoc_Optional2Optional_Context : DbContext
    {
        public DbSet<FKAssoc_Optional2Optional_Optional2End> Optional2End_Set { get; set; }
        public DbSet<FKAssoc_Optional2Optional_Optional1End> Optional1End_Set { get; set; }

        public FKAssoc_Optional2Optional_Context()
            : base("name=FKAssoc_Optional2Optional")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FKAssoc_Optional2Optional_Optional2End>()
                .HasKey(x => x.Id)
                .HasOptional(x => x.Optional1End)
                .WithOptionalPrincipal(x => x.Optional2End);
            modelBuilder.Entity<FKAssoc_Optional2Optional_Optional1End>()
                .HasKey(x => x.Id)
                .HasOptional(x => x.Optional2End)
                .WithOptionalDependent(x => x.Optional1End);
        }
    }

    public class FKAssoc_Optional2Optional_Test : IDisposable
    {
        private const int Entries = 50;

        public FKAssoc_Optional2Optional_Test()
        {
            using (var generationContext = new FKAssoc_Optional2Optional_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    var optional1End = new FKAssoc_Optional2Optional_Optional1End();
                    optional1End.dOptional1End = Guid.NewGuid().ToString();

                    var optional2End = new FKAssoc_Optional2Optional_Optional2End();
                    optional2End.dOptional2End = Guid.NewGuid().ToString();

                    if (i % 2 == 0)
                    {
                        optional1End.Optional2End = optional2End;
                        optional2End.Optional1End = optional1End;
                    }

                    generationContext.Optional1End_Set.Add(optional1End);
                    generationContext.Optional2End_Set.Add(optional2End);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void FKAssoc_Optional2Optional_EdgeOnOptional1End_LoadOptional1_PopulatesContext()
        {
            using (var context = new FKAssoc_Optional2Optional_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_Optional2Optional_Optional1End>(x => x.Optional2End);
                shape.Load<FKAssoc_Optional2Optional_Optional1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Optional1End_Set.Local.Count);
                Assert.Equal((context.Optional1End_Set.Local.First().Id - 1) % 2 == 0 ? 1 : 0, context.Optional2End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_Optional2Optional_EdgeOnOptional1End_LoadOptional2_PopulatesContext()
        {
            using (var context = new FKAssoc_Optional2Optional_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_Optional2Optional_Optional1End>(x => x.Optional2End);
                shape.Load<FKAssoc_Optional2Optional_Optional2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Optional2End_Set.Local.Count);
                Assert.Equal(0, context.Optional1End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_Optional2Optional_EdgeOnOptional2End_LoadOptional1_PopulatesContext()
        {
            using (var context = new FKAssoc_Optional2Optional_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_Optional2Optional_Optional2End>(x => x.Optional1End);
                shape.Load<FKAssoc_Optional2Optional_Optional1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Optional1End_Set.Local.Count);
                Assert.Equal(0, context.Optional2End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_Optional2Optional_EdgeOnOptional2End_LoadOptional2_PopulatesContext()
        {
            using (var context = new FKAssoc_Optional2Optional_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_Optional2Optional_Optional2End>(x => x.Optional1End);
                shape.Load<FKAssoc_Optional2Optional_Optional2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Optional2End_Set.Local.Count);
                Assert.Equal((context.Optional2End_Set.Local.First().Id - 1) % 2 == 0 ? 1 : 0, context.Optional1End_Set.Local.Count);
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new FKAssoc_Optional2Optional_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
