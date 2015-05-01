using System.Collections.Generic;
using System.Text;
using EntityGraph4EF6.Mapping;
using EntityGraph4EF6.SQL;

namespace EntityGraph4EF6
{
    internal class QueryPlan
    {
        public IEnumerable<TypeMapping> TypeMappings { get; private set; }
        public string CommandText { get; private set; }

        public QueryPlan(IEnumerable<SelectColumns> queries, IEnumerable<TypeMapping> typeMappings)
        {
            TypeMappings = typeMappings;

            var sb = new StringBuilder();
            foreach (var q in queries)
            {
                q.AppendToStringBuilder(sb);
            }
            CommandText = sb.ToString();
        }
    }
}
