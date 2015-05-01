using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace EntityGraph4EF6.Test
{
    public class IndAssoc_ManyToMany_Many2End
    {
        public int Id { get; set; }
        public string dMany2End { get; set; }

        public virtual ICollection<IndAssoc_ManyToMany_Many1End> Many1Ends { get; set; }

        public IndAssoc_ManyToMany_Many2End()
        {
            Many1Ends = new HashSet<IndAssoc_ManyToMany_Many1End>();
        }
    }

    public class IndAssoc_ManyToMany_Many1End
    {
        public int Id { get; set; }
        public string dMany1End { get; set; }

        public virtual ICollection<IndAssoc_ManyToMany_Many2End> Many2Ends { get; set; }

        public IndAssoc_ManyToMany_Many1End()
        {
            Many2Ends = new HashSet<IndAssoc_ManyToMany_Many2End>();
        }
    }

    public class IndAssoc_ManyToMany_Context : DbContext
    {
        public virtual DbSet<IndAssoc_ManyToMany_Many2End> Many2End_Set { get; set; }
        public virtual DbSet<IndAssoc_ManyToMany_Many1End> Many1End_Set { get; set; }

        public IndAssoc_ManyToMany_Context()
            : base("name=IndAssoc_ManyToMany")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IndAssoc_ManyToMany_Many2End>()
                .HasKey(x => x.Id)
                .HasMany(x => x.Many1Ends)
                .WithMany(x => x.Many2Ends);
            modelBuilder.Entity<IndAssoc_ManyToMany_Many1End>()
                .HasKey(x => x.Id)
                .HasMany(x => x.Many2Ends)
                .WithMany(x => x.Many1Ends);
        }
    }

    public class IndAssoc_ManyToMany_Test : IDisposable
    {
        private const int Entries = 50;

        public IndAssoc_ManyToMany_Test()
        {
            var many1s = new List<IndAssoc_ManyToMany_Many1End>();
            var many2s = new List<IndAssoc_ManyToMany_Many2End>();

            for (int i = 0; i < Entries; i++)
            {
                var many1End = new IndAssoc_ManyToMany_Many1End();
                many1End.dMany1End = Guid.NewGuid().ToString();
                many1s.Add(many1End);
            }

            for (int i = 0; i < Entries; i++)
            {
                var many2End = new IndAssoc_ManyToMany_Many2End();
                many2End.dMany2End = Guid.NewGuid().ToString();
                many2s.Add(many2End);
            }

            using (var generationContext = new IndAssoc_ManyToMany_Context())
            {
                generationContext.Database.Delete();

                for (int i = 0; i < Entries; i++)
                {
                    var many2sIndex = Entries - 1 - i;
                    many1s[i].Many2Ends = many2s.Take(many2sIndex + 1).ToList();
                    many2s[many2sIndex].Many1Ends = many1s.Skip(i).ToList();

                    generationContext.Many1End_Set.Add(many1s[i]);
                    generationContext.Many2End_Set.Add(many2s[many2sIndex]);

                    generationContext.SaveChanges();
                }
            }
        }

        [Fact]
        public void IndAssoc_ManyToMany_EdgeOnMany1End_LoadMany1End_PopulatesContext()
        {
            using (var context = new IndAssoc_ManyToMany_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<IndAssoc_ManyToMany_Many1End>(x => x.Many2Ends);
                shape.Load<IndAssoc_ManyToMany_Many1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many1End_Set.Local.Count);
                Assert.Equal(context.Many1End_Set.Local.First().Id - 1, context.Many2End_Set.Local.Count);
                Assert.True(Enumerable.SequenceEqual(context.Many1End_Set.Local.First().Many2Ends, context.Many2End_Set.Local));
            }
        }

        [Fact]
        public void IndAssoc_ManyToMany_EdgeOnMany1End_LoadMany2End_PopulatesContext()
        {
            using (var context = new IndAssoc_ManyToMany_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<IndAssoc_ManyToMany_Many1End>(x => x.Many2Ends);
                shape.Load<IndAssoc_ManyToMany_Many2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many2End_Set.Local.Count);
                Assert.Equal(0, context.Many1End_Set.Local.Count);
            }
        }

        [Fact]
        public void IndAssoc_ManyToMany_EdgeOnMany2End_LoadMany1End_PopulatesContext()
        {
            using (var context = new IndAssoc_ManyToMany_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<IndAssoc_ManyToMany_Many2End>(x => x.Many1Ends);
                shape.Load<IndAssoc_ManyToMany_Many1End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many1End_Set.Local.Count);
                Assert.Equal(0, context.Many2End_Set.Local.Count);
            }
        }

        [Fact]
        public void IndAssoc_ManyToMany_EdgeOnMany2End_LoadMany2End_PopulatesContext()
        {
            using (var context = new IndAssoc_ManyToMany_Context())
            {
                var shape = new SqlGraphShape<object>(context)
                    .Edge<IndAssoc_ManyToMany_Many2End>(x => x.Many1Ends);
                shape.Load<IndAssoc_ManyToMany_Many2End>(x => x.Id == new Random().Next(1, Entries + 1));

                Assert.Equal(1, context.Many2End_Set.Local.Count);
                Assert.Equal(Entries - context.Many2End_Set.Local.First().Id, context.Many1End_Set.Local.Count);
                Assert.True(Enumerable.SequenceEqual(context.Many2End_Set.Local.First().Many1Ends, context.Many1End_Set.Local));
            }
        }

        public void Dispose()
        {
            using (var deletionContext = new IndAssoc_ManyToMany_Context())
            {
                deletionContext.Database.Delete();
            }
        }
    }
}
