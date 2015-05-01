using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class Column : Sql
    {
        public string Table { get; private set; }
        public string Name { get; private set; }

        public Column(string table, string name)
        {
            Table = table;
            Name = name;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            sb.AppendFormat("\"{0}\".\"{1}\"", Table, Name);
        }
    }
}
