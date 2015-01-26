using Xunit;

namespace EntityGraph.Test
{
    public class GraphIDisposableTests
    {
		[Fact]
        public void DisposesOfEventHandlers()
        {
            var data = new SimpleData();

            using (var gr = new Graph<BaseData>(data.A, GraphShapes.CircularGraphFull))
            {
                gr.PropertyChanged += (sender, args) =>
                {
                    Assert.True(false, "PropertyChanged called after Dispose()");
                };
                gr.CollectionChanged += (sender, args) =>
                {
                    Assert.True(false, "CollectionChanged called after Dispose()");
                };
            }

            data.A.Name = "Some Name";
            data.A.BSet.Add(new B());
        }
    }
}
