using System.Collections.Generic;
using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class Case : ColumnSelect
    {
        public IEnumerable<When> Whens { get; private set; }

        public Case(string asName, IEnumerable<When> whens)
            : base(null, null, asName)
        {
            Whens = whens;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            sb.AppendLine("CASE");
            foreach(var when in Whens)
            {
                sb.Append("   ");
                when.AppendToStringBuilder(sb);
                sb.AppendLine();
            }
            sb.AppendLine("END");
            sb.AppendFormat("AS \"{0}\"", AsName);
        }
    }
}
