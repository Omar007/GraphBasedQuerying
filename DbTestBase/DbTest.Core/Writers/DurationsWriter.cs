using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DbTest.Core.Tests;
using PerformanceFramework;

namespace DbTest.Core.Writers
{
    internal class DurationsWriter : IWriter
    {
        public void Write(DirectoryInfo dir, string filePrefix, IEnumerable<TestBase> tests)
        {
            foreach (var testGroup in tests.GroupBy(t => (t as PopulationTest).ModelName).OrderBy(g => g.Key))
            {
                using (var csvFile = new StreamWriter(dir.FullName + filePrefix + testGroup.Key + ".durations.csv"))
                {
                    var output = new List<List<string>>();

                    foreach (var resultGroup in testGroup.GroupBy(t => (t as PopulationTest).Population.ECount).OrderBy(g => g.Key))
                    {
                        var realColumn = output.Count;
                        output.Add(new List<string>() { "Real " + resultGroup.Key });
                        var cpuColumn = output.Count;
                        output.Add(new List<string>() { "CPU " + resultGroup.Key });

                        var test = resultGroup.Single();
                        if (test.HasResults)
                        {
                            foreach (var result in test)
                            {
                                output[realColumn].Add(result.Duration.TotalMilliseconds.ToString());
                                output[cpuColumn].Add(result.ThreadDuration.TotalMilliseconds.ToString());
                            }
                        }
                        else
                        {
                            output[realColumn].Add("NaN");
                            output[cpuColumn].Add("NaN");
                        }
                    }

                    var longestColumnCount = output.Max(x => x.Count);
                    for (int i = 0; i < longestColumnCount; i++)
                    {
                        var line = String.Join(",", output.Select(x => i < x.Count ? x[i] : "NaN"));
                        csvFile.WriteLine(line);
                    }
                }
            }
        }
    }
}
