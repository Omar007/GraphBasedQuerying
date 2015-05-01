using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EntityGraph.Test
{
    public class GraphShapeCopyToTests
    {
        [Fact]
        public void CopyCloneSpaceToEntityGraphTest()
        {
            //TODO: Using the following Shape compiles but will cause the test to fail as the shape is not defined from source types.
            //toTypes are used where fromTypes should be used..
            //var shape = new GraphShape<object>().Edge<G>(x => x.GHSet);

            var shape = new GraphShape<object>().Edge<CloneSpace.G>(x => x.GHSet);
            var g = new CloneSpace.G { GHSet = new List<CloneSpace.GH> { new CloneSpace.GH() } };
            var result = shape.CopyTo<object, BaseData>(g, new AssemblyTypeMapper<H>());

            Assert.NotNull(result);
            Assert.IsType<G>(result);
            Assert.Equal(g.GHSet.Count(), ((G)result).GHSet.Count());
        }

        [Fact]
        public void CopyEntityGraphTestToCloneSpace()
        {
            var g = new G();
            g.GHSet.Add(new GH());

            var shape = new GraphShape<BaseData>().Edge<G>(x => x.GHSet);
            var result = shape.CopyTo<BaseData, object>(g, new AssemblyTypeMapper<CloneSpace.E>());

            Assert.NotNull(result);
            Assert.IsType<CloneSpace.G>(result);
            Assert.Equal(g.GHSet.Count(), ((CloneSpace.G)result).GHSet.Count());
        }

        [Fact]
        public void CopiesArrays()
        {
            var i = new I { X = new[] { 1.1, 2.2, 3.3 }, AString = "Hello" };
            
            var shape = new FullDynamicGraphShape<BaseData>();
            var result = shape.CopyTo<BaseData, object>(i, new AssemblyTypeMapper<CloneSpace.I>());
            
            Assert.NotNull(result);
            Assert.IsType<CloneSpace.I>(result);
            Assert.Equal(i.X.Count(), ((CloneSpace.I)result).X.Count());
        }
    }
}
