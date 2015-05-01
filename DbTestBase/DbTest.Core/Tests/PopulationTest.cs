using PerformanceFramework;

namespace DbTest.Core.Tests
{
    public abstract class PopulationTest : TestBase
    {
        public override string Name { get { return Connection.Name + "." + ModelName + "." + Population.ToString(); } }

        protected readonly int SelectedIndex;

        public abstract string ModelName { get; }
        public readonly IDatabaseConnection Connection;
        public readonly Population Population;
        
        public PopulationTest(IDatabaseConnection connection, Population population)
        {
            Connection = connection;
            Population = population;

            SelectedIndex = (int)(population.ECount * .75);
        }
    }
}
