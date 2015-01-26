using System.Data.Entity;

namespace DbTest.ModelDefinitions.Models.Inh3
{
    public class Inh3Container : DbContext
    {
        public Inh3Container()
            : base("name=Entities")
        {

        }

        public Inh3Container(string connectionString)
            : base(connectionString)
        {

        }

        public virtual DbSet<Inh3_O> OSet { get; set; }
        public virtual DbSet<Inh3_E00> E00Set { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var o = modelBuilder.Entity<Inh3_O>();
            o.HasMany(x => x.E00s).WithRequired(x => x.O);
            o.Property(x => x.dO).IsRequired();

            var e00 = modelBuilder.Entity<Inh3_E00>();
            e00.ToTable("E00Set");
            e00.Property(x => x.dE00).IsRequired();

            var e10 = modelBuilder.Entity<Inh3_E10>();
            e10.ToTable("E10Set");
            e10.Property(x => x.dE10).IsRequired();

            var e11 = modelBuilder.Entity<Inh3_E11>();
            e11.ToTable("E11Set");
            e11.Property(x => x.dE11).IsRequired();

            var e20 = modelBuilder.Entity<Inh3_E20>();
            e20.ToTable("E20Set");
            e20.Property(x => x.dE20).IsRequired();

            var e21 = modelBuilder.Entity<Inh3_E21>();
            e21.ToTable("E21Set");
            e21.Property(x => x.dE21).IsRequired();

            var e22 = modelBuilder.Entity<Inh3_E22>();
            e22.ToTable("E22Set");
            e22.Property(x => x.dE22).IsRequired();

            var e23 = modelBuilder.Entity<Inh3_E23>();
            e23.ToTable("E23Set");
            e23.Property(x => x.dE23).IsRequired();
        }
    }
}
