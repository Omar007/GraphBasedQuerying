using System.Linq;
using Xunit;

namespace EntityGraph.Test
{
    public class GraphINotifyPropertyChangedTests
    {
        [Fact]
        public void PropertyChangedTriggers()
        {
            var data = new SimpleData();
            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);
            var propertyChangedHandlerVisited = false;

            gr.PropertyChanged += (sender, args) => { propertyChangedHandlerVisited = true; };
            data.D.Name = "Hello";

            Assert.True(propertyChangedHandlerVisited);
        }

        [Fact]
        public void AssociationAdded()
        {
            var data = new SimpleData();
            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);

            var newB = new B();
            Assert.False(gr.Contains(newB));

            data.A.B = newB;
            Assert.True(gr.Contains(newB));
        }

        [Fact]
        public void AsociationRemoved()
        {
            var data = new SimpleData();
            var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull);
            Assert.True(gr.Contains(data.B));

            data.A.B = null;
            Assert.False(gr.Contains(data.B));
        }
    }
}
