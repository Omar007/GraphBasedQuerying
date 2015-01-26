using Xunit;

namespace EntityGraph.Test
{
    public class GraphIEquatableTests
    {
        [Fact]
        public void FullGraphEqualsSelf()
        {
            var data = new SimpleData();
            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);

            Assert.True(gr.Equals(gr));
        }

        [Fact]
        public void FullGraphANotEqualToFullGraphB()
        {
            var data = new SimpleData();
            var gr1 = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);
            var gr2 = new Graph<BaseData>(data.B, GraphShapes.CircularGraphFull);

            Assert.False(gr1.Equals(gr2));
        }

        [Fact]
        public void TypesOfFullGraphAEqualTypesOfFullGraphB()
        {
            var data = new SimpleData();
            var gr1 = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);
            var gr2 = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);

            Assert.True(gr1.Equals(gr2, (e1, e2) => e1.GetType() == e2.GetType()));
        }
    }
}
