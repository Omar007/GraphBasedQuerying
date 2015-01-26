using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class Table : Sql
    {
        public string Name { get; private set; }
        public string AsName { get; private set; }

        public Table(string name)
            : this(name, name)
        {

        }

        public Table(string name, string asName)
        {
            Name = name;
            AsName = asName;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            sb.AppendFormat("\"{0}\" AS \"{1}\"", Name, AsName);
        }
    }
}
