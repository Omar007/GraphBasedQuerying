using System;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public abstract class TPC_Base
    {
        public int Id { get; set; }
        public string dBase { get; set; }
    }

    public class TPC_Sub1 : TPC_Base
    {
        public string dSub1 { get; set; }

        public virtual ICollection<TPC_Sub1_Related> Sub1Related { get; set; }
        
        public TPC_Sub1()
        {
            Sub1Related = new HashSet<TPC_Sub1_Related>();
        }
    }

    public class TPC_Sub2 : TPC_Base
    {
        public string dSub2 { get; set; }

        public virtual ICollection<TPC_Sub2_Related> Sub2Related { get; set; }

        public TPC_Sub2()
        {
            Sub2Related = new HashSet<TPC_Sub2_Related>();
        }
    }

    public class TPC_Sub1_Related
    {
        public int Id { get; set; }

        public virtual TPC_Sub1 Sub1 { get; set; }
    }

    public class TPC_Sub2_Related
    {
        public int Id { get; set; }

        public virtual TPC_Sub2 Sub2 { get; set; }
    }

    public class TPC_Context : DbContext
    {
        public virtual DbSet<TPC_Base> TPC_Set { get; set; }

        public TPC_Context()
            : base("name=TPC")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TPC_Sub1>()
                .Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("Sub1");
                })
                .HasKey(x => x.Id)
                .HasMany(x => x.Sub1Related)
                .WithRequired(x => x.Sub1);
            modelBuilder.Entity<TPC_Sub2>()
                .Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("Sub2");
                })
                .HasKey(x => x.Id)
                .HasMany(x => x.Sub2Related)
                .WithRequired(x => x.Sub2);

            modelBuilder.Entity<TPC_Sub1_Related>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.Sub1)
                .WithMany(x => x.Sub1Related);
            modelBuilder.Entity<TPC_Sub2_Related>()
                .HasKey(x => x.Id)
                .HasRequired(x => x.Sub2)
                .WithMany(x => x.Sub2Related);
        }
    }

    public class TPC_Test : IDisposable
    {
        private const int Entries = 25;

        public TPC_Test()
        {
            using (var generationContext = new TPC_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    TPC_Base entity;
                    if (i % 2 == 0)
                    {
                        var sub = new TPC_Sub1();
                        sub.dSub1 = Guid.NewGuid().ToString();
                        entity = sub;
                    }
                    else
                    {
                        var sub = new TPC_Sub2();
                        sub.dSub2 = Guid.NewGuid().ToString();
                        entity = sub;
                    }
                    entity.Id = i;
                    entity.dBase = Guid.NewGuid().ToString();
                    generationContext.TPC_Set.Add(entity);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void TPC_PopulatesContext()
        {
            using (var context = new TPC_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<TPC_Sub1>(x => x.Sub1Related)
                    .Edge<TPC_Sub2>(x => x.Sub2Related);
                shape.Load<TPC_Base>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.TPC_Set.Local.Count);
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new TPC_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
