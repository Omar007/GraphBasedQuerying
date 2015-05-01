using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DbTest.Core
{
    internal class DbGenerator
    {
        private readonly IDatabaseConnection _connection;
        private readonly IEnumerable<Population> _populations;

        private readonly IEnumerable<IGenerator> _generators;

        public DbGenerator(IDatabaseConnection connection, IEnumerable<Population> populations)
        {
            _connection = connection;
            _populations = populations;

            _generators = GetGenerators();
        }

        private IEnumerable<IGenerator> GetGenerators()
        {
            var generatorTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(IGenerator).IsAssignableFrom(t));

            foreach (var generatorType in generatorTypes)
            {
                yield return (IGenerator)Activator.CreateInstance(generatorType);
            }
        }

        public void CreateDatabases(bool recreateExisting)
        {
            Console.WriteLine("Generating dbs for connection '{0}'...", _connection.Name);
            foreach (var generator in _generators)
            {
                generator.CreateDatabases(_connection, _populations, recreateExisting);
            }
        }

        public void DeleteDatabases()
        {
            Console.WriteLine("Deleting dbs for connection '{0}'...", _connection.Name);
            foreach (var generator in _generators)
            {
                generator.DeleteDatabases(_connection, _populations);
            }
        }
    }
}
