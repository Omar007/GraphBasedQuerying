using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class When : Expr
    {
        public Expr Expression { get; private set; }
        public ConstantExpr WhenTrue { get; private set; }

        public When(Expr expression, ConstantExpr whenTrue)
        {
            Expression = expression;
            WhenTrue = whenTrue;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            sb.Append("WHEN ");
            Expression.AppendToStringBuilder(sb);
            sb.Append(" THEN ");
            WhenTrue.AppendToStringBuilder(sb);
        }
    }
}
