using Xunit;

namespace EntityGraph.Test
{
    public class GraphFactoryTests
    {
        [Fact]
        public void FactoryReusesExistingGraphs()
        {
            var a1 = new A();
            var a2 = new A();
            var shape1 = new GraphShape<A>();
            var shape2 = new GraphShape<A>();

            var gr1 = GraphFactory.GetInstance(a1, shape1);
            var gr2 = GraphFactory.GetInstance(a1, shape2);
            var gr3 = GraphFactory.GetInstance(a2, shape1);
            var gr4 = GraphFactory.GetInstance(a2, shape2);
            var gr5 = GraphFactory.GetInstance(a1, shape1);

            Assert.True(gr1 == gr5);

            Assert.False(gr1 == gr2);
            Assert.False(gr1 == gr3);
            Assert.False(gr1 == gr4);
            Assert.False(gr2 == gr3);
            Assert.False(gr2 == gr4);
            Assert.False(gr3 == gr4);
        }
    }
}
