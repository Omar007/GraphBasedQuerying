using DbTest.Core;
using EntityGraph.SQL;
using NorthwindModel;

namespace DbTest.Merijn.Tests
{
    public class GraphBasedQueryNorthwind : GraphBasedQuery<NorthwindContainer, Customers>
    {
        public GraphBasedQueryNorthwind(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override Customers StubEntity
        {
            get { return new Customers { CustomerID = "FRANK" }; }
        }

        protected override EntityGraphShape4SQL Shape
        {
            get
            {
                return base.Shape
                    .Edge<Customers, Orders>(c => c.Orders)
                    .Edge<Orders, Order_Details>(c => c.Order_Details)
                    .Edge<Order_Details, Products>(c => c.Products);
            }
        }
    }
}
