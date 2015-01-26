using System;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class CompositeKeyFK_Base
    {
        public int Id1 { get; set; }
        public int Id2 { get; set; }
        public string dCompositeKeyFK { get; set; }

        public virtual ICollection<CompositeKeyFK_Related> Related { get; set; }

        public CompositeKeyFK_Base()
        {
            Related = new HashSet<CompositeKeyFK_Related>();
        }
    }

    public class CompositeKeyFK_Related
    {
        public int Id { get; set; }

        public int BaseId1 { get; set; }
        public int BaseId2 { get; set; }
        public virtual CompositeKeyFK_Base Base { get; set; }
    }

    public class CompositeKeyFK_Context : DbContext
    {
        public virtual DbSet<CompositeKeyFK_Base> CompositeKeyFK_Set { get; set; }

        public CompositeKeyFK_Context()
            : base("name=CompositeKeyFK")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompositeKeyFK_Base>()
                .HasKey(x => new { x.Id1, x.Id2 })
                .HasMany(x => x.Related)
                .WithRequired(x => x.Base)
                .HasForeignKey(x => new { x.BaseId1, x.BaseId2 });

            modelBuilder.Entity<CompositeKeyFK_Related>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.Base)
                .WithMany(x => x.Related)
                .HasForeignKey(x => new { x.BaseId1, x.BaseId2 });
        }
    }

    public class CompositeKeyFK_Test : IDisposable
    {
        private const int Entries = 50;

        public CompositeKeyFK_Test()
        {
            using (var generationContext = new CompositeKeyFK_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    var entity = new CompositeKeyFK_Base();
                    entity.dCompositeKeyFK = Guid.NewGuid().ToString();
                    entity.Id1 = i;
                    entity.Id2 = Entries - i;

                    generationContext.CompositeKeyFK_Set.Add(entity);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void CompositeKeyFK_PopulatesContext()
        {
            using (var context = new CompositeKeyFK_Context())
            {
                Assert.Empty(context.CompositeKeyFK_Set.Local);

                var shape = new SqlGraphShape<object>(context)
                    .Edge<CompositeKeyFK_Base>(x => x.Related);
                var id1 = new Random().Next(Entries);
                var id2 = Entries - id1;
                shape.Load<CompositeKeyFK_Base>(x => x.Id1 == id1 && x.Id2 == id2);

                Assert.Equal(1, context.CompositeKeyFK_Set.Local.Count);
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new CompositeKeyFK_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
