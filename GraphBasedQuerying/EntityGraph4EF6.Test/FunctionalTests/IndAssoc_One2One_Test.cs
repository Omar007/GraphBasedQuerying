using System;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class IndAssoc_One2One_One2End
    {
        public int Id { get; set; }
        public string dOne2End { get; set; }

        public virtual IndAssoc_One2One_One1End One1End { get; set; }
    }

    public class IndAssoc_One2One_One1End
    {
        public int Id { get; set; }
        public string dOne1End { get; set; }

        public virtual IndAssoc_One2One_One2End One2End { get; set; }
    }

    public class IndAssoc_One2One_Context : DbContext
    {
        public virtual DbSet<IndAssoc_One2One_One2End> One2End_Set { get; set; }
        public virtual DbSet<IndAssoc_One2One_One1End> One1End_Set { get; set; }

        public IndAssoc_One2One_Context()
            : base("name=IndAssoc_One2One")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IndAssoc_One2One_One2End>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.One1End)
                .WithRequiredPrincipal(x => x.One2End)
                .Map(x => x.MapKey("One2EndId"));
            modelBuilder.Entity<IndAssoc_One2One_One1End>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.One2End)
                .WithRequiredDependent(x => x.One1End)
                .Map(x => x.MapKey("One2EndId"));
        }
    }

    public class IndAssoc_One2One_Test : IDisposable
    {
        private const int Entries = 50;

        public IndAssoc_One2One_Test()
        {
            using (var generationContext = new IndAssoc_One2One_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    var one1End = new IndAssoc_One2One_One1End();
                    one1End.dOne1End = Guid.NewGuid().ToString();

                    var one2End = new IndAssoc_One2One_One2End();
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
        public void IndAssoc_One2One_EdgeOnOne1End_LoadOne1End_PopulatesContext()
        {
            using (var context = new IndAssoc_One2One_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<IndAssoc_One2One_One1End>(x => x.One2End);
                shape.Load<IndAssoc_One2One_One1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.One1End_Set.Local.Count);
                Assert.Equal(1, context.One2End_Set.Local.Count);
                Assert.Equal(context.One1End_Set.Local.First().One2End, context.One2End_Set.Local.First());
            }
        }

        [Fact]
        public void IndAssoc_One2One_EdgeOnOne1End_LoadOne2End_PopulatesContext()
        {
            using (var context = new IndAssoc_One2One_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<IndAssoc_One2One_One1End>(x => x.One2End);
                shape.Load<IndAssoc_One2One_One2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.One2End_Set.Local.Count);
                Assert.Equal(0, context.One1End_Set.Local.Count);
            }
        }

        [Fact]
        public void IndAssoc_One2One_EdgeOnOne2End_LoadEnd1End_PopulatesContext()
        {
            using (var context = new IndAssoc_One2One_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<IndAssoc_One2One_One2End>(x => x.One1End);
                shape.Load<IndAssoc_One2One_One1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.One1End_Set.Local.Count);
                Assert.Equal(0, context.One2End_Set.Local.Count);
            }
        }

        [Fact]
        public void IndAssoc_One2One_EdgeOnOne2End_LoadEnd2End_PopulatesContext()
        {
            using (var context = new IndAssoc_One2One_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<IndAssoc_One2One_One2End>(x => x.One1End);
                shape.Load<IndAssoc_One2One_One2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.One2End_Set.Local.Count);
                Assert.Equal(1, context.One1End_Set.Local.Count);
                Assert.Equal(context.One2End_Set.Local.First().One1End, context.One1End_Set.Local.First());
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new IndAssoc_One2One_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
