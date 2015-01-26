using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class FKAssoc_Optional2Many_ManyEnd
    {
        public int Id { get; set; }
        public string dManyEnd { get; set; }

        public Nullable<int> OptionalEndId { get; set; }
        public virtual FKAssoc_Optional2Many_OptionalEnd OptionalEnd { get; set; }
    }

    public class FKAssoc_Optional2Many_OptionalEnd
    {
        public int Id { get; set; }
        public string dOptionalEnd { get; set; }

        public virtual ICollection<FKAssoc_Optional2Many_ManyEnd> ManyEnds { get; set; }

        public FKAssoc_Optional2Many_OptionalEnd()
        {
            ManyEnds = new HashSet<FKAssoc_Optional2Many_ManyEnd>();
        }
    }

    public class FKAssoc_Optional2Many_Context : DbContext
    {
        public virtual DbSet<FKAssoc_Optional2Many_ManyEnd> ManyEnd_Set { get; set; }
        public virtual DbSet<FKAssoc_Optional2Many_OptionalEnd> OptionalEnd_Set { get; set; }

        public FKAssoc_Optional2Many_Context()
            : base("name=FKAssoc_Optional2Many")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FKAssoc_Optional2Many_ManyEnd>()
                .HasKey(x => x.Id)
                .HasOptional(x => x.OptionalEnd)
                .WithMany(x => x.ManyEnds)
                .HasForeignKey(x => x.OptionalEndId);
            modelBuilder.Entity<FKAssoc_Optional2Many_OptionalEnd>()
                .HasKey(x => x.Id)
                .HasMany(x => x.ManyEnds)
                .WithOptional(x => x.OptionalEnd)
                .HasForeignKey(x => x.OptionalEndId);
        }
    }

    public class FKAssoc_Optional2Many_Test : IDisposable
    {
        private const int OptionalEntries = 50;
        private const int ManyEntriesPerOptional = 10;

        public FKAssoc_Optional2Many_Test()
        {
            using (var generationContext = new FKAssoc_Optional2Many_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < OptionalEntries; i++)
                {
                    var optionalEnd = new FKAssoc_Optional2Many_OptionalEnd();
                    optionalEnd.dOptionalEnd = Guid.NewGuid().ToString();

                    for (int j = 0; j < ManyEntriesPerOptional; j++)
                    {
                        var manyEnd = new FKAssoc_Optional2Many_ManyEnd();
                        manyEnd.dManyEnd = Guid.NewGuid().ToString();

                        if (i % 2 == 0)
                        {
                            manyEnd.OptionalEnd = optionalEnd;
                            optionalEnd.ManyEnds.Add(manyEnd);
                        }

                        generationContext.ManyEnd_Set.Add(manyEnd);
                    }

                    generationContext.OptionalEnd_Set.Add(optionalEnd);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void FKAssoc_Optional2Many_EdgeOnManyEnd_LoadManyEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_Optional2Many_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_Optional2Many_ManyEnd>(x => x.OptionalEnd);
                shape.Load<FKAssoc_Optional2Many_ManyEnd>(x => x.Id == new Random().Next(1, ((OptionalEntries * ManyEntriesPerOptional) / 2) + 1) * 2);

                Assert.Equal(1, context.ManyEnd_Set.Local.Count);
                Assert.Equal(((context.ManyEnd_Set.Local.First().Id - 1) / ManyEntriesPerOptional) % 2 == 0 ? 1 : 0, context.OptionalEnd_Set.Local.Count);
                Assert.Equal(context.ManyEnd_Set.Local.First().OptionalEnd, context.OptionalEnd_Set.Local.FirstOrDefault());
            }
        }

        [Fact]
        public void FKAssoc_Optional2Many_EdgeOnManyEnd_LoadOptionalEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_Optional2Many_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_Optional2Many_ManyEnd>(x => x.OptionalEnd);
                shape.Load<FKAssoc_Optional2Many_OptionalEnd>(x => x.Id == new Random().Next(1, OptionalEntries + 1));

                Assert.Equal(1, context.OptionalEnd_Set.Local.Count);
                Assert.Equal(0, context.ManyEnd_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_Optional2Many_EdgeOnOptionalEnd_LoadManyEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_Optional2Many_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_Optional2Many_OptionalEnd>(x => x.ManyEnds);
                shape.Load<FKAssoc_Optional2Many_ManyEnd>(x => x.Id == new Random().Next(1, ((OptionalEntries * ManyEntriesPerOptional) / 2) + 1) * 2);

                Assert.Equal(1, context.ManyEnd_Set.Local.Count);
                Assert.Equal(0, context.OptionalEnd_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_Optional2Many_EdgeOnOptionalEnd_LoadOptionalEnd_PopulatesContext()
        {
            using (var context = new FKAssoc_Optional2Many_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_Optional2Many_OptionalEnd>(x => x.ManyEnds);
                shape.Load<FKAssoc_Optional2Many_OptionalEnd>(x => x.Id == new Random().Next(1, OptionalEntries + 1));

                Assert.Equal(1, context.OptionalEnd_Set.Local.Count);
                Assert.Equal((context.OptionalEnd_Set.Local.First().Id - 1) % 2 == 0
                    ? ManyEntriesPerOptional : 0, context.ManyEnd_Set.Local.Count);
                Assert.True(Enumerable.SequenceEqual(context.OptionalEnd_Set.Local.First().ManyEnds, context.ManyEnd_Set.Local));
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new FKAssoc_Optional2Many_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
