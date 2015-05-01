using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using EntityGraph;

namespace EntityGraph4EF6
{
    internal class TypeTreeGraph<TEntity>
        where TEntity : class
    {
        private class TypeTreeEdgeComparer : IComparer<TypeTreeEdge>
        {
            public int Compare(TypeTreeEdge x, TypeTreeEdge y)
            {
                if (x == y)
                {
                    return 0;
                }
                if (x.To == y.From)
                {
                    return -1;
                }
                if (x.From == y.To)
                {
                    return 1;
                }
                return -1;
            }
        }

        private readonly ICollection<TypeTreeEdge> _edges;

        public TypeTreeGraph(DbContext context, GraphShape<TEntity> shape)
        {
            var edges = shape.Select(x => new TypeTreeEdge(new TypeTree(context, x.FromType),
                new TypeTree(context, x.ToType), x));

            Unify(edges);

            _edges = edges.OrderBy(e => e, new TypeTreeEdgeComparer()).ToList();
        }

        private void Unify(IEnumerable<TypeTreeEdge> edges)
        {
            var types = edges.SelectMany(e => new [] { e.From, e.To }).Distinct().ToArray();
            bool isUnified = false;
            while (!isUnified)
            {
                isUnified = true;
                for (int i = 0; i < types.Length; i++)
                {
                    var first = types[i];
                    for (int j = 1; j < types.Length; j++)
                    {
                        var second = types[j];
                        if (first.EntityType == second.EntityType && first != second)
                        {
                            types[j] = types[i];
                            isUnified = false;
                        }
                    }
                }
            }

            foreach (var e in edges)
            {
                e.From = types.First(t => t.EntityType == e.From.EntityType);
                e.To = types.First(t => t.EntityType == e.To.EntityType);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var group in _edges.GroupBy(e => e.From))
            {
                sb.AppendLine(String.Format("{0}({1})", group.Key.EntityType.Name, group.Key.EntityType.Name));
                foreach (var n in group)
                {
                    sb.AppendLine(String.Format("   {0}({1}:{2})", n.To.EntityType.Name, n.To.EntityType.Name, n.Edge.EdgeInfo.Name));
                }
            }
            return sb.ToString();
        }
    }
}
