using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace EntityGraph4EF6.Test.FunctionalTests
{
    public class FKAssoc_ManyToManyLinkEntity_Many1End
    {
        public int Id { get; set; }
        public string dMany1End { get; set; }

        public virtual ICollection<FKAssoc_ManyToManyLinkEntity_LinkEntity> LinkEntities { get; set; }

        public FKAssoc_ManyToManyLinkEntity_Many1End()
        {
            LinkEntities = new HashSet<FKAssoc_ManyToManyLinkEntity_LinkEntity>();
        }
    }

    public class FKAssoc_ManyToManyLinkEntity_Many2End
    {
        public int Id { get; set; }
        public string dMany2End { get; set; }

        public virtual ICollection<FKAssoc_ManyToManyLinkEntity_LinkEntity> LinkEntities { get; set; }

        public FKAssoc_ManyToManyLinkEntity_Many2End()
        {
            LinkEntities = new HashSet<FKAssoc_ManyToManyLinkEntity_LinkEntity>();
        }
    }

    public class FKAssoc_ManyToManyLinkEntity_LinkEntity
    {
        public int Many1Id { get; set; }
        public int Many2Id { get; set; }
        public string dLinkEntity { get; set; }

        public virtual FKAssoc_ManyToManyLinkEntity_Many1End Many1End { get; set; }
        public virtual FKAssoc_ManyToManyLinkEntity_Many2End Many2End { get; set; }
    }

    public class FKAssoc_ManyToManyLinkEntity_Context : DbContext
    {
        public virtual DbSet<FKAssoc_ManyToManyLinkEntity_Many1End> Many1End_Set { get; set; }
        public virtual DbSet<FKAssoc_ManyToManyLinkEntity_Many2End> Many2End_Set { get; set; }
        public virtual DbSet<FKAssoc_ManyToManyLinkEntity_LinkEntity> LinkEntity_Set { get; set; }

        public FKAssoc_ManyToManyLinkEntity_Context()
            : base("name=FKAssoc_ManyToManyLinkEntity")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FKAssoc_ManyToManyLinkEntity_Many1End>()
                .HasKey(x => x.Id)
                .HasMany(x => x.LinkEntities)
                .WithRequired(x => x.Many1End)
                .HasForeignKey(x => x.Many1Id);

            modelBuilder.Entity<FKAssoc_ManyToManyLinkEntity_Many2End>()
                .HasKey(x => x.Id)
                .HasMany(x => x.LinkEntities)
                .WithRequired(x => x.Many2End)
                .HasForeignKey(x => x.Many2Id);

            var linkEntity = modelBuilder.Entity<FKAssoc_ManyToManyLinkEntity_LinkEntity>()
                .HasKey(x => new { x.Many1Id, x.Many2Id });
            linkEntity.HasRequired(x => x.Many1End)
                .WithMany(x => x.LinkEntities)
                .HasForeignKey(x => x.Many1Id);
            linkEntity.HasRequired(x => x.Many2End)
                .WithMany(x => x.LinkEntities)
                .HasForeignKey(x => x.Many2Id);
        }
    }

    public class FKFKAssoc_Many2ManyLinkEntity_Test : IDisposable
    {
        private const int Entries = 20;
        private const int EntriesPerEntry = 5;

        public FKFKAssoc_Many2ManyLinkEntity_Test()
        {
            using (var generationContext = new FKAssoc_ManyToManyLinkEntity_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    var many1End = new FKAssoc_ManyToManyLinkEntity_Many1End();
                    many1End.dMany1End = Guid.NewGuid().ToString();
                    for (int j = 0; j < EntriesPerEntry; j++)
                    {
                        var many2 = new FKAssoc_ManyToManyLinkEntity_Many2End();
                        many2.dMany2End = Guid.NewGuid().ToString();

                        var linkEntity = new FKAssoc_ManyToManyLinkEntity_LinkEntity();
                        linkEntity.dLinkEntity = Guid.NewGuid().ToString();
                        linkEntity.Many1End = many1End;
                        linkEntity.Many2End = many2;
                        generationContext.LinkEntity_Set.Add(linkEntity);

                        many1End.LinkEntities.Add(linkEntity);
                        many2.LinkEntities.Add(linkEntity);

                        generationContext.Many2End_Set.Add(many2);
                    }
                    generationContext.Many1End_Set.Add(many1End);

                    var many2End = new FKAssoc_ManyToManyLinkEntity_Many2End();
                    many2End.dMany2End = Guid.NewGuid().ToString();
                    for (int j = 0; j < EntriesPerEntry; j++)
                    {
                        var many1 = new FKAssoc_ManyToManyLinkEntity_Many1End();
                        many1.dMany1End = Guid.NewGuid().ToString();
                        generationContext.Many1End_Set.Add(many1);

                        var linkEntity = new FKAssoc_ManyToManyLinkEntity_LinkEntity();
                        linkEntity.dLinkEntity = Guid.NewGuid().ToString();
                        linkEntity.Many1End = many1;
                        linkEntity.Many2End = many2End;
                        generationContext.LinkEntity_Set.Add(linkEntity);

                        many1.LinkEntities.Add(linkEntity);
                        many2End.LinkEntities.Add(linkEntity);

                        generationContext.Many1End_Set.Add(many1);
                    }
                    generationContext.Many2End_Set.Add(many2End);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void FKAssoc_ManyToManyLinkEntity_EdgeOnLinkEntity_LoadMany1End_PopulatesContext()
        {
            using (var context = new FKAssoc_ManyToManyLinkEntity_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_ManyToManyLinkEntity_LinkEntity>(x => x.Many1End)
                    .Edge<FKAssoc_ManyToManyLinkEntity_LinkEntity>(x => x.Many2End);
                shape.Load<FKAssoc_ManyToManyLinkEntity_Many1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many1End_Set.Local.Count);
                Assert.Equal(context.Many1End_Set.Local.First().LinkEntities.Count, context.LinkEntity_Set.Local.Count);
                Assert.Equal(0, context.Many2End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_ManyToManyLinkEntity_EdgeOnLinkEntity_LoadMany2End_PopulatesContext()
        {
            using (var context = new FKAssoc_ManyToManyLinkEntity_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_ManyToManyLinkEntity_LinkEntity>(x => x.Many1End)
                    .Edge<FKAssoc_ManyToManyLinkEntity_LinkEntity>(x => x.Many2End);
                shape.Load<FKAssoc_ManyToManyLinkEntity_Many2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many2End_Set.Local.Count);
                Assert.Equal(context.Many2End_Set.Local.First().LinkEntities.Count, context.LinkEntity_Set.Local.Count);
                Assert.Equal(0, context.Many1End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_ManyToManyLinkEntity_EdgeOnManyEnds_LoadMany1End_PopulatesContext()
        {
            using (var context = new FKAssoc_ManyToManyLinkEntity_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_ManyToManyLinkEntity_Many1End>(x => x.LinkEntities)
                    .Edge<FKAssoc_ManyToManyLinkEntity_Many2End>(x => x.LinkEntities);
                shape.Load<FKAssoc_ManyToManyLinkEntity_Many1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many1End_Set.Local.Count);
                Assert.Equal(context.Many1End_Set.Local.First().LinkEntities.Count, context.LinkEntity_Set.Local.Count);
                Assert.Equal(0, context.Many2End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_ManyToManyLinkEntity_EdgeOnManyEnds_LoadMany2End_PopulatesContext()
        {
            using (var context = new FKAssoc_ManyToManyLinkEntity_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_ManyToManyLinkEntity_Many1End>(x => x.LinkEntities)
                    .Edge<FKAssoc_ManyToManyLinkEntity_Many2End>(x => x.LinkEntities);
                shape.Load<FKAssoc_ManyToManyLinkEntity_Many2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many2End_Set.Local.Count);
                Assert.Equal(context.Many2End_Set.Local.First().LinkEntities.Count, context.LinkEntity_Set.Local.Count);
                Assert.Equal(0, context.Many1End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_ManyToManyLinkEntity_EdgeOnMany1End_LoadMany1End_PopulatesContext()
        {
            using (var context = new FKAssoc_ManyToManyLinkEntity_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_ManyToManyLinkEntity_Many1End>(x => x.LinkEntities)
                    .Edge<FKAssoc_ManyToManyLinkEntity_LinkEntity>(x => x.Many2End);
                shape.Load<FKAssoc_ManyToManyLinkEntity_Many1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many1End_Set.Local.Count);
                Assert.Equal(context.Many1End_Set.Local.First().LinkEntities.Count, context.LinkEntity_Set.Local.Count);
                Assert.Equal(context.Many1End_Set.Local.First().LinkEntities.Count, context.Many2End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_ManyToManyLinkEntity_EdgeOnMany1End_LoadMany2End_PopulatesContext()
        {
            using (var context = new FKAssoc_ManyToManyLinkEntity_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_ManyToManyLinkEntity_Many1End>(x => x.LinkEntities)
                    .Edge<FKAssoc_ManyToManyLinkEntity_LinkEntity>(x => x.Many2End);
                shape.Load<FKAssoc_ManyToManyLinkEntity_Many2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many2End_Set.Local.Count);
                Assert.Equal(context.Many2End_Set.Local.First().LinkEntities.Count, context.LinkEntity_Set.Local.Count);
                Assert.Equal(0, context.Many1End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_ManyToManyLinkEntity_EdgeOnMany2End_LoadMany1End_PopulatesContext()
        {
            using (var context = new FKAssoc_ManyToManyLinkEntity_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_ManyToManyLinkEntity_Many2End>(x => x.LinkEntities)
                    .Edge<FKAssoc_ManyToManyLinkEntity_LinkEntity>(x => x.Many1End);
                shape.Load<FKAssoc_ManyToManyLinkEntity_Many1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many1End_Set.Local.Count);
                Assert.Equal(context.Many1End_Set.Local.First().LinkEntities.Count, context.LinkEntity_Set.Local.Count);
                Assert.Equal(0, context.Many2End_Set.Local.Count);
            }
        }

        [Fact]
        public void FKAssoc_ManyToManyLinkEntity_EdgeOnMany2End_LoadMany2End_PopulatesContext()
        {
            using (var context = new FKAssoc_ManyToManyLinkEntity_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<FKAssoc_ManyToManyLinkEntity_Many2End>(x => x.LinkEntities)
                    .Edge<FKAssoc_ManyToManyLinkEntity_LinkEntity>(x => x.Many1End);
                shape.Load<FKAssoc_ManyToManyLinkEntity_Many2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many2End_Set.Local.Count);
                Assert.Equal(context.Many2End_Set.Local.First().LinkEntities.Count, context.LinkEntity_Set.Local.Count);
                Assert.Equal(context.Many2End_Set.Local.First().LinkEntities.Count, context.Many1End_Set.Local.Count);
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new FKAssoc_ManyToManyLinkEntity_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
