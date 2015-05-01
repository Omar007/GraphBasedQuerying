using System;

namespace EntityGraph
{
    public interface ITypeMapper
    {
        Type Map(Type fromType);
    }
}
