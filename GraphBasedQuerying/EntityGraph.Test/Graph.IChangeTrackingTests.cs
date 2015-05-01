using System.ComponentModel;
using System.Linq;
using Xunit;

namespace EntityGraph.Test
{
    public class GraphIChangeTrackingTests
    {
        [Fact]
        public void IsChangedAndAcceptChangesTest()
        {
            var data = new SimpleData();
            var graph = new Graph<BaseData>(data.A, new FullDynamicGraphShape<BaseData>());

            foreach (var node in graph.OfType<IChangeTracking>())
            {
                node.AcceptChanges();
            }

            Assert.False(graph.IsChanged);

            data.A.B.Name = "Changed";
            Assert.True(graph.IsChanged);

            graph.AcceptChanges();
            Assert.False(graph.IsChanged);
        }
    }
}
