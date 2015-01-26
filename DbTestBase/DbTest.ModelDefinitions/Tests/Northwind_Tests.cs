using System.Data.Entity;
using System.Linq;
using DbTest.Core;
using DbTest.ModelDefinitions.Models.Northwind;
using EntityGraph4EF6;

namespace DbTest.ModelDefinitions.Tests
{
    public class Northwind_EFTest : EFPopulationTest<NorthwindContainer>
    {
        public Northwind_EFTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            Context.Customers
                .Where(c => c.CustomerID == "FRANK")
                .Include(c => c.CustomerDemographics)
                .Include(c => c.Orders.Select(o => o.Order_Details.Select(od => od.Products)))
                .Include(c => c.Orders.Select(o => o.Shippers))
                .ToList();

            return 0;
        }

        protected override NorthwindContainer GetDbContext()
        {
            return new NorthwindContainer(Connection.CreateConnectionString(ModelName, null));
        }

        public override string ModelName
        {
            get { return "Northwind"; }
        }
    }

    public class Northwind_GBQTest : GBQPopulationTest<NorthwindContainer>
    {
        public Northwind_GBQTest(IDatabaseConnection connection, Population population)
            : base(connection, population)
        {

        }

        protected override int DoTest()
        {
            new SqlGraphShape<object>(Context)
                .Edge<Customers>(c => c.Orders)
                .Edge<Customers>(c => c.CustomerDemographics)
                .Edge<Orders>(o => o.Order_Details)
                .Edge<Orders>(o => o.Shippers)
                .Edge<Order_Details>(od => od.Products)
                .Load<Customers>(c => c.CustomerID == "FRANK");

            return 0;
        }

        protected override NorthwindContainer GetDbContext()
        {
            return new NorthwindContainer(Connection.CreateConnectionString(ModelName, null));
        }

        public override string ModelName
        {
            get { return "Northwind"; }
        }
    }
}
