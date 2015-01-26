using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class ColumnExpr : UnaryExpr
    {
        public Column Column { get; private set; }

        public ColumnExpr(Column column)
        {
            Column = column;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            Column.AppendToStringBuilder(sb);
        }
    }
}
