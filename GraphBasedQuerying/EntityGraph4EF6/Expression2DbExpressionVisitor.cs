using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EntityGraph4EF6.Mapping;

namespace EntityGraph4EF6
{
    internal class Expression2DbExpressionVisitor : ExpressionVisitor
    {
        public DbExpression DbExpression
        {
            get
            {
                Debug.Assert(_sqlExprStack.Count == 1);
                return _sqlExprStack.Peek();
            }
        }

        private readonly Stack<DbExpression> _sqlExprStack;
        private readonly TypeMapping _typeMapping;
        private readonly DbVariableReferenceExpression _tableVar;

        public Expression2DbExpressionVisitor(TypeMapping typeMapping, DbVariableReferenceExpression tableVar)
        {
            _typeMapping = typeMapping;
            _tableVar = tableVar;

            _sqlExprStack = new Stack<DbExpression>();
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
                    _sqlExprStack.Push(DbExpressionBuilder.And(_sqlExprStack.Pop(), _sqlExprStack.Pop()));
                    break;
                case ExpressionType.OrElse:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(DbExpressionBuilder.Or(_sqlExprStack.Pop(), _sqlExprStack.Pop()));
                    break;
                case ExpressionType.Equal:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(DbExpressionBuilder.Equal(_sqlExprStack.Pop(), _sqlExprStack.Pop()));
                    break;
                case ExpressionType.NotEqual:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(DbExpressionBuilder.NotEqual(_sqlExprStack.Pop(), _sqlExprStack.Pop()));
                    break;
                case ExpressionType.GreaterThan:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(DbExpressionBuilder.GreaterThan(_sqlExprStack.Pop(), _sqlExprStack.Pop()));
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(DbExpressionBuilder.GreaterThanOrEqual(_sqlExprStack.Pop(), _sqlExprStack.Pop()));
                    break;
                case ExpressionType.LessThan:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(DbExpressionBuilder.LessThan(_sqlExprStack.Pop(), _sqlExprStack.Pop()));
                    break;
                case ExpressionType.LessThanOrEqual:
                    returnValue = base.VisitBinary(node);
                    _sqlExprStack.Push(DbExpressionBuilder.LessThanOrEqual(_sqlExprStack.Pop(), _sqlExprStack.Pop()));
                    break;

                default: //TODO: Verify this default case.
                    returnValue = node;
                    var value = Expression.Lambda(node).Compile().DynamicInvoke();
                    _sqlExprStack.Push(DbExpressionBuilder.Constant(value));
                    break;
            }

            return returnValue;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var value = Expression.Lambda(node).Compile().DynamicInvoke();
            _sqlExprStack.Push(DbExpressionBuilder.Constant(value));

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var value = Expression.Lambda(node).Compile().DynamicInvoke();
            _sqlExprStack.Push(DbExpressionBuilder.Constant(value));

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ParameterExpression)
            {
                var propInfo = node.Member as PropertyInfo;
                var tableMapping = _typeMapping.TableMappings.First(tm => tm.PropertyMappings.Any(pm => pm.Property == propInfo));
                var propMapping = tableMapping.PropertyMappings.Single(pm => pm.Property == propInfo);

                _sqlExprStack.Push(DbExpressionBuilder.Property(_tableVar, propMapping.ColumnProperty));
            }
            else
            {
                var value = Expression.Lambda(node).Compile().DynamicInvoke();
                _sqlExprStack.Push(DbExpressionBuilder.Constant(value));
            }

            return node;
        }
    }
}
