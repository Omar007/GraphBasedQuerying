using System.Collections;
using System.Collections.Generic;

namespace EntityGraph
{
    public class RelationNode<TEntity> : ICollection<IRelation<TEntity>>
        where TEntity : class
    {
        private readonly ICollection<IRelation<TEntity>> _relations;

        public int Count { get { return _relations.Count; } }
        public bool IsReadOnly { get { return _relations.IsReadOnly; } }

        public TEntity Node { get; private set; }

        public RelationNode(TEntity node)
        {
            Node = node;

            _relations = new HashSet<IRelation<TEntity>>();
        }

        public void Add(IRelation<TEntity> relation)
        {
            _relations.Add(relation);
        }

        public void Clear()
        {
            _relations.Clear();
        }

        public bool Contains(IRelation<TEntity> item)
        {
            return _relations.Contains(item);
        }

        public void CopyTo(IRelation<TEntity>[] array, int arrayIndex)
        {
            _relations.CopyTo(array, arrayIndex);
        }

        public bool Remove(IRelation<TEntity> item)
        {
            return _relations.Remove(item);
        }

        public IEnumerator<IRelation<TEntity>> GetEnumerator()
        {
            return _relations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
