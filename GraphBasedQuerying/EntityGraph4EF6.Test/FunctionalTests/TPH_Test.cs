using System;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public abstract class TPH_Base
    {
        public int Id { get; set; }
        public string dBase { get; set; }

        public virtual ICollection<TPH_Related> Related { get; set; }

        public TPH_Base()
        {
            Related = new HashSet<TPH_Related>();
        }
    }

    public class TPH_Sub1 : TPH_Base
    {
        public string dSub1 { get; set; }
    }

    public class TPH_Sub2 : TPH_Base
    {
        public string dSub2 { get; set; }
    }

    public class TPH_Related
    {
        public int Id { get; set; }

        public virtual TPH_Base TPH { get; set; }
    }

    public class TPH_Context : DbContext
    {
        public virtual DbSet<TPH_Base> TPH_Set { get; set; }

        public TPH_Context()
            : base("name=TPH")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TPH_Base>()
                .Map<TPH_Sub1>(m => m.Requires("SubType").HasValue(1))
                .Map<TPH_Sub2>(m => m.Requires("SubType").HasValue(2))
                .HasKey(x => x.Id)
                .HasMany(x => x.Related)
                .WithRequired(x => x.TPH);

            modelBuilder.Entity<TPH_Related>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.TPH)
                .WithMany(x => x.Related);
        }
    }

    public class TPH_Test : IDisposable
    {
        private const int Entries = 25;

        public TPH_Test()
        {
            using (var generationContext = new TPH_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    TPH_Base entity;
                    if (i % 2 == 0)
                    {
                        var sub = new TPH_Sub1();
                        sub.dSub1 = Guid.NewGuid().ToString();
                        entity = sub;
                    }
                    else
                    {
                        var sub = new TPH_Sub2();
                        sub.dSub2 = Guid.NewGuid().ToString();
                        entity = sub;
                    }
                    entity.dBase = Guid.NewGuid().ToString();
                    generationContext.TPH_Set.Add(entity);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void TPH_PopulatesContext()
        {
            using (var context = new TPH_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<TPH_Base>(x => x.Related);
                shape.Load<TPH_Base>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.TPH_Set.Local.Count);
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new TPH_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
