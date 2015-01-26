using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace EntityGraph.Test
{
    public class BaseData : INotifyPropertyChanged, IChangeTracking
    {
        private string _name;

        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            IsChanged = true;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Implementation of IChangeTracking

        public void AcceptChanges()
        {
            IsChanged = false;
        }

        public bool IsChanged { get; private set; }

        #endregion
    }
}
