using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace EntityGraph.Test
{
    /// <summary>
    ///     A                  B      C          D
    ///     .B           *--1 .ASet
    ///     .BNotInGraph *--1
    ///     .BSet        1--* .A
    ///     .DSet        1--*                    .A
    ///     .C *--1 .BSet
    ///     .D    *--1 .CSet
    /// </summary>
    public class A : BaseData
    {
        private B _b;

        public A()
        {
            Id = IdFactory.Assign;
            BSet = new ObservableCollection<B>();
            DSet = new ObservableCollection<D>();
        }

        [DataMember]
        public int Id { get; set; }

        public B B
        {
            get { return _b; }
            set
            {
                if (Equals(value, _b)) return;
                _b = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public int? BId { get; set; }

        public B BNotInGraph { get; set; }

        [DataMember]
        public int? BNotInGraphId { get; set; }

        public IList<B> BSet { get; set; }

        public IList<D> DSet { get; set; }

        [DataMember]
        public string LastName { get; set; }
    }

    public class B : BaseData
    {
        public B()
        {
            Id = IdFactory.Assign;
            ASet = new ObservableCollection<A>();
        }

        [DataMember]
        public int Id { get; set; }

        public A A { get; set; }

        [DataMember]
        public int? AId { get; set; }

        public IList<A> ASet { get; set; }

        public C C { get; set; }

        [DataMember]
        public int? CId { get; set; }
    }

    public class C : BaseData
    {
        public C()
        {
            Id = IdFactory.Assign;
            BSet = new ObservableCollection<B>();
        }

        [DataMember]
        public int Id { get; set; }

        public IList<B> BSet { get; set; }

        public D D { get; set; }

        [DataMember]
        public int? DId { get; set; }
    }

    public class D : BaseData
    {
        public D()
        {
            Id = IdFactory.Assign;
            CSet = new ObservableCollection<C>();
        }

        [DataMember]
        public int Id { get; set; }

        public A A { get; set; }

        [DataMember]
        public int? AId { get; set; }

        public IList<C> CSet { get; set; }
    }
}
