
namespace EntityGraph4EF6.SQL
{
    internal class InnerJoin : Join
    {
        public InnerJoin(Table table, Table joinTable, Expr onExpression)
            : base(table, joinTable, onExpression, "INNER")
        {

        }
    }
}
