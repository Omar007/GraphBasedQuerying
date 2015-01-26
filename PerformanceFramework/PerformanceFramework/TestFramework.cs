using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime;
using System.Text;

namespace PerformanceFramework
{
    public class TestFramework : IEnumerable<TestBase>
    {
        public bool IgnoreRunResults { get; set; }
        public IEnumerable<TestBase> Tests { get; set; }

        private readonly TextWriter _log;

        public TestFramework()
            : this(null)
        {
            
        }

        public TestFramework(TextWriter log)
        {
            _log = log;

            _log.WriteLine(TimerProperties().ToString());
            _log.Flush();

            //var currentProcess = Process.GetCurrentProcess();
            //currentProcess.ProcessorAffinity = new IntPtr(2);
            //currentProcess.PriorityClass = ProcessPriorityClass.High;
            //Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        private void WriteRunToLog(int result, TestBase test)
        {
            if (_log != null)
            {
                _log.WriteLine(result);
                _log.WriteLine(test.ToString());

                if (result == -1)
                {
                    _log.WriteLine(String.Format("ERROR: Run {0} of test '{1}' failed!",
                        test.TotalRunsCount, test.Name));
                    _log.WriteLine(test.LastError);
                }

                _log.Flush();
            }
        }

        private void RunGC()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        public void Run(TimeSpan minimumRunTime)
        {
            var oldLatency = GCSettings.LatencyMode;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            foreach (var test in Tests)
            {
                RunGC();
                var result = test.Run(IgnoreRunResults, minimumRunTime);
                RunGC();

                WriteRunToLog(result, test);
            }
            IgnoreRunResults = false;

            GCSettings.LatencyMode = oldLatency;
        }

        public void Run(int subSteps)
        {
            var oldLatency = GCSettings.LatencyMode;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            foreach (var test in Tests)
            {
                RunGC();
                var result = test.Run(IgnoreRunResults, subSteps);
                RunGC();

                WriteRunToLog(result, test);
            }
            IgnoreRunResults = false;

            GCSettings.LatencyMode = oldLatency;
        }

        public void Reset()
        {
            foreach (var test in Tests)
            {
                test.Clear();
            }
        }

        public IEnumerator<TestBase> GetEnumerator()
        {
            return Tests.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static StringBuilder TimerProperties()
        {
            var sb = new StringBuilder();

            if (Stopwatch.IsHighResolution)
            {
                sb.AppendLine("Operations timed using the system's high-resolution performance counter.");
            }
            else
            {
                sb.AppendLine("Operations timed using the DateTime class.");
            }

            long frequency = Stopwatch.Frequency;
            sb.AppendFormat("Timer frequency in ticks per second = {0}", frequency);
            sb.AppendLine();

            long nanosecPerTick = 1000000000L / frequency;
            sb.AppendFormat("Timer is accurate within {0} nanoseconds", nanosecPerTick);

            return sb;
        }
    }
}
