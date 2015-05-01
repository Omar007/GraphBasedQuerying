using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Graph4EF
{
    public class GraphShape
    {
        private readonly IDictionary<Type, IDictionary<Type, Expression>> _edges;

        public GraphShape()
        {
            _edges = new Dictionary<Type, IDictionary<Type, Expression>>();
        }

        public IEnumerable<TLoad> Load<TLoad>(DbContext context)
            where TLoad : class
        {
            var baseQuery = context.Set<TLoad>().OfType<TLoad>();
            var plan = new QueryPlan(_edges);
            return plan.Execute(context, baseQuery);
        }

        public IEnumerable<TLoad> Load<TLoad>(DbContext context, Expression<Func<TLoad, bool>> where)
            where TLoad : class
        {
            var baseQuery = context.Set<TLoad>().OfType<TLoad>().Where(where);
            var plan = new QueryPlan(_edges);
            return plan.Execute(context, baseQuery);
        }

        public GraphShape Edge<TFrom, TTo>(Expression<Func<TFrom, TTo>> edge)
            where TFrom : class
            where TTo : class
        {
            if (!_edges.ContainsKey(typeof(TFrom)))
            {
                _edges.Add(typeof(TFrom), new Dictionary<Type, Expression>());
            }
            _edges[typeof(TFrom)].Add(typeof(TTo), edge);

            return this;
        }

        public GraphShape Edge<TFrom, TTo>(Expression<Func<TFrom, IEnumerable<TTo>>> edge)
            where TFrom : class
            where TTo : class
        {
            if (!_edges.ContainsKey(typeof(TFrom)))
            {
                _edges.Add(typeof(TFrom), new Dictionary<Type, Expression>());
            }
            _edges[typeof(TFrom)].Add(typeof(TTo), edge);

            return this;
        }
    }
}
