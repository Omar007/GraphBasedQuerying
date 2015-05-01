using System.Linq;
using Xunit;

namespace EntityGraph.Test
{
    public class GraphINotifyCollectionChangedTests
    {
        [Fact]
        public void CollectionChangedTriggers()
        {
            var data = new SimpleData();
            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);
            var collectionChangedHandlerVisited = false;

            gr.CollectionChanged += (sender, args) =>
            {
                collectionChangedHandlerVisited = true;
            };

            data.A.BSet.Add(new B());
            Assert.True(collectionChangedHandlerVisited, "CollectionChanged handler not called");
        }

        [Fact]
        public void AddedToCollection()
        {
            var data = new SimpleData();
            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);

            var b = new B();
            data.A.BSet.Add(b);

            Assert.True(gr.Contains(b));
        }

        [Fact]
        public void RemovedFromCollection()
        {
            var data = new SimpleData();
            var b = new B();
            data.A.BSet.Add(b);

            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);
            Assert.True(gr.Contains(b));
            
            data.A.BSet.Remove(b);
            Assert.False(gr.Contains(b));
        }
    }
}
