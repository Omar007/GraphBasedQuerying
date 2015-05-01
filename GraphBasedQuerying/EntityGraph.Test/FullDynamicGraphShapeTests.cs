using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EntityGraph.Test
{
    public class FullDynamicGraphShapeTests
    {
        public class TestA
        {
            public TestB B { get; set; }

            public int Foo { get; set; }

            public string TestString { get; set; }
        }

        public class TestB
        {
            public int Bar { get; set; }
            public List<TestC> Cs { get; set; }
            public TestA A { get; set; }
        }

        public class TestC
        {
            public TestA A { get; set; }
        }

        public class TestD
        {
            public IEnumerable<int> Ints { get; set; }
            public string String { get; set; }
        }

        [Fact]
        public void FullDynamicGraphShapeDetectsSingleAssociation()
        {
            var a = new TestA { B = new TestB() };

            var shape = new FullDynamicGraphShape<object>();
            var outEdges = shape.OutEdges(a).ToArray();

            Assert.Equal(1, outEdges.Count());
            Assert.Equal(typeof(TestA).GetProperty("B"), outEdges.Single());

            var graph = new Graph<object>(a, shape);
            Assert.Equal(2, graph.Count());
        }

        [Fact]
        public void FullDynamicGraphShapeDetectsMultiAssociations()
        {
            var shape = new FullDynamicGraphShape<object>();
            var outedges = shape.OutEdges(new TestB());
            Assert.Equal(2, outedges.Count());
        }

        [Fact]
        public void FullDynamicGraphShapeDetectsCollectionAssociations()
        {
            var a = new TestA { B = new TestB() };
            a.B.Cs = new List<TestC> { new TestC() };

            var shape = new FullDynamicGraphShape<object>();

            var outedges = shape.OutEdges(a.B).ToArray();
            Assert.Equal(2, outedges.Count());

            var collection = outedges.Single(x => x.Name == "Cs");

            Assert.Equal(typeof(TestB).GetProperty("Cs"), collection);

            var graph = new Graph<object>(a, shape);
            Assert.Equal(3, graph.Count());
        }

        [Fact]
        public void FullDynamicGraphShapeCompatibleWithCycle()
        {
            var a = new TestA { B = new TestB() };
            a.B.Cs = new List<TestC> { new TestC { A = a } };

            var graph = new Graph<object>(a, new FullDynamicGraphShape<object>());
            Assert.Equal(3, graph.Count());
        }

        [Fact]
        public void FullDynamicGraphShapeExcludesSimpleTypeArrays()
        {
            var d = new TestD();
            var shape = new FullDynamicGraphShape<object>();
            Assert.False(shape.OutEdges(d).Any());

            d.Ints = new[] { 1, 2, 3 };
            Assert.False(shape.OutEdges(d).Any());
        }
    }
}
