using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntityGraph
{
    public class AssembliesTypeMapper : ITypeMapper
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public AssembliesTypeMapper(params Type[] types)
        {
            _assemblies = types.Select(x => x.Assembly).ToList();
        }

        public Type Map(Type fromType)
        {
            foreach (var assembly in _assemblies)
            {
                var types = assembly.GetExportedTypes().Where(t => t != fromType);
                var toType = types.SingleOrDefault(type => type.Name.Equals(fromType.Name));
                if (toType != null)
                {
                    return toType;
                }
            }
            throw new Exception("Can't map type " + fromType.FullName);
        }
    }

    public class AssemblyTypeMapper<TTo> : AssembliesTypeMapper
        where TTo : class
    {
        public AssemblyTypeMapper()
            : base(typeof(TTo))
        {

        }
    }
}
