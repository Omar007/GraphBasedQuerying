using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityGraph
{
    public class GraphShape<TEntity> : IGraphShape<TEntity>, IEnumerable<Edge>
        where TEntity : class
    {
        private readonly ICollection<Edge> _edges;

        internal GraphShape(ICollection<Edge> edges)
        {
            _edges = edges;
        }

        public GraphShape()
            : this(new List<Edge>())
        {

        }

        public IEnumerable<PropertyInfo> OutEdges(TEntity entity)
        {
            if (entity == null)
            {
                return Enumerable.Empty<PropertyInfo>();
            }
            return OutEdges(entity.GetType());
        }

        public IEnumerable<PropertyInfo> OutEdges(Type entityType)
        {
            return _edges.Where(edge => edge.FromType.IsAssignableFrom(entityType))
                .Select(edge => edge.EdgeInfo).Distinct();
        }

        public bool IsEdge(PropertyInfo edge)
        {
            return _edges.Any(e => e.EdgeInfo == edge);
        }

        public virtual TEntity GetNode(TEntity entity, PropertyInfo edge)
        {
            return (TEntity)edge.GetValue(entity, null);
        }

        public virtual IEnumerable<TEntity> GetNodes(TEntity entity, PropertyInfo edge)
        {
            var node = edge.GetValue(entity, null);
            if (node == null)
            {
                return Enumerable.Empty<TEntity>();
            }
            var nodes = node as IEnumerable<TEntity>;
            return nodes ?? new List<TEntity> { (TEntity)node };
        }

        public GraphShape<TEntity> Edge<TFrom>(Expression<Func<TFrom, TEntity>> edge)
            where TFrom : TEntity
        {
            return Edge<TFrom, Func<TFrom, TEntity>>(edge, edge.Body);
        }

        public GraphShape<TEntity> Edge<TFrom>(Expression<Func<TFrom, IEnumerable<TEntity>>> edge)
            where TFrom : TEntity
        {
            var expression = edge.Body as UnaryExpression;
            if (expression != null)
            {
                if (edge.Body.NodeType != ExpressionType.Convert)
                {
                    var msg = String.Format(
                        "Edge expression '{0}' is invalid: the lambda expression has an unsupported format.", edge);
                    throw new Exception(msg);
                }

                return Edge<TFrom, Func<TFrom, IEnumerable<TEntity>>>(edge, expression.Operand);
            }

            return Edge<TFrom, Func<TFrom, IEnumerable<TEntity>>>(edge, edge.Body);
        }

        private GraphShape<TEntity> Edge<TFrom, T>(Expression<T> edge, Expression body)
            where TFrom : TEntity
        {
            Debug.Assert(typeof(TFrom) == edge.Parameters.Single().Type);

            var mExpr = body as MemberExpression;
            if (mExpr == null || !(mExpr.Expression is ParameterExpression))
            {
                var msg = String.Format("Edge expression '{0}' is invalid: it should have the form 'A => A.B'", edge);
                throw new Exception(msg);
            }

            var propInfo = mExpr.Member as PropertyInfo;
            if (propInfo != null)
            {
                _edges.Add(new Edge<TFrom>(propInfo));
            }

            return this;
        }

        public IEnumerator<Edge> GetEnumerator()
        {
            return _edges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
