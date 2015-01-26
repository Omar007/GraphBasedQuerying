using System;
using DbTest.Core;
using Oracle.ManagedDataAccess.Client;

namespace OracleDb.DbHelpers
{
    public class OracleDbConnection : IDatabaseConnection
    {
        public string Name { get { return "Oracle Db"; } }

        public OracleDbConnection()
        {

        }

        public string CreateConnectionString(string modelName, Population population)
        {
            var oracleBuilder = new OracleConnectionStringBuilder()
            {
                UserID = "root",
                Password = "kikker",
                DataSource = "OraDB12Home1\\" + modelName + (population != null ? "." + population.ToString() : String.Empty),
                PersistSecurityInfo = true
            };
            return oracleBuilder.ConnectionString;
        }
    }
}
