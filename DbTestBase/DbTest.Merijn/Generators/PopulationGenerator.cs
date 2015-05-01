using System;
using System.Collections.Generic;
using System.Data.Objects;
using DbTest.Core;

namespace DbTest.Merijn.Generators
{
    public abstract class PopulationGenerator<TContext> : IGenerator
        where TContext : ObjectContext, new()
    {
        public PopulationGenerator()
        {

        }

        protected TContext CreateContext(IDatabaseConnection connection, Population population)
        {
            var objContext = new TContext();
            objContext.Connection.ConnectionString = connection.CreateConnectionString(GetType().Name, population);
            objContext.ContextOptions.LazyLoadingEnabled = false;
            return objContext;
        }

        public void CreateDatabases(IDatabaseConnection connection, IEnumerable<Population> populations, bool recreateExisting)
        {
            foreach (var population in populations)
            {
                using (var db = CreateContext(connection, population))
                {
                    if (recreateExisting && db.DatabaseExists())
                    {
                        Console.WriteLine("Deleting existing db '{0}'...", db.Connection.Database);
                        db.DeleteDatabase();
                    }

                    if (!db.DatabaseExists())
                    {
                        Console.WriteLine("Creating db '{0}'...", db.Connection.Database);
                        db.CreateDatabase();
                        Console.WriteLine("Populating db '{0}'...", db.Connection.Database);
                        PopulateDatabase(connection, population);
                    }
                }
            }
        }

        public abstract void PopulateDatabase(IDatabaseConnection connection, Population population);

        public void DeleteDatabases(IDatabaseConnection connection, System.Collections.Generic.IEnumerable<Population> populations)
        {
            foreach (var population in populations)
            {
                using (var db = CreateContext(connection, population))
                {
                    Console.WriteLine("Deleting db '{0}'...", db.Connection.Database);
                    db.DeleteDatabase();
                }
            }
        }
    }
}
