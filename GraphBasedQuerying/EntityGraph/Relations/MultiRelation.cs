using System.Collections;
using System.Collections.Generic;

namespace EntityGraph
{
    public class MultiRelation<TEntity> : IRelation<TEntity>
        where TEntity : class
    {
        public IEnumerable<TEntity> To { get; private set; }
        
        public MultiRelation(IEnumerable<TEntity> to)
        {
            To = to;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return To.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
