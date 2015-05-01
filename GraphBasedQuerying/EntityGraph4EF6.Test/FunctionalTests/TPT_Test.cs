using System;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public abstract class TPT_Base
    {
        public int Id { get; set; }
        public string dBase { get; set; }

        public virtual ICollection<TPT_Related> Related { get; set; }

        public TPT_Base()
        {
            Related = new HashSet<TPT_Related>();
        }
    }

    public class TPT_Sub1 : TPT_Base
    {
        public string dSub1 { get; set; }
    }

    public class TPT_Sub2 : TPT_Base
    {
        public string dSub2 { get; set; }
    }

    public class TPT_Related
    {
        public int Id { get; set; }

        public virtual TPT_Base TPT { get; set; }
    }

    public class TPT_Context : DbContext
    {
        public virtual DbSet<TPT_Base> TPT_Set { get; set; }

        public TPT_Context()
            : base("name=TPT")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TPT_Base>()
                .ToTable("Base")
                .HasKey(x => x.Id)
                .HasMany(x => x.Related)
                .WithRequired(x => x.TPT);
            modelBuilder.Entity<TPT_Sub1>()
                .ToTable("Sub1")
                .HasKey(x => x.Id);
            modelBuilder.Entity<TPT_Sub2>()
                .ToTable("Sub2")
                .HasKey(x => x.Id);

            modelBuilder.Entity<TPT_Related>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.TPT)
                .WithMany(x => x.Related);
        }
    }

    public class TPT_Test : IDisposable
    {
        private const int Entries = 25;

        public TPT_Test()
        {
            using (var generationContext = new TPT_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    TPT_Base entity;
                    if (i % 2 == 0)
                    {
                        var sub = new TPT_Sub1();
                        sub.dSub1 = Guid.NewGuid().ToString();
                        entity = sub;
                    }
                    else
                    {
                        var sub = new TPT_Sub2();
                        sub.dSub2 = Guid.NewGuid().ToString();
                        entity = sub;
                    }
                    entity.dBase = Guid.NewGuid().ToString();
                    generationContext.TPT_Set.Add(entity);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void TPT_PopulatesContext()
        {
            using (var context = new TPT_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<TPT_Base>(x => x.Related);
                shape.Load<TPT_Base>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.TPT_Set.Local.Count);
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new TPT_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
