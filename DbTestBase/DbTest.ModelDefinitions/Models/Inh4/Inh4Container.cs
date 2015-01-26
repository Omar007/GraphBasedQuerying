using System.Data.Entity;

namespace DbTest.ModelDefinitions.Models.Inh4
{
    public class Inh4Container : DbContext
    {
        public Inh4Container()
            : base("name=Entities")
        {

        }

        public Inh4Container(string connectionString)
            : base(connectionString)
        {

        }

        public virtual DbSet<Inh4_O> OSet { get; set; }
        public virtual DbSet<Inh4_E00> E00Set { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var o = modelBuilder.Entity<Inh4_O>();
            o.HasMany(x => x.E00s).WithRequired(x => x.O);
            o.Property(x => x.dO).IsRequired();

            var e00 = modelBuilder.Entity<Inh4_E00>();
            e00.ToTable("E00Set");
            e00.Property(x => x.dE00).IsRequired();

            var e10 = modelBuilder.Entity<Inh4_E10>();
            e10.ToTable("E10Set");
            e10.Property(x => x.dE10).IsRequired();

            var e11 = modelBuilder.Entity<Inh4_E11>();
            e11.ToTable("E11Set");
            e11.Property(x => x.dE11).IsRequired();

            var e20 = modelBuilder.Entity<Inh4_E20>();
            e20.ToTable("E20Set");
            e20.Property(x => x.dE20).IsRequired();

            var e21 = modelBuilder.Entity<Inh4_E21>();
            e21.ToTable("E21Set");
            e21.Property(x => x.dE21).IsRequired();

            var e22 = modelBuilder.Entity<Inh4_E22>();
            e22.ToTable("E22Set");
            e22.Property(x => x.dE22).IsRequired();

            var e23 = modelBuilder.Entity<Inh4_E23>();
            e23.ToTable("E23Set");
            e23.Property(x => x.dE23).IsRequired();

            var e30 = modelBuilder.Entity<Inh4_E30>();
            e30.ToTable("E30Set");
            e30.Property(x => x.dE30).IsRequired();

            var e31 = modelBuilder.Entity<Inh4_E31>();
            e31.ToTable("E31Set");
            e31.Property(x => x.dE31).IsRequired();

            var e32 = modelBuilder.Entity<Inh4_E32>();
            e32.ToTable("E32Set");
            e32.Property(x => x.dE32).IsRequired();

            var e33 = modelBuilder.Entity<Inh4_E33>();
            e33.ToTable("E33Set");
            e33.Property(x => x.dE33).IsRequired();

            var e34 = modelBuilder.Entity<Inh4_E34>();
            e34.ToTable("E34Set");
            e34.Property(x => x.dE34).IsRequired();

            var e35 = modelBuilder.Entity<Inh4_E35>();
            e35.ToTable("E35Set");
            e35.Property(x => x.dE35).IsRequired();

            var e36 = modelBuilder.Entity<Inh4_E36>();
            e36.ToTable("E36Set");
            e36.Property(x => x.dE36).IsRequired();

            var e37 = modelBuilder.Entity<Inh4_E37>();
            e37.ToTable("E37Set");
            e37.Property(x => x.dE37).IsRequired();
        }
    }
}
