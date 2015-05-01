using System;
using DbTest.Core;
using Npgsql;

namespace Postgresql.DbHelpers
{
    public class PostgresqlConnection : IDatabaseConnection
    {
        public string Name { get { return "Postgresql"; } }

        public PostgresqlConnection()
        {

        }

        public string CreateConnectionString(string modelName, Population population)
        {
            var postgresBuilder = new NpgsqlConnectionStringBuilder()
            {
                Host = "127.0.0.1",
                Port = 5432,
                UserName = "postgres",
                Password = "kikker",
                Database = modelName + (population != null ? "." + population.ToString() : String.Empty)
            };
            return postgresBuilder.ConnectionString;
        }
    }
}
