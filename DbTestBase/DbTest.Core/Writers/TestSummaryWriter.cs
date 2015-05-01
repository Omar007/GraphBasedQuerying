using System.Collections.Generic;
using System.IO;
using System.Linq;
using DbTest.Core.Tests;
using PerformanceFramework;

namespace DbTest.Core.Writers
{
    internal class TestSummaryWriter : IWriter
    {
        public void Write(DirectoryInfo dir, string filePrefix, IEnumerable<TestBase> tests)
        {
            foreach (var testGroup in tests.GroupBy(t => (t as PopulationTest).ModelName).OrderBy(g => g.Key))
            {
                using (var csvFile = new StreamWriter(dir.FullName + filePrefix + testGroup.Key + ".csv"))
                {
                    csvFile.Write(@"Population,");
                    csvFile.Write(@"Minimum,");
                    csvFile.Write(@"Maximum,");
                    csvFile.Write(@"Average,");
                    csvFile.Write(@"Median,");
                    csvFile.Write(@"Standard Deviation,");
                    csvFile.Write(@"Thread Minimum,");
                    csvFile.Write(@"Thread Maximum,");
                    csvFile.Write(@"Thread Average,");
                    csvFile.Write(@"Thread Median,");
                    csvFile.Write(@"Thread Standard Deviation");
                    csvFile.WriteLine();

                    foreach (var resultGroup in testGroup.GroupBy(t => (t as PopulationTest).Population.ECount).OrderBy(g => g.Key))
                    {
                        csvFile.Write(resultGroup.Key);

                        var test = resultGroup.Single();
                        if (test.HasResults)
                        {
                            csvFile.Write(',');
                            csvFile.Write(test.Minimum.TotalMilliseconds);
                            csvFile.Write(',');
                            csvFile.Write(test.Maximum.TotalMilliseconds);
                            csvFile.Write(',');
                            csvFile.Write(test.Average.TotalMilliseconds);
                            csvFile.Write(',');
                            csvFile.Write(test.Median.TotalMilliseconds);
                            csvFile.Write(',');
                            csvFile.Write(test.StandardDeviation.TotalMilliseconds);
                            csvFile.Write(',');
                            csvFile.Write(test.ThreadMinimum.TotalMilliseconds);
                            csvFile.Write(',');
                            csvFile.Write(test.ThreadMaximum.TotalMilliseconds);
                            csvFile.Write(',');
                            csvFile.Write(test.ThreadAverage.TotalMilliseconds);
                            csvFile.Write(',');
                            csvFile.Write(test.ThreadMedian.TotalMilliseconds);
                            csvFile.Write(',');
                            csvFile.Write(test.ThreadStandardDeviation.TotalMilliseconds);
                        }
                        else
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                csvFile.Write(",NaN");
                            }
                        }

                        csvFile.WriteLine();
                    }
                }
            }
        }
    }
}
