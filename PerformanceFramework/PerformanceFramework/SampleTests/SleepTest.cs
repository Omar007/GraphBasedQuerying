using System.Threading.Tasks;

namespace PerformanceFramework.SampleTests
{
    public class SleepTest : TestBase
    {
        public override string Name { get { return "Sleep Test"; } }

        protected override int DoTest()
        {
            Task.Delay(5000).RunSynchronously();
            
            return 0;
        }
    }
}
