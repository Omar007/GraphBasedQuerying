using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class EqualExpr : BinaryExpr
    {
        public UnaryExpr Lhs { get; private set; }
        public UnaryExpr Rhs { get; private set; }

        public EqualExpr(UnaryExpr lhs, UnaryExpr rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            Lhs.AppendToStringBuilder(sb);
            sb.Append("=");
            Rhs.AppendToStringBuilder(sb);
        }
    }
}
