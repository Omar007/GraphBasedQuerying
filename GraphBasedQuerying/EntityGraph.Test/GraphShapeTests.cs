using System;
using System.Linq;
using Xunit;

namespace EntityGraph.Test
{
    public class GraphShapeTests
    {
        [Fact]
        public void GraphContainsDefined()
        {
            var data = new SimpleData();
            var newB = new B { Name = "NewB" };
            data.A.BSet.Add(newB);

            var shape = new GraphShape<BaseData>()
                .Edge<A>(x => x.B)
                .Edge<A>(x => x.BSet);
            var gr = new Graph<BaseData>(data.A, shape);

            Assert.True(gr.Contains(data.A));
            Assert.True(gr.Contains(data.B));
            Assert.True(gr.Contains(newB));
        }

        [Fact]
        public void DetectsInvalidPathExpressions1()
        {
            Assert.Throws<Exception>(() => new GraphShape<BaseData>().Edge<A>(x => x.B.C.D));
        }

        [Fact]
        public void DetectsInvalidPathExpressions2()
        {
            Assert.Throws<Exception>(() => new GraphShape<BaseData>().Edge<A>(x => x.BSet.First()));
        }

        [Fact]
        public void DetectsInvalidPathExpressions3()
        {
            Assert.Throws<Exception>(() => new GraphShape<BaseData>().Edge<A>(x => x.B.ASet));
        }

        [Fact]
        public void ShapeUnionFunctionsProperly()
        {
            var shape1 = new GraphShape<BaseData>()
                .Edge<A>(x => x.DSet)
                .Edge<A>(x => x.B)
                .Edge<B>(x => x.C);
            var shape2 = new GraphShape<BaseData>()
                .Edge<A>(x => x.DSet)
                .Edge<A>(x => x.B)
                .Edge<C>(x => x.D);
            var shape3 = shape1.Union(shape2);

            Assert.True(shape1.All(shape3.Contains));
            Assert.True(shape2.All(shape3.Contains));
            Assert.False(shape3.All(shape1.Contains));
            Assert.False(shape3.All(shape2.Contains));

            Assert.False(shape3.Any(edge => !shape1.Contains(edge) && !shape2.Contains(edge)));
            Assert.Equal(4, shape3.Count());
        }
    }
}
