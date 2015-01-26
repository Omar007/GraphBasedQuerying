using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class ColumnSelect : Column
    {
        public string AsName { get; private set; }

        public ColumnSelect(string table, string name, string asName)
            : base(table, name)
        {
            AsName = asName;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            base.AppendToStringBuilder(sb);
            sb.AppendFormat(" AS \"{0}\"", AsName);
        }
    }
}
