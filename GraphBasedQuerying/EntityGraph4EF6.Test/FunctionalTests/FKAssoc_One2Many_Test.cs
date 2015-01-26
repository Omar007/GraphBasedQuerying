using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class FKAssoc_One2Many_ManyEnd
    {
        public int Id { get; set; }
        public string dManyEnd { get; set; }

        public int OneEndId { get; set; }
        public virtual FKAssoc_One2Many_OneEnd OneEnd { get; set; }
    }

    public class FKAssoc_One2Many_OneEnd
    {
        public int Id { get; set; }
        public string dOneEnd { get; set; }

        public virtual ICollection<FKAssoc_One2Many_ManyEnd> ManyEnds { get; set; }

        public FKAssoc_One2Many_OneEnd()
        {
            ManyEnds = new HashSet<FKAssoc_One2Many_ManyEnd>();
        }
    }

    public class FKAssoc_One2Many_Context : DbContext
    {
        public virtual DbSet<FKAssoc_One2Many_ManyEnd> ManyEnd_Set { get; set; }
        public virtual DbSet<FKAssoc_One2Many_OneEnd> OneEnd_Set { get; set; }

        public FKAssoc_One2Many_Context()
            : base("name=FKAssoc_One2Many")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FKAssoc_One2Many_ManyEnd>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.OneEnd)
                .WithMany(x => x.ManyEnds)
                .HasForeignKey(x => x.OneEndId);
            modelBuilder.Entity<FKAssoc_One2Many_OneEnd>()
                .HasKey(x => x.Id)
                .HasMany(x => x.ManyEnds)
                .WithRequired(x => x.OneEnd)
                .HasForeignKey(x => x.OneEndId);
        }
    }

    public class FKAssoc_One2Many_Test : IDisposable
    {
        private const int OneEntries = 50;
        private const int ManyEntriesPerOne = 10;

        public FKAssoc_One2Many_Test()
        {
            using (var generationContext = new FKAssoc_One2Many_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < OneEntries; i++)
                {
                    var oneEnd = new FKAssoc_One2Many_OneEnd();
                    oneEnd.dOneEnd = Guid.NewGuid().ToString();

                    for (int j = 0; j < ManyEntriesPerOne; j++)
                    {
                        var manyEnd = new FKAssoc_One2Many_ManyEnd();
                        manyEnd.dManyEnd = Guid.NewGuid().ToString();
                        manyEnd.OneEnd = oneEnd;
                        generationContext.ManyEnd_Set.Add(manyEnd);

                        oneEnd.ManyEnds.Add(manyEnd);
                    }

                    generationContext.OneEnd_Set.Add(oneEnd);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void FKAssoc_One2Many_EdgeOnOneEnd_LoadOneEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_One2Many_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2Many_OneEnd>(x => x.ManyEnds);
                shape.Load<FKAssoc_One2Many_OneEnd>(x => x.Id == new Random().Next(1, OneEntries + 1));

                Assert.Equal(1, context.OneEnd_Set.Local.Count);
                Assert.Equal(ManyEntriesPerOne, context.ManyEnd_Set.Local.Count);
                Assert.True(Enumerable.SequenceEqual(context.OneEnd_Set.Local.First().ManyEnds, context.ManyEnd_Set.Local));
            }
        }

        [Fact]
        public void FKAssoc_One2Many_EdgeOnOneEnd_LoadManyEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_One2Many_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2Many_OneEnd>(x => x.ManyEnds);
                shape.Load<FKAssoc_One2Many_ManyEnd>(x => x.Id == new Random().Next(1, (OneEntries * ManyEntriesPerOne) + 1));

                Assert.Equal(1, context.ManyEnd_Set.Local.Count);
                Assert.Equal(0, context.OneEnd_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_One2Many_EdgeOnManyEnd_LoadOneEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_One2Many_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2Many_ManyEnd>(x => x.OneEnd);
                shape.Load<FKAssoc_One2Many_OneEnd>(x => x.Id == new Random().Next(1, OneEntries + 1));

                Assert.Equal(1, context.OneEnd_Set.Local.Count);
                Assert.Equal(0, context.ManyEnd_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_One2Many_EdgeOnManyEnd_LoadManyEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_One2Many_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_One2Many_ManyEnd>(x => x.OneEnd);
                shape.Load<FKAssoc_One2Many_ManyEnd>(x => x.Id == new Random().Next(1, (OneEntries * ManyEntriesPerOne) + 1));

                Assert.Equal(1, context.ManyEnd_Set.Local.Count);
                Assert.Equal(1, context.OneEnd_Set.Local.Count);
                Assert.Equal(context.ManyEnd_Set.Local.First().OneEnd, context.OneEnd_Set.Local.First());
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new FKAssoc_One2Many_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
