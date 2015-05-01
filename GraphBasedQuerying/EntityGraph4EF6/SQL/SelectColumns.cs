using System.Collections.Generic;
using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class SelectColumns : Table
    {
        public Table Table { get; private set; }
        public WhereExpr Where { get; private set; }
        public IList<ColumnSelect> Columns { get; private set; }
        
        public SelectColumns(IList<ColumnSelect> columns, Table table, WhereExpr where)
            : base(null, null)
        {
            Table = table;
            Where = where;
            Columns = columns;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            sb.AppendLine("SELECT");
            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i].AppendToStringBuilder(sb);
                sb.AppendLine(i + 1 < Columns.Count ? "," : null);
            }
            if (Table != null)
            {
                sb.Append(" FROM ");
                Table.AppendToStringBuilder(sb);
            }
            if (Where != null)
            {
                sb.AppendLine();
                Where.AppendToStringBuilder(sb);
            }
            sb.AppendLine(";");
        }
    }
}
