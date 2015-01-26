using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class AndExpr : BinaryExpr
    {
        public Expr Lhs { get; private set; }
        public Expr Rhs { get; private set; }

        public AndExpr(Expr lhs, Expr rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            sb.Append('(');
            Lhs.AppendToStringBuilder(sb);
            sb.Append(" AND ");
            Rhs.AppendToStringBuilder(sb);
            sb.Append(')');
        }
    }
}
