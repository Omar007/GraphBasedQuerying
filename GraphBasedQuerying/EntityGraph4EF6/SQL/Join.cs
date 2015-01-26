using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal abstract class Join : Table
    {
        public Table Table { get; private set; }
        public Table JoinTable { get; private set; }
        public Expr OnExpression { get; private set; }

        private readonly string _joinString;

        protected Join(Table table, Table joinTable, Expr onExpression, string joinString)
            : base(null, null)
        {
            Table = table;
            JoinTable = joinTable;
            OnExpression = onExpression;

            _joinString = joinString;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            Table.AppendToStringBuilder(sb);
            sb.AppendLine();
            sb.Append(_joinString);
            sb.Append(" JOIN ");
            JoinTable.AppendToStringBuilder(sb);
            sb.Append(" ON ");
            OnExpression.AppendToStringBuilder(sb);
        }
    }
}
