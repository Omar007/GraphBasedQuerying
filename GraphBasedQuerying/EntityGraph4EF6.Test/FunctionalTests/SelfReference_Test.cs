using System;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class SelfReference_Base
    {
        public int Id { get; set; }
        public string dSelfReference { get; set; }

        public virtual SelfReference_Base RelatedSelf { get; set; }
        public virtual ICollection<SelfReference_Base> RelatedToSelfs { get; set; }

        public SelfReference_Base()
        {
            RelatedToSelfs = new HashSet<SelfReference_Base>();
        }
    }

    public class SelfReference_Context : DbContext
    {
        public virtual DbSet<SelfReference_Base> SelfReference_Set { get; set; }

        public SelfReference_Context()
            : base("name=SelfReference")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SelfReference_Base>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.RelatedSelf)
                .WithMany(x => x.RelatedToSelfs);
        }
    }

    public class SelfReference_Test : IDisposable
    {
        private const int Entries = 50;
        private const int RelatedEntries = 10;

        public SelfReference_Test()
        {
            using (var generationContext = new SelfReference_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    var entity = new SelfReference_Base();
                    entity.dSelfReference = Guid.NewGuid().ToString();

                    for (int j = 0; j < RelatedEntries; j++)
                    {
                        var relatedEntity = new SelfReference_Base();
                        relatedEntity.dSelfReference = Guid.NewGuid().ToString();
                        generationContext.SelfReference_Set.Add(relatedEntity);

                        entity.RelatedToSelfs.Add(relatedEntity);
                    }

                    generationContext.SelfReference_Set.Add(entity);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact(Skip="Creates infinite loop in path construction")]
        public void SelfReference_PopulatesContext()
        {
            using (var context = new SelfReference_Context())
            {
                var shape = new SqlGraphShape<object>(context);
                shape.Load<SelfReference_Base>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(RelatedEntries + 1, context.SelfReference_Set.Local.Count);
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new SelfReference_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
