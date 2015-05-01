using System;
using System.Linq.Expressions;
using EntityGraph4EF6.Mapping;
using EntityGraph4EF6.SQL;

namespace EntityGraph4EF6
{
    internal class LoadWhereExpr<TEntity>
        where TEntity : class
    {
        private readonly TypeMapping _typeMapping;
        private readonly Expression<Func<TEntity, bool>> _expr;

        public WhereExpr Condition { get; private set; }

        public LoadWhereExpr(TypeMapping typeMapping, Expression<Func<TEntity, bool>> expr)
        {
            _typeMapping = typeMapping;
            _expr = expr;

            var visitor = new Expression2SqlVisitor(typeMapping);
            visitor.Visit(expr);
            Condition = new WhereExpr(visitor.SqlExpression);
        }
    }
}
