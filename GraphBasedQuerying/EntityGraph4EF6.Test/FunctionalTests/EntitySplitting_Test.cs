using System;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class EntitySplitting_Base
    {
        public int Id { get; set; }
        public string dSplit1 { get; set; }
        public string dSplit2 { get; set; }

        public virtual ICollection<EntitySplitting_Related> Related { get; set; }

        public EntitySplitting_Base()
        {
            Related = new HashSet<EntitySplitting_Related>();
        }
    }

    public class EntitySplitting_Related
    {
        public int Id { get; set; }

        public virtual EntitySplitting_Base Base { get; set; }
    }

    public class EntitySplitting_Context : DbContext
    {
        public virtual DbSet<EntitySplitting_Base> EntitySplitting_Set { get; set; }

        public EntitySplitting_Context()
            : base("name=EntitySplitting")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntitySplitting_Base>()
                .Map(m =>
                {
                    m.Properties(p => p.dSplit1);
                    m.ToTable("Split1");
                })
                .Map(m =>
                {
                    m.Properties(p => p.dSplit2);
                    m.ToTable("Split2");
                })
            .HasKey(x => x.Id)
            .HasMany(x => x.Related)
            .WithRequired(x => x.Base);

            modelBuilder.Entity<EntitySplitting_Related>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.Base)
                .WithMany(x => x.Related);
        }
    }

    public class EntitySplitting_Test : IDisposable
    {
        private const int Entries = 50;

        public EntitySplitting_Test()
        {
            using (var generationContext = new EntitySplitting_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    var entity = new EntitySplitting_Base();
                    entity.dSplit1 = Guid.NewGuid().ToString();
                    entity.dSplit2 = Guid.NewGuid().ToString();

                    generationContext.EntitySplitting_Set.Add(entity);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void EntitySplitting_PopulatesContext()
        {
            using (var context = new EntitySplitting_Context())
            {
                Assert.Empty(context.EntitySplitting_Set.Local);

                var shape = new SqlGraphShape<object>(context)
                    .Edge<EntitySplitting_Base>(x => x.Related);
                shape.Load<EntitySplitting_Base>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.EntitySplitting_Set.Local.Count);
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new EntitySplitting_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
