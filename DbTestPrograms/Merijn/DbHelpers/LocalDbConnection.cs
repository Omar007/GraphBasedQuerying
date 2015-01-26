using System;
using System.Data.EntityClient;
using System.Data.SqlClient;
using DbTest.Core;

namespace Merijn.DbHelpers
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
                InitialCatalog = modelName + "." + population.ECount,
                IntegratedSecurity = true,
                MultipleActiveResultSets = true
            };

            var entityBuilder = new EntityConnectionStringBuilder()
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = sqlBuilder.ConnectionString,
                Metadata = String.Format(@"res://*/Models.{0}Model.csdl|res://*/Models.{0}Model.ssdl|res://*/Models.{0}Model.msl", modelName)
            };
            return entityBuilder.ConnectionString;
        }
    }
}
