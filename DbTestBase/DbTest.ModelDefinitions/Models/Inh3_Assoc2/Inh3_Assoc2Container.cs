using System.Data.Entity;

namespace DbTest.ModelDefinitions.Models.Inh3_Assoc2
{
    public class Inh3_Assoc2Container : DbContext
    {
        public Inh3_Assoc2Container()
            : base("name=Entities")
        {

        }

        public Inh3_Assoc2Container(string connectionString)
            : base(connectionString)
        {

        }

        public virtual DbSet<Inh3_Assoc2_O> OSet { get; set; }
        public virtual DbSet<Inh3_Assoc2_E00> E00Set { get; set; }
        public virtual DbSet<Inh3_Assoc2_A00> A00Set { get; set; }
        public virtual DbSet<Inh3_Assoc2_B00> B00Set { get; set; }
        public virtual DbSet<Inh3_Assoc2_C00> C00Set { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var o = modelBuilder.Entity<Inh3_Assoc2_O>();
            o.HasMany(x => x.E00s).WithRequired(x => x.O);
            o.Property(x => x.dO).IsRequired();

            var e00 = modelBuilder.Entity<Inh3_Assoc2_E00>();
            e00.ToTable("E00Set");
            e00.Property(x => x.dE00).IsRequired();
            e00.HasMany(x => x.A00s).WithOptional(x => x.E00);

            var e10 = modelBuilder.Entity<Inh3_Assoc2_E10>();
            e10.ToTable("E10Set");
            e10.Property(x => x.dE10).IsRequired();

            var e11 = modelBuilder.Entity<Inh3_Assoc2_E11>();
            e11.ToTable("E11Set");
            e11.Property(x => x.dE11).IsRequired();

            var e20 = modelBuilder.Entity<Inh3_Assoc2_E20>();
            e20.ToTable("E20Set");
            e20.Property(x => x.dE20).IsRequired();

            var e21 = modelBuilder.Entity<Inh3_Assoc2_E21>();
            e21.ToTable("E21Set");
            e21.Property(x => x.dE21).IsRequired();

            var e22 = modelBuilder.Entity<Inh3_Assoc2_E22>();
            e22.ToTable("E22Set");
            e22.Property(x => x.dE22).IsRequired();

            var e23 = modelBuilder.Entity<Inh3_Assoc2_E23>();
            e23.ToTable("E23Set");
            e23.Property(x => x.dE23).IsRequired();

            var a00 = modelBuilder.Entity<Inh3_Assoc2_A00>();
            a00.ToTable("A00Set");
            a00.Property(x => x.dA00).IsRequired();

            var a10 = modelBuilder.Entity<Inh3_Assoc2_A10>();
            a10.ToTable("A10Set");
            a10.Property(x => x.dA10).IsRequired();
            a10.HasMany(x => x.B00s).WithRequired(x => x.A10);

            var a11 = modelBuilder.Entity<Inh3_Assoc2_A11>();
            a11.ToTable("A11Set");
            a11.Property(x => x.dA11).IsRequired();
            a11.HasMany(x => x.C00s).WithRequired(x => x.A11);

            var a12 = modelBuilder.Entity<Inh3_Assoc2_A12>();
            a12.ToTable("A12Set");
            a12.Property(x => x.dA12).IsRequired();

            var b00 = modelBuilder.Entity<Inh3_Assoc2_B00>();
            b00.Property(x => x.dB00).IsRequired();

            var c00 = modelBuilder.Entity<Inh3_Assoc2_C00>();
            c00.Property(x => x.dC00).IsRequired();
        }
    }
}
