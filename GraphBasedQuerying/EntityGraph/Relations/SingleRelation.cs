using System.Collections;
using System.Collections.Generic;

namespace EntityGraph
{
    public class SingleRelation<TEntity> : IRelation<TEntity>
        where TEntity : class
    {
        public TEntity To { get; private set; }

        public SingleRelation(TEntity to)
        {
            To = to;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            yield return To;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
