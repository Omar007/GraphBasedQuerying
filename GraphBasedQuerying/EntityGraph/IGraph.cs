using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace EntityGraph
{
    public interface IGraph<TEntity> : IChangeTracking, IDisposable, IEnumerable<TEntity>,
        IEquatable<IGraph<TEntity>>, INotifyCollectionChanged, INotifyPropertyChanged
        where TEntity : class
    {
        TEntity Source { get; }
        IGraphShape<TEntity> GraphShape { get; }
    }
}
