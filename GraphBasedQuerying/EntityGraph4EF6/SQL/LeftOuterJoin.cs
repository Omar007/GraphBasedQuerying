
namespace EntityGraph4EF6.SQL
{
    internal class LeftOuterJoin : Join
    {
        public LeftOuterJoin(Table table, Table joinTable, Expr onExpression)
            : base(table, joinTable, onExpression, "LEFT OUTER")
        {

        }
    }
}
