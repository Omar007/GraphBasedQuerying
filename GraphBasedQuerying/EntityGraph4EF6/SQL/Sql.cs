using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal abstract class Sql
    {
        public abstract void AppendToStringBuilder(StringBuilder sb);

        public override string ToString()
        {
            var sb = new StringBuilder();
            AppendToStringBuilder(sb);
            return sb.ToString();
        }
    }
}
