using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DbTest.Core;

namespace DbTest.ModelDefinitions.Generators
{
    public abstract class Generator : IGenerator
    {
        public Generator()
        {

        }

        public void CreateDatabases(IDatabaseConnection connection, IEnumerable<Population> populations, bool recreateExisting)
        {
            foreach (var population in populations)
            {
                var db = GetDatabase(connection, population);
                db.Log = Console.Write;

                if (recreateExisting && db.Exists())
                {
                    Console.WriteLine("Deleting existing db '{0}'...", db.Connection.Database);
                    db.Delete();
                }

                if (!db.Exists())
                {
                    Console.WriteLine("Creating db '{0}'...", db.Connection.Database);
                    db.Create();
                    Console.WriteLine("Populating db '{0}'...", db.Connection.Database);
                    PopulateDatabase(connection, population);
                }
            }
        }
        
        public abstract void PopulateDatabase(IDatabaseConnection connection, Population population);

        public void DeleteDatabases(IDatabaseConnection connection, IEnumerable<Population> populations)
        {
            foreach (var population in populations)
            {
                var db = GetDatabase(connection, population);
                Console.WriteLine("Deleting db '{0}'...", db.Connection.Database);
                db.Delete();
            }
        }

        protected Database GetDatabase(IDatabaseConnection connection, Population population)
        {
            return GetDbContext<DbContext>(connection, population).Database;
        }

        protected abstract T GetDbContext<T>(IDatabaseConnection connection, Population population)
            where T : DbContext;

        protected T Make<T>()
            where T : new()
        {
            return SetDataPropertyValues(new T());
        }

        private T SetDataPropertyValues<T>(T x)
        {
            foreach (var prop in x.GetType().GetProperties().Where(p => p.Name.StartsWith("d")))
            {
                prop.SetValue(x, GetValue());
            }

            return x;
        }

        private string GetValue()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
