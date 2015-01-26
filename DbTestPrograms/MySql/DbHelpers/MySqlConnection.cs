using System;
using DbTest.Core;
using MySql.Data.MySqlClient;

namespace MySql.DbHelpers
{
    public class MySqlConnection : IDatabaseConnection
    {
        public string Name { get { return "MySql"; } }

        public MySqlConnection()
        {

        }

        public string CreateConnectionString(string modelName, Population population)
        {
            var mySqlBuilder = new MySqlConnectionStringBuilder()
            {
                Server = "127.0.0.1",
                Port = 3306,
                UserID = "root",
                Password = "kikker",
                Database = modelName + (population != null ? "." + population.ToString() : String.Empty),
                IntegratedSecurity = true
            };
            return mySqlBuilder.ConnectionString;
        }
    }
}
