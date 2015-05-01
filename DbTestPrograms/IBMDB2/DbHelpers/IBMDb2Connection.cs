using System;
using DbTest.Core;
using IBM.Data.DB2;

namespace IBMDB2.DbHelpers
{
    public class IBMDb2Connection : IDatabaseConnection
    {
        public string Name { get { return "IBM DB2"; } }

        public IBMDb2Connection()
        {

        }

        public string CreateConnectionString(string modelName, Population population)
        {
            var db2Builder = new DB2ConnectionStringBuilder()
            {
                Server = "127.0.0.1:50000",
                UserID = "root",
                Password = "kikker",
                Database = modelName + (population != null ? "." + population.ToString() : String.Empty),
                PersistSecurityInfo = true
            };
            return db2Builder.ConnectionString;
        }
    }
}
