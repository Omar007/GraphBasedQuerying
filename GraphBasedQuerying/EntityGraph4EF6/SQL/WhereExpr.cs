using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class WhereExpr : UnaryExpr
    {
        public Expr Expression { get; private set; }

        public WhereExpr(Expr expression)
        {
            Expression = expression;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            sb.Append("WHERE ");
            Expression.AppendToStringBuilder(sb);
        }
    }
}
