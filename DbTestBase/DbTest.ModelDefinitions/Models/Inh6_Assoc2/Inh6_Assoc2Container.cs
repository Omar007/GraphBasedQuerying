using System.Data.Entity;

namespace DbTest.ModelDefinitions.Models.Inh6_Assoc2
{
    public class Inh6_Assoc2Container : DbContext
    {
        public Inh6_Assoc2Container()
            : base("name=Entities")
        {

        }

        public Inh6_Assoc2Container(string connectionString)
            : base(connectionString)
        {

        }

        public virtual DbSet<Inh6_Assoc2_O> OSet { get; set; }
        public virtual DbSet<Inh6_Assoc2_E00> E00Set { get; set; }
        public virtual DbSet<Inh6_Assoc2_A00> A00Set { get; set; }
        public virtual DbSet<Inh6_Assoc2_B00> B00Set { get; set; }
        public virtual DbSet<Inh6_Assoc2_C00> C00Set { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var o = modelBuilder.Entity<Inh6_Assoc2_O>();
            o.HasMany(x => x.E00s).WithRequired(x => x.O);
            o.Property(x => x.dO).IsRequired();

            var e00 = modelBuilder.Entity<Inh6_Assoc2_E00>();
            e00.ToTable("E00Set");
            e00.Property(x => x.dE00).IsRequired();
            e00.HasMany(x => x.A00s).WithOptional(x => x.E00);

            var e10 = modelBuilder.Entity<Inh6_Assoc2_E10>();
            e10.ToTable("E10Set");
            e10.Property(x => x.dE10).IsRequired();

            var e11 = modelBuilder.Entity<Inh6_Assoc2_E11>();
            e11.ToTable("E11Set");
            e11.Property(x => x.dE11).IsRequired();

            var e20 = modelBuilder.Entity<Inh6_Assoc2_E20>();
            e20.ToTable("E20Set");
            e20.Property(x => x.dE20).IsRequired();

            var e21 = modelBuilder.Entity<Inh6_Assoc2_E21>();
            e21.ToTable("E21Set");
            e21.Property(x => x.dE21).IsRequired();

            var e22 = modelBuilder.Entity<Inh6_Assoc2_E22>();
            e22.ToTable("E22Set");
            e22.Property(x => x.dE22).IsRequired();

            var e23 = modelBuilder.Entity<Inh6_Assoc2_E23>();
            e23.ToTable("E23Set");
            e23.Property(x => x.dE23).IsRequired();

            var e30 = modelBuilder.Entity<Inh6_Assoc2_E30>();
            e30.ToTable("E30Set");
            e30.Property(x => x.dE30).IsRequired();

            var e31 = modelBuilder.Entity<Inh6_Assoc2_E31>();
            e31.ToTable("E31Set");
            e31.Property(x => x.dE31).IsRequired();

            var e32 = modelBuilder.Entity<Inh6_Assoc2_E32>();
            e32.ToTable("E32Set");
            e32.Property(x => x.dE32).IsRequired();

            var e33 = modelBuilder.Entity<Inh6_Assoc2_E33>();
            e33.ToTable("E33Set");
            e33.Property(x => x.dE33).IsRequired();

            var e34 = modelBuilder.Entity<Inh6_Assoc2_E34>();
            e34.ToTable("E34Set");
            e34.Property(x => x.dE34).IsRequired();

            var e35 = modelBuilder.Entity<Inh6_Assoc2_E35>();
            e35.ToTable("E35Set");
            e35.Property(x => x.dE35).IsRequired();

            var e36 = modelBuilder.Entity<Inh6_Assoc2_E36>();
            e36.ToTable("E36Set");
            e36.Property(x => x.dE36).IsRequired();

            var e37 = modelBuilder.Entity<Inh6_Assoc2_E37>();
            e37.ToTable("E37Set");
            e37.Property(x => x.dE37).IsRequired();

            var e40 = modelBuilder.Entity<Inh6_Assoc2_E40>();
            e40.ToTable("E40Set");
            e40.Property(x => x.dE40).IsRequired();

            var e41 = modelBuilder.Entity<Inh6_Assoc2_E41>();
            e41.ToTable("E41Set");
            e41.Property(x => x.dE41).IsRequired();

            var e42 = modelBuilder.Entity<Inh6_Assoc2_E42>();
            e42.ToTable("E42Set");
            e42.Property(x => x.dE42).IsRequired();

            var e43 = modelBuilder.Entity<Inh6_Assoc2_E43>();
            e43.ToTable("E43Set");
            e43.Property(x => x.dE43).IsRequired();

            var e44 = modelBuilder.Entity<Inh6_Assoc2_E44>();
            e44.ToTable("E44Set");
            e44.Property(x => x.dE44).IsRequired();

            var e45 = modelBuilder.Entity<Inh6_Assoc2_E45>();
            e45.ToTable("E45Set");
            e45.Property(x => x.dE45).IsRequired();

            var e46 = modelBuilder.Entity<Inh6_Assoc2_E46>();
            e46.ToTable("E46Set");
            e46.Property(x => x.dE46).IsRequired();

            var e47 = modelBuilder.Entity<Inh6_Assoc2_E47>();
            e47.ToTable("E47Set");
            e47.Property(x => x.dE47).IsRequired();

            var e48 = modelBuilder.Entity<Inh6_Assoc2_E48>();
            e48.ToTable("E48Set");
            e48.Property(x => x.dE48).IsRequired();

            var e49 = modelBuilder.Entity<Inh6_Assoc2_E49>();
            e49.ToTable("E49Set");
            e49.Property(x => x.dE49).IsRequired();

            var e410 = modelBuilder.Entity<Inh6_Assoc2_E410>();
            e410.ToTable("E410Set");
            e410.Property(x => x.dE410).IsRequired();

            var e411 = modelBuilder.Entity<Inh6_Assoc2_E411>();
            e411.ToTable("E411Set");
            e411.Property(x => x.dE411).IsRequired();

            var e412 = modelBuilder.Entity<Inh6_Assoc2_E412>();
            e412.ToTable("E412Set");
            e412.Property(x => x.dE412).IsRequired();

            var e413 = modelBuilder.Entity<Inh6_Assoc2_E413>();
            e413.ToTable("E413Set");
            e413.Property(x => x.dE413).IsRequired();

            var e414 = modelBuilder.Entity<Inh6_Assoc2_E414>();
            e414.ToTable("E414Set");
            e414.Property(x => x.dE414).IsRequired();

            var e415 = modelBuilder.Entity<Inh6_Assoc2_E415>();
            e415.ToTable("E415Set");
            e415.Property(x => x.dE415).IsRequired();

            var e50 = modelBuilder.Entity<Inh6_Assoc2_E50>();
            e50.ToTable("E50Set");
            e50.Property(x => x.dE50).IsRequired();

            var e51 = modelBuilder.Entity<Inh6_Assoc2_E51>();
            e51.ToTable("E51Set");
            e51.Property(x => x.dE51).IsRequired();

            var e52 = modelBuilder.Entity<Inh6_Assoc2_E52>();
            e52.ToTable("E52Set");
            e52.Property(x => x.dE52).IsRequired();

            var e53 = modelBuilder.Entity<Inh6_Assoc2_E53>();
            e53.ToTable("E53Set");
            e53.Property(x => x.dE53).IsRequired();

            var e54 = modelBuilder.Entity<Inh6_Assoc2_E54>();
            e54.ToTable("E54Set");
            e54.Property(x => x.dE54).IsRequired();

            var e55 = modelBuilder.Entity<Inh6_Assoc2_E55>();
            e55.ToTable("E55Set");
            e55.Property(x => x.dE55).IsRequired();

            var e56 = modelBuilder.Entity<Inh6_Assoc2_E56>();
            e56.ToTable("E56Set");
            e56.Property(x => x.dE56).IsRequired();

            var e57 = modelBuilder.Entity<Inh6_Assoc2_E57>();
            e57.ToTable("E57Set");
            e57.Property(x => x.dE57).IsRequired();

            var e58 = modelBuilder.Entity<Inh6_Assoc2_E58>();
            e58.ToTable("E58Set");
            e58.Property(x => x.dE58).IsRequired();

            var e59 = modelBuilder.Entity<Inh6_Assoc2_E59>();
            e59.ToTable("E59Set");
            e59.Property(x => x.dE59).IsRequired();

            var e510 = modelBuilder.Entity<Inh6_Assoc2_E510>();
            e510.ToTable("E510Set");
            e510.Property(x => x.dE510).IsRequired();

            var e511 = modelBuilder.Entity<Inh6_Assoc2_E511>();
            e511.ToTable("E511Set");
            e511.Property(x => x.dE511).IsRequired();

            var e512 = modelBuilder.Entity<Inh6_Assoc2_E512>();
            e512.ToTable("E512Set");
            e512.Property(x => x.dE512).IsRequired();

            var e513 = modelBuilder.Entity<Inh6_Assoc2_E513>();
            e513.ToTable("E513Set");
            e513.Property(x => x.dE513).IsRequired();

            var e514 = modelBuilder.Entity<Inh6_Assoc2_E514>();
            e514.ToTable("E514Set");
            e514.Property(x => x.dE514).IsRequired();

            var e515 = modelBuilder.Entity<Inh6_Assoc2_E515>();
            e515.ToTable("E515Set");
            e515.Property(x => x.dE515).IsRequired();

            var e516 = modelBuilder.Entity<Inh6_Assoc2_E516>();
            e516.ToTable("E516Set");
            e516.Property(x => x.dE516).IsRequired();

            var e517 = modelBuilder.Entity<Inh6_Assoc2_E517>();
            e517.ToTable("E517Set");
            e517.Property(x => x.dE517).IsRequired();

            var e518 = modelBuilder.Entity<Inh6_Assoc2_E518>();
            e518.ToTable("E518Set");
            e518.Property(x => x.dE518).IsRequired();

            var e519 = modelBuilder.Entity<Inh6_Assoc2_E519>();
            e519.ToTable("E519Set");
            e519.Property(x => x.dE519).IsRequired();

            var e520 = modelBuilder.Entity<Inh6_Assoc2_E520>();
            e520.ToTable("E520Set");
            e520.Property(x => x.dE520).IsRequired();

            var e521 = modelBuilder.Entity<Inh6_Assoc2_E521>();
            e521.ToTable("E521Set");
            e521.Property(x => x.dE521).IsRequired();

            var e522 = modelBuilder.Entity<Inh6_Assoc2_E522>();
            e522.ToTable("E522Set");
            e522.Property(x => x.dE522).IsRequired();

            var e523 = modelBuilder.Entity<Inh6_Assoc2_E523>();
            e523.ToTable("E523Set");
            e523.Property(x => x.dE523).IsRequired();

            var e524 = modelBuilder.Entity<Inh6_Assoc2_E524>();
            e524.ToTable("E524Set");
            e524.Property(x => x.dE524).IsRequired();

            var e525 = modelBuilder.Entity<Inh6_Assoc2_E525>();
            e525.ToTable("E525Set");
            e525.Property(x => x.dE525).IsRequired();

            var e526 = modelBuilder.Entity<Inh6_Assoc2_E526>();
            e526.ToTable("E526Set");
            e526.Property(x => x.dE526).IsRequired();

            var e527 = modelBuilder.Entity<Inh6_Assoc2_E527>();
            e527.ToTable("E527Set");
            e527.Property(x => x.dE527).IsRequired();

            var e528 = modelBuilder.Entity<Inh6_Assoc2_E528>();
            e528.ToTable("E528Set");
            e528.Property(x => x.dE528).IsRequired();

            var e529 = modelBuilder.Entity<Inh6_Assoc2_E529>();
            e529.ToTable("E529Set");
            e529.Property(x => x.dE529).IsRequired();

            var e530 = modelBuilder.Entity<Inh6_Assoc2_E530>();
            e530.ToTable("E530Set");
            e530.Property(x => x.dE530).IsRequired();

            var e531 = modelBuilder.Entity<Inh6_Assoc2_E531>();
            e531.ToTable("E531Set");
            e531.Property(x => x.dE531).IsRequired();

            var a00 = modelBuilder.Entity<Inh6_Assoc2_A00>();
            a00.ToTable("A00Set");
            a00.Property(x => x.dA00).IsRequired();

            var a10 = modelBuilder.Entity<Inh6_Assoc2_A10>();
            a10.ToTable("A10Set");
            a10.Property(x => x.dA10).IsRequired();
            a10.HasMany(x => x.B00s).WithRequired(x => x.A10);

            var a11 = modelBuilder.Entity<Inh6_Assoc2_A11>();
            a11.ToTable("A11Set");
            a11.Property(x => x.dA11).IsRequired();
            a11.HasMany(x => x.C00s).WithRequired(x => x.A11);

            var a12 = modelBuilder.Entity<Inh6_Assoc2_A12>();
            a12.ToTable("A12Set");
            a12.Property(x => x.dA12).IsRequired();

            var b00 = modelBuilder.Entity<Inh6_Assoc2_B00>();
            b00.Property(x => x.dB00).IsRequired();

            var c00 = modelBuilder.Entity<Inh6_Assoc2_C00>();
            c00.Property(x => x.dC00).IsRequired();
        }
    }
}
