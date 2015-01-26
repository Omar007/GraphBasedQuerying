using System.Data.Entity;

namespace DbTest.ModelDefinitions.Models.Inh5
{
    public class Inh5Container : DbContext
    {
        public Inh5Container()
            : base("name=Entities")
        {

        }

        public Inh5Container(string connectionString)
            : base(connectionString)
        {

        }

        public virtual DbSet<Inh5_O> OSet { get; set; }
        public virtual DbSet<Inh5_E00> E00Set { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var o = modelBuilder.Entity<Inh5_O>();
            o.HasMany(x => x.E00s).WithRequired(x => x.O);
            o.Property(x => x.dO).IsRequired();

            var e00 = modelBuilder.Entity<Inh5_E00>();
            e00.ToTable("E00Set");
            e00.Property(x => x.dE00).IsRequired();

            var e10 = modelBuilder.Entity<Inh5_E10>();
            e10.ToTable("E10Set");
            e10.Property(x => x.dE10).IsRequired();

            var e11 = modelBuilder.Entity<Inh5_E11>();
            e11.ToTable("E11Set");
            e11.Property(x => x.dE11).IsRequired();

            var e20 = modelBuilder.Entity<Inh5_E20>();
            e20.ToTable("E20Set");
            e20.Property(x => x.dE20).IsRequired();

            var e21 = modelBuilder.Entity<Inh5_E21>();
            e21.ToTable("E21Set");
            e21.Property(x => x.dE21).IsRequired();

            var e22 = modelBuilder.Entity<Inh5_E22>();
            e22.ToTable("E22Set");
            e22.Property(x => x.dE22).IsRequired();

            var e23 = modelBuilder.Entity<Inh5_E23>();
            e23.ToTable("E23Set");
            e23.Property(x => x.dE23).IsRequired();

            var e30 = modelBuilder.Entity<Inh5_E30>();
            e30.ToTable("E30Set");
            e30.Property(x => x.dE30).IsRequired();

            var e31 = modelBuilder.Entity<Inh5_E31>();
            e31.ToTable("E31Set");
            e31.Property(x => x.dE31).IsRequired();

            var e32 = modelBuilder.Entity<Inh5_E32>();
            e32.ToTable("E32Set");
            e32.Property(x => x.dE32).IsRequired();

            var e33 = modelBuilder.Entity<Inh5_E33>();
            e33.ToTable("E33Set");
            e33.Property(x => x.dE33).IsRequired();

            var e34 = modelBuilder.Entity<Inh5_E34>();
            e34.ToTable("E34Set");
            e34.Property(x => x.dE34).IsRequired();

            var e35 = modelBuilder.Entity<Inh5_E35>();
            e35.ToTable("E35Set");
            e35.Property(x => x.dE35).IsRequired();

            var e36 = modelBuilder.Entity<Inh5_E36>();
            e36.ToTable("E36Set");
            e36.Property(x => x.dE36).IsRequired();

            var e37 = modelBuilder.Entity<Inh5_E37>();
            e37.ToTable("E37Set");
            e37.Property(x => x.dE37).IsRequired();

            var e40 = modelBuilder.Entity<Inh5_E40>();
            e40.ToTable("E40Set");
            e40.Property(x => x.dE40).IsRequired();

            var e41 = modelBuilder.Entity<Inh5_E41>();
            e41.ToTable("E41Set");
            e41.Property(x => x.dE41).IsRequired();

            var e42 = modelBuilder.Entity<Inh5_E42>();
            e42.ToTable("E42Set");
            e42.Property(x => x.dE42).IsRequired();

            var e43 = modelBuilder.Entity<Inh5_E43>();
            e43.ToTable("E43Set");
            e43.Property(x => x.dE43).IsRequired();

            var e44 = modelBuilder.Entity<Inh5_E44>();
            e44.ToTable("E44Set");
            e44.Property(x => x.dE44).IsRequired();

            var e45 = modelBuilder.Entity<Inh5_E45>();
            e45.ToTable("E45Set");
            e45.Property(x => x.dE45).IsRequired();

            var e46 = modelBuilder.Entity<Inh5_E46>();
            e46.ToTable("E46Set");
            e46.Property(x => x.dE46).IsRequired();

            var e47 = modelBuilder.Entity<Inh5_E47>();
            e47.ToTable("E47Set");
            e47.Property(x => x.dE47).IsRequired();

            var e48 = modelBuilder.Entity<Inh5_E48>();
            e48.ToTable("E48Set");
            e48.Property(x => x.dE48).IsRequired();

            var e49 = modelBuilder.Entity<Inh5_E49>();
            e49.ToTable("E49Set");
            e49.Property(x => x.dE49).IsRequired();

            var e410 = modelBuilder.Entity<Inh5_E410>();
            e410.ToTable("E410Set");
            e410.Property(x => x.dE410).IsRequired();

            var e411 = modelBuilder.Entity<Inh5_E411>();
            e411.ToTable("E411Set");
            e411.Property(x => x.dE411).IsRequired();

            var e412 = modelBuilder.Entity<Inh5_E412>();
            e412.ToTable("E412Set");
            e412.Property(x => x.dE412).IsRequired();

            var e413 = modelBuilder.Entity<Inh5_E413>();
            e413.ToTable("E413Set");
            e413.Property(x => x.dE413).IsRequired();

            var e414 = modelBuilder.Entity<Inh5_E414>();
            e414.ToTable("E414Set");
            e414.Property(x => x.dE414).IsRequired();

            var e415 = modelBuilder.Entity<Inh5_E415>();
            e415.ToTable("E415Set");
            e415.Property(x => x.dE415).IsRequired();
        }
    }
}
