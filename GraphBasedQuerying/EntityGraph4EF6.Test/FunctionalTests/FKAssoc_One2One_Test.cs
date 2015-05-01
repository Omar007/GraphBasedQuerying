using System;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class FKAssoc_One2One_One2End
    {
        public int Id { get; set; }
        public string dOne2End { get; set; }

        public virtual FKAssoc_One2One_One1End One1End { get; set; }
    }

    public class FKAssoc_One2One_One1End
    {
        public int Id { get; set; }
        public string dOne1End { get; set; }

        public virtual FKAssoc_One2One_One2End One2End { get; set; }
    }

    public class FKAssoc_One2One_Context : DbContext
    {
        public virtual DbSet<FKAssoc_One2One_One2End> One2End_Set { get; set; }
        public virtual DbSet<FKAssoc_One2One_One1End> One1End_Set { get; set; }

        public FKAssoc_One2One_Context()
            : base("name=FKAssoc_One2One")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FKAssoc_One2One_One2End>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.One1End)
                .WithRequiredPrincipal(x => x.One2End);
            modelBuilder.Entity<FKAssoc_One2One_One1End>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.One2End)
                .WithRequiredDependent(x => x.One1End);
        }
    }

    public class FKAssoc_One2One_Test : IDisposable
    {
        private const int Entries = 50;

        public FKAssoc_One2One_Test()
        {
            using (var generationContext = new FKAssoc_One2One_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    var one1End = new FKAssoc_One2One_One1End();
                    one1End.dOne1End = Guid.NewGuid().ToString();

                    var one2End = new FKAssoc_One2One_One2End();
                    one2End.dOne2End = Guid.NewGuid().ToString();

                    one1End.One2End = one2End;
                    one2End.One1End = one1End;

                    generationContext.One1End_Set.Add(one1End);
                    generationContext.One2End_Set.Add(one2End);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void FKAssoc_One2One_EdgeOnOne1End_LoadOne1End_PopulatesContext()
        {
            using (var context = new FKAssoc_One2One_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2One_One1End>(x => x.One2End);
                shape.Load<FKAssoc_One2One_One1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.One1End_Set.Local.Count);
                Assert.Equal(1, context.One2End_Set.Local.Count);
                Assert.Equal(context.One1End_Set.Local.First().One2End, context.One2End_Set.Local.First());
            }
        }

        [Fact]
        public void FKAssoc_One2One_EdgeOnOne1End_LoadOne2End_PopulatesContext()
        {
            using (var context = new FKAssoc_One2One_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2One_One1End>(x => x.One2End);
                shape.Load<FKAssoc_One2One_One2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.One2End_Set.Local.Count);
                Assert.Equal(0, context.One1End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_One2One_EdgeOnOne2End_LoadEnd1End_PopulatesContext()
        {
            using (var context = new FKAssoc_One2One_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2One_One2End>(x => x.One1End);
                shape.Load<FKAssoc_One2One_One1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.One1End_Set.Local.Count);
                Assert.Equal(0, context.One2End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_One2One_EdgeOnOne2End_LoadEnd2End_PopulatesContext()
        {
            using (var context = new FKAssoc_One2One_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2One_One2End>(x => x.One1End);
                shape.Load<FKAssoc_One2One_One2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.One2End_Set.Local.Count);
                Assert.Equal(1, context.One1End_Set.Local.Count);
                Assert.Equal(context.One2End_Set.Local.First().One1End, context.One1End_Set.Local.First());
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new FKAssoc_One2One_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
