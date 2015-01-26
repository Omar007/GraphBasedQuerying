using System.Collections.Generic;

namespace EntityGraph
{
    public interface IRelation<TEntity> : IEnumerable<TEntity>
        where TEntity : class
    {

    }
}
