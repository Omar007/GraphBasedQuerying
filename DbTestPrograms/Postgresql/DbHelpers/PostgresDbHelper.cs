using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DbTest.Core;
using Npgsql;

namespace Postgresql.DbHelpers
{
    public class PostgresDbHelper
    {
        private readonly NpgsqlConnectionStringBuilder _conStringBuilder;

        public PostgresDbHelper()
        {
            _conStringBuilder = new NpgsqlConnectionStringBuilder()
            {
                Host = "127.0.0.1",
                Port = 5432,
                UserName = "postgres",
                Password = "kikker",
                Database = "postgres"
            };
        }

        public void CreateDbs(IDatabaseConnection connection, IEnumerable<Population> populations)
        {
            var assembly = Assembly.GetExecutingAssembly();

            ExecuteSqlScript(assembly.GetManifestResourceStream("Postgresql.SqlScripts.CreateAll.sql"));
        }

        public void DropDbs(IDatabaseConnection connection, IEnumerable<Population> populations)
        {
            var assembly = Assembly.GetExecutingAssembly();

            ExecuteSqlScript(assembly.GetManifestResourceStream("Postgresql.SqlScripts.DropAll.sql"));
        }

        private void ExecuteSqlScript(Stream s)
        {
            using (var sr = new StreamReader(s))
            {
                using (var con = new NpgsqlConnection(_conStringBuilder))
                {
                    con.Open();
                    while (!sr.EndOfStream)
                    {
                        try
                        {
                            var cmd = con.CreateCommand();
                            cmd.CommandText = sr.ReadLine();
                            cmd.ExecuteNonQuery();
                        }
                        catch { }
                    }
                    con.Close();
                }
            }
        }
    }
}
