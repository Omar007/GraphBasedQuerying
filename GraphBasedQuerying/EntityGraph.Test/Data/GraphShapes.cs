
namespace EntityGraph.Test
{
    public static class GraphShapes
    {
        public static IGraphShape<BaseData> SimpleGraphShape1
        {
            get
            {
                return new GraphShape<BaseData>()
                    .Edge<E>(x => x.F)
                    .Edge<F>(x => x.ESet);
            }
        }

        public static IGraphShape<BaseData> SimpleGraphShape2
        {
            get
            {
                return new GraphShape<BaseData>()
                    .Edge<F>(x => x.ESet)
                    .Edge<GH>(x => x.G)
                    .Edge<G>(x => x.GHSet);
            }
        }

        public static IGraphShape<BaseData> SimpleGraphShape3
        {
            get
            {
                return new GraphShape<BaseData>()
                    .Edge<GH>(x => x.H)
                    .Edge<H>(x => x.GhSet);
            }
        }

        public static IGraphShape<BaseData> SimpleGraphShapeFull
        {
            get
            {
                return new GraphShape<BaseData>()
                    .Edge<E>(x => x.F)
                    .Edge<F>(x => x.ESet)
                    .Edge<GH>(x => x.G)
                    .Edge<GH>(x => x.H)
                    .Edge<G>(x => x.GHSet)
                    .Edge<H>(x => x.GhSet);
            }
        }

        public static IGraphShape<BaseData> CircularGraphShape1
        {
            get
            {
                return new GraphShape<BaseData>()
                    .Edge<A>(x => x.B)
                    .Edge<B>(x => x.C)
                    .Edge<C>(x => x.D)
                    .Edge<D>(x => x.A);
            }
        }

        public static IGraphShape<BaseData> CircularGraphFull
        {
            get
            {
                return new GraphShape<BaseData>()
                    .Edge<A>(x => x.B)
                    .Edge<A>(x => x.BSet)
                    .Edge<B>(x => x.A)
                    .Edge<B>(x => x.C)
                    .Edge<C>(x => x.D)
                    .Edge<D>(x => x.A);
            }
        }
    }
}
