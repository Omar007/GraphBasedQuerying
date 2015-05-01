using System;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class CompositeKey_Base
    {
        public int Id1 { get; set; }
        public int Id2 { get; set; }
        public string dCompositeKey { get; set; }

        public virtual ICollection<CompositeKey_Related> Related { get; set; }

        public CompositeKey_Base()
        {
            Related = new HashSet<CompositeKey_Related>();
        }
    }

    public class CompositeKey_Related
    {
        public int Id { get; set; }

        public virtual CompositeKey_Base Base { get; set; }
    }

    public class CompositeKey_Context : DbContext
    {
        public virtual DbSet<CompositeKey_Base> CompositeKey_Set { get; set; }

        public CompositeKey_Context()
            : base("name=CompositeKey")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompositeKey_Base>()
                .HasKey(x => new { x.Id1, x.Id2 })
                .HasMany(x => x.Related)
                .WithRequired(x => x.Base);

            modelBuilder.Entity<CompositeKey_Related>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.Base)
                .WithMany(x => x.Related);
        }
    }

    public class CompositeKey_Test : IDisposable
    {
        private const int Entries = 50;

        public CompositeKey_Test()
        {
            using (var generationContext = new CompositeKey_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    var entity = new CompositeKey_Base();
                    entity.dCompositeKey = Guid.NewGuid().ToString();
                    entity.Id1 = i;
                    entity.Id2 = Entries - i;

                    generationContext.CompositeKey_Set.Add(entity);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void CompositeKey_PopulatesContext()
        {
            using (var context = new CompositeKey_Context())
            {
                Assert.Empty(context.CompositeKey_Set.Local);

                var shape = new SqlGraphShape<object>(context)
                    .Edge<CompositeKey_Base>(x => x.Related);
                var id1 = new Random().Next(Entries);
                var id2 = Entries - id1;
                shape.Load<CompositeKey_Base>(x => x.Id1 == id1 && x.Id2 == id2);

                Assert.Equal(1, context.CompositeKey_Set.Local.Count);
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new CompositeKey_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
