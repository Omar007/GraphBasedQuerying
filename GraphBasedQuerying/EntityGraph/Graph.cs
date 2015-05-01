using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace EntityGraph
{
    public class Graph<TEntity> : IGraph<TEntity>
        where TEntity : class
    {
        private EventHandler<EventArgs> RelationGraphResetting;
        private EventHandler<EventArgs> RelationGraphResetted;

        private RelationGraph<TEntity> _relationGraph;
        protected RelationGraph<TEntity> RelationGraph
        {
            get
            {
                _relationGraph = _relationGraph ?? new RelationGraph<TEntity>(this);
                return _relationGraph;
            }
            private set { _relationGraph = value; }
        }

        public TEntity Source { get; private set; }
        public IGraphShape<TEntity> GraphShape { get; private set; }
        
        public Graph(TEntity source, IGraphShape<TEntity> graphShape)
        {
            if(source == null)
            {
                throw new ArgumentNullException("source", "The constructor argument 'source' can't be null");
            }
            if(graphShape == null)
            {
                throw new ArgumentNullException("graphShape", "The constructor argument 'graphShape' can't be null");
            }

            Source = source;
            GraphShape = graphShape;

            InitializeINotifyPropertyChanged();
            InitializeINotifyCollectionChanged();
        }

        protected void ResetRelationGraph()
        {
            if (RelationGraphResetting != null)
            {
                RelationGraphResetting(this, new EventArgs());
            }
            RelationGraph = null;
            if (RelationGraphResetted != null)
            {
                RelationGraphResetted(this, new EventArgs());
            }
        }

        #region IChangeTracking Implementation
        public bool IsChanged
        {
            get { return RelationGraph.Nodes.OfType<IChangeTracking>().Aggregate(false, (isChanged, e) => isChanged || e.IsChanged); }
        }

        public void AcceptChanges()
        {
            foreach (var changeTracker in RelationGraph.Nodes.OfType<IChangeTracking>())
            {
                changeTracker.AcceptChanges();
            }
        }
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposePropertyChangedHandlers();
                DisposeCollectionChangedHandlers();
            }
        }
        #endregion

        #region IEnumerable<TEntity> Implementation
        public IEnumerator<TEntity> GetEnumerator()
        {
            return RelationGraph.Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IEquatable<IGraph<TEntity>> Implementation
        public bool Equals(IGraph<TEntity> other, Func<TEntity, TEntity, bool> comparer)
        {
            if (RelationGraph.Nodes.Count() != other.Count())
            {
                return false;
            }

            var zipList = RelationGraph.Nodes.Zip(other, (e1, e2) => new { e1, e2 });
            return zipList.All(elem => comparer(elem.e1, elem.e2));
        }

        public bool Equals(IGraph<TEntity> other)
        {
            return Equals(other, (e1, e2) => e1 == e2);
        }
        #endregion

        #region INotifyCollectionChanged Implementation
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, args);
            }
        }

        private void InitializeINotifyCollectionChanged()
        {
            RelationGraphResetting += (sender, args) => DisposeCollectionChangedHandlers();
            RelationGraphResetted += (sender, args) =>
            {
                InitializeCollectionChangedHandlers();
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            };
            InitializeCollectionChangedHandlers();
        }

        private void InitializeCollectionChangedHandlers()
        {
            foreach (var list in RelationGraph.Relations.OfType<MultiRelation<TEntity>>()
                .Select(r => r.To as INotifyCollectionChanged).Where(l => l != null))
            {
                list.CollectionChanged += CollectionCollectionChanged;
            }
        }

        private void DisposeCollectionChangedHandlers()
        {
            foreach (var list in RelationGraph.Relations.OfType<MultiRelation<TEntity>>()
                .Select(r => r.To as INotifyCollectionChanged).Where(l => l != null))
            {
                list.CollectionChanged -= CollectionCollectionChanged;
            }
        }

        private void CollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ResetRelationGraph();
            if (CollectionChanged != null)
            {
                CollectionChanged(sender, e);
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, args);
            }
        }

        private void InitializeINotifyPropertyChanged()
        {
            RelationGraphResetting += (sender, args) => DisposePropertyChangedHandlers();
            RelationGraphResetted += (sender, args) => InitializePropertyChangedHandlers();

            InitializePropertyChangedHandlers();
        }

        private void InitializePropertyChangedHandlers()
        {
            foreach (var node in RelationGraph.Nodes.OfType<INotifyPropertyChanged>())
            {
                node.PropertyChanged += NodePropertyChanged;
            }
        }

        private void DisposePropertyChangedHandlers()
        {
            foreach (var node in RelationGraph.Nodes.OfType<INotifyPropertyChanged>())
            {
                node.PropertyChanged -= NodePropertyChanged;
            }
        }

        private void NodePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propInfo = sender.GetType().GetProperty(e.PropertyName);
            if (GraphShape.IsEdge(propInfo))
            {
                ResetRelationGraph();
            }
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, e);
            }
        }
        #endregion
    }
}
