using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EntityFramework.Reflection;

namespace Graph4EF
{
    internal class GraphPath
    {
        public class PathNode
        {
            public Expression InEdge { get; private set; }
            public Expression OutEdge { get; private set; }
            public Type Type { get; private set; }
            public PathNode Next { get; private set; }

            internal PathNode(Expression outEdge, Type type)
                : this(outEdge, type, null)
            {

            }

            internal PathNode(Expression outEdge, Type type, PathNode next)
            {
                OutEdge = outEdge;
                Type = type;
                Next = next;

                if (Next != null)
                {
                    Next.InEdge = OutEdge;
                }
            }
        }

        public bool IsInheritancePath { get; set; }
        public PathNode First { get; private set; }
        public PathNode Last { get; private set; }

        public bool IsLoaded { get; private set; }
        public ObjectQuery ObjectQuery { get; private set; }
        public IEnumerable Result { get; private set; }

        public GraphPath()
        {

        }

        public GraphPath(Type from, Type to, Expression toExpr)
        {
            Last = new PathNode(null, to);
            First = new PathNode(toExpr, from, Last);
        }

        public void Add(Expression outEdge, Type type)
        {
            if (Last == null)
            {
                Last = First;
            }
            First = new PathNode(outEdge, type, First);
        }

        public bool IsSubPath(GraphPath other)
        {
            if (Object.ReferenceEquals(this, other))
            {
                return false;
            }

            //TODO: Check inheritance
            if (IsInheritancePath || other.IsInheritancePath)
            {
                return false;
            }

            for (PathNode ownPath = First, otherPath = other.First;
                ownPath != null && otherPath != null;
                ownPath = ownPath.Next, otherPath = otherPath.Next)
            {
                if (ownPath.OutEdge != otherPath.OutEdge)
                {
                    if ((ownPath.Next == null && otherPath.Next == null)
                        || !(otherPath.Next == null && otherPath.OutEdge == null))
                    {
                        return false;
                    }
                }
                if (ownPath.InEdge != otherPath.InEdge
                    || ownPath.Type != otherPath.Type)
                {
                    return false;
                }
            }

            return true;
        }

        //public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector);
        //public static IQueryable<TResult> SelectMany<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector);
        //public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector);
        //public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector);

        public void BuildQuery<T>(IQueryable<T> baseQuery)
        {
            if (IsInheritancePath)
            {
                ObjectQuery = BuildInheritanceQuery(baseQuery);
            }
            else
            {
                ObjectQuery = BuildIncludeQuery(baseQuery);
            }
        }

        private ObjectQuery BuildInheritanceQuery<T>(IQueryable<T> baseQuery)
        {
            var ofTypeMethod = typeof(Queryable).GetMethod("OfType");

            dynamic sequenceQuery = baseQuery;

            for (var current = First; current.Next != null; current = current.Next)
            {
                if (current.OutEdge == null)
                {
                    var mInfo = ofTypeMethod.MakeGenericMethod(current.Next.Type);
                    sequenceQuery = mInfo.Invoke(null, new[] { sequenceQuery });
                }
                else if (typeof(IEnumerable).IsAssignableFrom(((dynamic)current.OutEdge).ReturnType))
                {
                    sequenceQuery = Queryable.SelectMany(sequenceQuery, (dynamic)current.OutEdge);
                }
                else
                {
                    sequenceQuery = Queryable.Select(sequenceQuery, (dynamic)current.OutEdge);
                }
            }

            return IQueryableExtensions.ToObjectQuery(sequenceQuery);
        }

        private ObjectQuery BuildIncludeQuery<T>(IQueryable<T> baseQuery)
        {
            Func<IEnumerable<object>, Func<object, object>, IEnumerable<object>> selectMethod = Enumerable.Select;
            var selectMethodInfo = selectMethod.GetMethodInfo().GetGenericMethodDefinition();

            dynamic finalExpr = First.OutEdge;

            for (var current = First.Next; current.Next != null; current = current.Next)
            {
                var next = current.Next;

                var mInfo = selectMethodInfo.MakeGenericMethod(current.Type, ((dynamic)current.OutEdge).ReturnType);

                var selectCallExpr = Expression.Call(mInfo, finalExpr.Body, current.OutEdge);
                finalExpr = Expression.Lambda(selectCallExpr, finalExpr.Parameters);
            }

            return IQueryableExtensions.ToObjectQuery(QueryableExtensions.Include(baseQuery, finalExpr));
        }

        public void SetResult(DbContext context, DbDataReader reader)
        {
            IsLoaded = true;

            dynamic queryProxy = new DynamicProxy(ObjectQuery);
            dynamic queryState = queryProxy.QueryState;
            dynamic executionPlan = queryState.GetExecutionPlan(null);
            dynamic shaperFactory = executionPlan.ResultShaperFactory;
            var objContext = context.GetObjectContext();
            dynamic shaper = shaperFactory.Create(reader, objContext, objContext.MetadataWorkspace, MergeOption.AppendOnly, false, true, false);

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(IsInheritancePath ? Last.Type : First.Type));
            IEnumerator enumerator = shaper.GetEnumerator();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }

            Result = list;
        }
    }
}
