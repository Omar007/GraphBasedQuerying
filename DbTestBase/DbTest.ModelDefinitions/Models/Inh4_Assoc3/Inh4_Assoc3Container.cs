﻿using System.Data.Entity;

namespace DbTest.ModelDefinitions.Models.Inh4_Assoc3
{
    public class Inh4_Assoc3Container : DbContext
    {
        public Inh4_Assoc3Container()
            : base("name=Entities")
        {

        }

        public Inh4_Assoc3Container(string connectionString)
            : base(connectionString)
        {

        }

        public virtual DbSet<Inh4_Assoc3_O> OSet { get; set; }
        public virtual DbSet<Inh4_Assoc3_E00> E00Set { get; set; }
        public virtual DbSet<Inh4_Assoc3_A00> A00Set { get; set; }
        public virtual DbSet<Inh4_Assoc3_B00> B00Set { get; set; }
        public virtual DbSet<Inh4_Assoc3_C00> C00Set { get; set; }
        public virtual DbSet<Inh4_Assoc3_D00> D00Set { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var o = modelBuilder.Entity<Inh4_Assoc3_O>();
            o.HasMany(x => x.E00s).WithRequired(x => x.O);
            o.Property(x => x.dO).IsRequired();

            var e00 = modelBuilder.Entity<Inh4_Assoc3_E00>();
            e00.ToTable("E00Set");
            e00.Property(x => x.dE00).IsRequired();
            e00.HasMany(x => x.A00s).WithOptional(x => x.E00);

            var e10 = modelBuilder.Entity<Inh4_Assoc3_E10>();
            e10.ToTable("E10Set");
            e10.Property(x => x.dE10).IsRequired();

            var e11 = modelBuilder.Entity<Inh4_Assoc3_E11>();
            e11.ToTable("E11Set");
            e11.Property(x => x.dE11).IsRequired();

            var e20 = modelBuilder.Entity<Inh4_Assoc3_E20>();
            e20.ToTable("E20Set");
            e20.Property(x => x.dE20).IsRequired();

            var e21 = modelBuilder.Entity<Inh4_Assoc3_E21>();
            e21.ToTable("E21Set");
            e21.Property(x => x.dE21).IsRequired();

            var e22 = modelBuilder.Entity<Inh4_Assoc3_E22>();
            e22.ToTable("E22Set");
            e22.Property(x => x.dE22).IsRequired();

            var e23 = modelBuilder.Entity<Inh4_Assoc3_E23>();
            e23.ToTable("E23Set");
            e23.Property(x => x.dE23).IsRequired();

            var e30 = modelBuilder.Entity<Inh4_Assoc3_E30>();
            e30.ToTable("E30Set");
            e30.Property(x => x.dE30).IsRequired();

            var e31 = modelBuilder.Entity<Inh4_Assoc3_E31>();
            e31.ToTable("E31Set");
            e31.Property(x => x.dE31).IsRequired();

            var e32 = modelBuilder.Entity<Inh4_Assoc3_E32>();
            e32.ToTable("E32Set");
            e32.Property(x => x.dE32).IsRequired();

            var e33 = modelBuilder.Entity<Inh4_Assoc3_E33>();
            e33.ToTable("E33Set");
            e33.Property(x => x.dE33).IsRequired();

            var e34 = modelBuilder.Entity<Inh4_Assoc3_E34>();
            e34.ToTable("E34Set");
            e34.Property(x => x.dE34).IsRequired();

            var e35 = modelBuilder.Entity<Inh4_Assoc3_E35>();
            e35.ToTable("E35Set");
            e35.Property(x => x.dE35).IsRequired();

            var e36 = modelBuilder.Entity<Inh4_Assoc3_E36>();
            e36.ToTable("E36Set");
            e36.Property(x => x.dE36).IsRequired();

            var e37 = modelBuilder.Entity<Inh4_Assoc3_E37>();
            e37.ToTable("E37Set");
            e37.Property(x => x.dE37).IsRequired();

            var a00 = modelBuilder.Entity<Inh4_Assoc3_A00>();
            a00.ToTable("A00Set");
            a00.Property(x => x.dA00).IsRequired();

            var a10 = modelBuilder.Entity<Inh4_Assoc3_A10>();
            a10.ToTable("A10Set");
            a10.Property(x => x.dA10).IsRequired();
            a10.HasMany(x => x.B00s).WithRequired(x => x.A10);

            var a11 = modelBuilder.Entity<Inh4_Assoc3_A11>();
            a11.ToTable("A11Set");
            a11.Property(x => x.dA11).IsRequired();
            a11.HasMany(x => x.C00s).WithRequired(x => x.A11);

            var a12 = modelBuilder.Entity<Inh4_Assoc3_A12>();
            a12.ToTable("A12Set");
            a12.Property(x => x.dA12).IsRequired();
            a12.HasMany(x => x.D00s).WithRequired(x => x.A12);

            var b00 = modelBuilder.Entity<Inh4_Assoc3_B00>();
            b00.Property(x => x.dB00).IsRequired();

            var c00 = modelBuilder.Entity<Inh4_Assoc3_C00>();
            c00.Property(x => x.dC00).IsRequired();

            var d00 = modelBuilder.Entity<Inh4_Assoc3_D00>();
            d00.Property(x => x.dD00).IsRequired();
        }
    }
}
