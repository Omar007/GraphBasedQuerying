using System.Collections.Generic;

namespace DbTest.Core
{
    public interface IGenerator
    {
        void CreateDatabases(IDatabaseConnection connection, IEnumerable<Population> populations, bool recreateExisting);
        void PopulateDatabase(IDatabaseConnection connection, Population population);
        void DeleteDatabases(IDatabaseConnection connection, IEnumerable<Population> populations);
    }
}
