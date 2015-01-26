
namespace EntityGraph.Test
{
    public class SimpleData
    {
        public A A { get; private set; }
        public B B { get; private set; }
        public C C { get; private set; }
        public D D { get; private set; }

        public SimpleData()
        {
            A = new A {Name = "A"};
            B = new B {Name = "B"};
            C = new C {Name = "C"};
            D = new D {Name = "D"};

            A.B = B;
            B.ASet.Add(A);
            B.C = C;
            C.BSet.Add(B);
            C.D = D;
            D.CSet.Add(C);
            D.A = A;
            A.DSet.Add(D);
        }
    }
}
