using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DbTest.Core.Tests;
using DbTest.Core.Writers;
using PerformanceFramework;

namespace DbTest.Core
{
    internal class DbTester
    {
        private readonly IDatabaseConnection _connection;
        private readonly IEnumerable<Population> _populations;
        private readonly TestSettings _settings;

        private readonly IEnumerable<IWriter> _writers;

        public DbTester(IDatabaseConnection connection, IEnumerable<Population> populations,
            TestSettings settings)
        {
            _connection = connection;
            _populations = populations;
            _settings = settings;

            _writers = new List<IWriter>()
            {
                new TestSummaryWriter(),
                new DurationsWriter()
            };
        }

        public void RunTests()
        {
            Console.WriteLine("Running tests for connection '{0}'...", _connection.Name);

            var dir = _settings.OutputDir.CreateSubdirectory(_connection.Name)
                .CreateSubdirectory(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss").Replace(' ', '/') + "/");

            RunTests(InstantiateTestsForTestType(typeof(IEFConcretesTest)).ToList(), dir, "EFConcretes.");
            RunTests(InstantiateTestsForTestType(typeof(IEFTest)).ToList(), dir, "EF.");
            RunTests(InstantiateTestsForTestType(typeof(IGBQTest)).ToList(), dir, "GBQ.");
        }

        private IEnumerable<PopulationTest> InstantiateTestsForTestType(Type type)
        {
            var testTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface && t.IsSubclassOf(typeof(PopulationTest)) && type.IsAssignableFrom(t));

            foreach (var testType in testTypes)
            {
                foreach (var population in _populations)
                {
                    yield return (PopulationTest)Activator.CreateInstance(testType, _connection, population);
                }
            }
        }

        private void RunTests(IEnumerable<PopulationTest> queryTests, DirectoryInfo dir, string filePrefix)
        {
            var fw = new TestFramework(new StreamWriter(dir.FullName + filePrefix + "log"));
            fw.Tests = queryTests;

            if (_settings.BlankRuns > 0)
            {
                Console.WriteLine("Executing non-measured (blank) test runs...");

                for (int i = 0; i < _settings.BlankRuns; i++)
                {
                    fw.IgnoreRunResults = true;
                    fw.Run(1);
                }
            }

            if (_settings.RepeatCount == 0 || (_settings.SubSteps == 0 && _settings.MinimumRunTime == TimeSpan.Zero))
            {
                Console.WriteLine("No tests are measured! You must set RepeatCount > 0 and either SubSteps > 0 or MinimumRunTime > 0.");
                return;
            }

            if (_settings.SubSteps != 0)
            {
                Console.WriteLine("Running tests using SubSteps.");
                for (int i = 0; i < _settings.RepeatCount; i++)
                {
                    fw.Run(_settings.SubSteps);
                }
            }
            else if (_settings.MinimumRunTime != TimeSpan.Zero)
            {
                Console.WriteLine("Running tests using MinimumRunTime.");
                for (int i = 0; i < _settings.RepeatCount; i++)
                {
                    fw.Run(_settings.MinimumRunTime);
                }
            }

            foreach (var writer in _writers)
            {
                writer.Write(dir, filePrefix, fw);
            }
        }
    }
}
