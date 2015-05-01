using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EntityGraph.Test
{
    /// <summary>
    ///     E      F
    ///     .F*--1 .ESet
    ///     G           GH      H
    ///     .GHSet 1--* .G
    ///     .H *--1 .GHSet
    /// </summary>
    public class E : BaseData
    {
        public E()
        {
            Id = IdFactory.Assign;
        }

        [DataMember]
        public int Id { get; set; }

        public F F { get; set; }

        [DataMember]
        public int? FId { get; set; }
    }

    public class F : BaseData
    {
        public F()
        {
            Id = IdFactory.Assign;
        }

        [DataMember]
        public int Id { get; set; }

        public List<E> ESet { get; set; }
    }

    public class GH : BaseData
    {
        [DataMember]
        public int GId { get; set; }

        public G G { get; set; }

        [DataMember]
        public int HId { get; set; }

        public H H { get; set; }
    }

    public enum GEnum
    {
        V1,
        V2
    }

    public class G : BaseData
    {
        public G()
        {
            Id = IdFactory.Assign;
            GHSet = new List<GH>();
        }

        [DataMember]
        public int Id { get; set; }

        public List<GH> GHSet { get; set; }

        [DataMember]
        public GEnum GEnum { get; set; }
    }

    public class H : BaseData
    {
        public H()
        {
            Id = IdFactory.Assign;
        }

        [DataMember]
        public int Id { get; set; }

        public List<GH> GhSet { get; set; }
    }

    public class I : BaseData
    {
        public I()
        {
            Id = IdFactory.Assign;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public IEnumerable<double> X { get; set; }

        [DataMember]
        public string AString { get; set; }
    }
}

namespace CloneSpace
{
    public class E
    {
        public int Id { get; set; }

        public F F { get; set; }
        public int? FId { get; set; }
    }

    public class F
    {
        public int Id { get; set; }
        public List<E> ESet { get; set; }
    }

    public class GH
    {
        public int GId { get; set; }

        public G G { get; set; }

        public int HId { get; set; }
        public H H { get; set; }
    }

    public enum GEnum
    {
        V1, V2
    }

    public class G
    {
        [DataMember]
        public GEnum GEnum { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<GH> GHSet { get; set; }
    }

    public class H
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<GH> GHSet { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
    public class I
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<double> X { get; set; }
        [DataMember]
        public string AString { get; set; }
    }
}
