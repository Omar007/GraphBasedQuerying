using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EntityGraph4EF6.Mapping;
using EntityGraph4EF6.SQL;

namespace EntityGraph4EF6
{
    internal class Expression2SqlVisitor : ExpressionVisitor
    {
        public Expr SqlExpression
        {
            get
            {
                Debug.Assert(_sqlExprStack.Count == 1);
                return _sqlExprStack.Peek();
            }
        }

        private readonly Stack<Expr> _sqlExprStack;
        private readonly TypeMapping _typeMapping;

        public Expression2SqlVisitor(TypeMapping typeMapping)
        {
            _typeMapping = typeMapping;
            _sqlExprStack = new Stack<Expr>();
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            var testValue = (bool)Expression.Lambda(node.Test).Compile().DynamicInvoke();
            return testValue ? node.IfTrue : node.IfFalse;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            //var returnValue = base.VisitBinary(node);
            Expression returnValue = null;
            
            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(new AndExpr(_sqlExprStack.Pop(), _sqlExprStack.Pop()));
                    break;
                case ExpressionType.OrElse:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(new OrExpr(_sqlExprStack.Pop(), _sqlExprStack.Pop()));
                    break;
                case ExpressionType.Equal:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(new EqualExpr((UnaryExpr)_sqlExprStack.Pop(), (UnaryExpr)_sqlExprStack.Pop()));
                    break;
                case ExpressionType.NotEqual:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(new NotEqualExpr((UnaryExpr)_sqlExprStack.Pop(), (UnaryExpr)_sqlExprStack.Pop()));
                    break;
                case ExpressionType.GreaterThan:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(new GreaterThenExpr((UnaryExpr)_sqlExprStack.Pop(), (UnaryExpr)_sqlExprStack.Pop()));
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(new GreaterThenEqualExpr((UnaryExpr)_sqlExprStack.Pop(), (UnaryExpr)_sqlExprStack.Pop()));
                    break;
                case ExpressionType.LessThan:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(new LessThenExpr((UnaryExpr)_sqlExprStack.Pop(), (UnaryExpr)_sqlExprStack.Pop()));
                    break;
                case ExpressionType.LessThanOrEqual:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(new LessThenEqualExpr((UnaryExpr)_sqlExprStack.Pop(), (UnaryExpr)_sqlExprStack.Pop()));
                    break;

                default: //TODO: Verify this default case.
                    returnValue = node;
                    var value = Expression.Lambda(node).Compile().DynamicInvoke();
                    _sqlExprStack.Push(new ConstantExpr(value));
                    break;
            }

            return returnValue;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var value = Expression.Lambda(node).Compile().DynamicInvoke();
            _sqlExprStack.Push(new ConstantExpr(value));

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var value = Expression.Lambda(node).Compile().DynamicInvoke();
            _sqlExprStack.Push(new ConstantExpr(value));

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ParameterExpression)
            {
                var propInfo = node.Member as PropertyInfo;
                var tableMapping = _typeMapping.TableMappings.First(tm => tm.PropertyMappings.Any(pm => pm.Property == propInfo));
                var propMapping = tableMapping.PropertyMappings.Single(pm => pm.Property == propInfo);

                _sqlExprStack.Push(new ColumnExpr(new Column(tableMapping.TableName, propMapping.ColumnName)));
            }
            else
            {
                var value = Expression.Lambda(node).Compile().DynamicInvoke();
                _sqlExprStack.Push(new ConstantExpr(value));
            }

            return node;
        }
    }
}
