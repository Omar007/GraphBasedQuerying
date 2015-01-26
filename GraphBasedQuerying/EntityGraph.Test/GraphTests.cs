using System;
using Xunit;

namespace EntityGraph.Test
{
    public class GraphTests
    {
        [Fact]
        public void ThrowsSourceArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Graph<object>(null, new GraphShape<object>()));
        }

        [Fact]
        public void ThrowsShapeArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Graph<BaseData>(new A(), null));
        }
    }
}
