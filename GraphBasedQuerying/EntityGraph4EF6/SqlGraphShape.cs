using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Linq.Expressions;
using EntityGraph;
using EntityGraph4EF6.Mapping;

namespace EntityGraph4EF6
{
    public class SqlGraphShape<TEntity> : GraphShape<TEntity>
        where TEntity : class
    {
        //TODO: Decouple WHERE from plan so we can reuse the whole query and only rebuild the WHERE.
        private IDictionary<Type, QueryPlan> _cache = new Dictionary<Type, QueryPlan>();

        private GraphPathCollection<TEntity> _graphPathCollection;
        private GraphPathCollection<TEntity> GraphPathCollection
        {
            get
            {
                if (_graphPathCollection == null)
                {
                    _graphPathCollection = new GraphPathCollection<TEntity>(this);
                }
                return _graphPathCollection;
            }
        }
        private readonly IEnumerable<TypeMapping> _typeMappings;
        
        internal readonly DbContext Context;

        public SqlGraphShape(DbContext context)
        {
            Context = context;

            _typeMappings = Mapper.GetTypeMappings(Context);
        }

        internal TypeMapping GetTypeMapping(Type type)
        {
            return _typeMappings.SingleOrDefault(m => m.Type == type);
        }

        //TODO: Check these. Path loading using stub entities has been removed.
        //public override TEntity GetNode(TEntity entity, PropertyInfo edge)
        //{
        //    var paths = GraphPathCollection.Where(p => p[0].OutEdge.EdgeInfo == edge).ToList();
        //    if (paths.Any())
        //    {
        //        LoadPaths(entity, paths);
        //    }
        //    return base.GetNode(entity, edge);
        //}

        //public override IEnumerable<TEntity> GetNodes(TEntity entity, PropertyInfo edge)
        //{
        //    var paths = GraphPathCollection.Where(p => p[0].OutEdge.EdgeInfo == edge).ToList();
        //    if (paths.Any())
        //    {
        //        LoadPaths(entity, paths);
        //    }
        //    return (IEnumerable<TEntity>)edge.GetValue(entity, null);
        //}

        public void Load<T>()
            where T : class, TEntity
        {
            var type = typeof(T);
            if (!_cache.ContainsKey(type))
            {
                var paths = GraphPathCollection.Where(p => p.IsMatchingForType(type)).ToList();
                LoadPaths<T>(paths);
            }

            Context.ExecuteQueryPlan(_cache[type]);
        }

        private void LoadPaths<T>(IEnumerable<GraphPath> paths)
            where T : class, TEntity
        {
            var typeMapping = GetTypeMapping(typeof(T));
            var queryData = new Dictionary<DbExpression, TypeMapping>();
            queryData.Add(typeMapping.ToDbExpression<T>(null), typeMapping);

            foreach (var path in paths)
            {
                queryData.Add(path.ToDbExpression<T>(typeMapping, null), path.Last().TypeMapping);
            }

            var queryPlan = new QueryPlan(Context, queryData);

            _cache.Add(typeof(T), queryPlan);
        }

        public void Load<T>(Expression<Func<T, bool>> expr)
            where T : class, TEntity
        {
            //var type = typeof(T);
            
            //if (!_cache.ContainsKey(type))
            //{
            //    var paths = GraphPathCollection.Where(p => p.IsMatchingForType(type)).ToList();
            //    if (paths.Any())
            //    {
            //        LoadPaths(expr, paths);
            //    }
            //}

            //Context.ExecuteQueryPlan(_cache[type]);

            Load<T>();
        }

        private void LoadPaths<T>(Expression<Func<T, bool>> expr, IEnumerable<GraphPath> paths)
            where T : class, TEntity
        {
            var typeMapping = GetTypeMapping(typeof(T));

            var queryData = new Dictionary<DbExpression, TypeMapping>();
            queryData.Add(typeMapping.ToDbExpression(expr), typeMapping);

            foreach (var path in paths)
            {
                queryData.Add(path.ToDbExpression(typeMapping, expr), path.Last().TypeMapping);
            }

            var queryPlan = new QueryPlan(Context, queryData);
            _cache.Add(typeof(T), queryPlan);
        }

        new public SqlGraphShape<TEntity> Edge<TFrom>(Expression<Func<TFrom, TEntity>> edge)
            where TFrom : TEntity
        {
            base.Edge(edge);
            return this;
        }

        new public SqlGraphShape<TEntity> Edge<TFrom>(Expression<Func<TFrom, IEnumerable<TEntity>>> edge)
            where TFrom : TEntity
        {
            base.Edge(edge);
            return this;
        }
    }
}
