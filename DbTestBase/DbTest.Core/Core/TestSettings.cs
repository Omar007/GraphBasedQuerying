using System;
using System.IO;

namespace DbTest.Core
{
    public class TestSettings
    {
        public static readonly TestSettings Default = new TestSettings(5, 2, 1, TimeSpan.Zero, Directory.CreateDirectory("./"));

        public int RepeatCount { get; set; }
        public int SubSteps { get; set; }
        public int BlankRuns { get; set; }
        public TimeSpan MinimumRunTime { get; set; }
        public DirectoryInfo OutputDir { get; set; }

        public TestSettings(int repeatCount, int subSteps, int blankRuns, TimeSpan minimumRuntime, DirectoryInfo outputDir)
        {
            RepeatCount = repeatCount;
            SubSteps = subSteps;
            BlankRuns = blankRuns;
            MinimumRunTime = minimumRuntime;
            OutputDir = outputDir;
        }

        public override string ToString()
        {
            return String.Format("RepeatCount={0}, SubSteps={1}, BlankRuns={2}, MinimumRunTime={3}, OutputDir={4}",
                RepeatCount, SubSteps, BlankRuns, MinimumRunTime, OutputDir.FullName);
        }
    }
}
