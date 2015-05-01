using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class IsNotNull : UnaryExpr
    {
        public ColumnExpr ColumnExpression { get; private set; }

        public IsNotNull(ColumnExpr columnExpression)
        {
            ColumnExpression = columnExpression;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            ColumnExpression.AppendToStringBuilder(sb);
            sb.Append(" IS NOT NULL");
        }
    }
}
