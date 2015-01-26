using System;
using System.Data.SqlClient;
using DbTest.Core;

namespace SqlServer.DbHelpers
{
    public class LocalDbConnection : IDatabaseConnection
    {
        public string Name { get { return "LocalDb V12"; } }

        public LocalDbConnection()
        {

        }

        public string CreateConnectionString(string modelName, Population population)
        {
            var sqlBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\ProjectsV12",
                InitialCatalog = modelName + (population != null ? "." + population.ToString() : String.Empty),
                IntegratedSecurity = true,
                MultipleActiveResultSets = true
            };
            return sqlBuilder.ConnectionString;
        }
    }
}
