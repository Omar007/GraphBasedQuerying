using System.Linq;
using Xunit;

namespace EntityGraph.Test
{
    public class GraphIEnumerableTests
    {
		[Fact]
        public void GraphCountFindsAllItemsInCircularGraphFull()
        {
            var data = new SimpleData();
            data.A.BSet.Add(new B());

            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);
            Assert.Equal(5, gr.Count());
        }

        [Fact]
        public void GraphCountFindsAllItemsInCircularGraphShape1()
        {
            var data = new SimpleData();
            data.A.BSet.Add(new B());

            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphShape1);
            Assert.Equal(4, gr.Count());
        }

        [Fact]
        public void GraphOfTypeFindsAllItemsInCircularGraphFull()
        {
            var data = new SimpleData();
            var newB = new B();
            data.A.BSet.Add(newB);

            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);
            Assert.Equal(data.A, gr.OfType<A>().Single());
            Assert.Equal(data.C, gr.OfType<C>().Single());
            Assert.Equal(data.D, gr.OfType<D>().Single());

            Assert.True(gr.OfType<B>().Contains(data.B));
            Assert.True(gr.OfType<B>().Contains(newB));

            Assert.Equal(5, gr.Count());
        }

        [Fact]
        public void GraphOfTypeFindsAllItemsInCircularGraphShape1()
        {
            var data = new SimpleData();
            var newB = new B();
            data.A.BSet.Add(newB);

            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphShape1);
            Assert.Equal(data.A, gr.OfType<A>().Single());
            Assert.Equal(data.B, gr.OfType<B>().Single());
            Assert.Equal(data.C, gr.OfType<C>().Single());
            Assert.Equal(data.D, gr.OfType<D>().Single());

            Assert.Equal(4, gr.Count());
        }
    }
}
