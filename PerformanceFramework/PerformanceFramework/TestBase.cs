using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LinqStatistics;

namespace PerformanceFramework
{
    public abstract class TestBase : IEnumerable<TestResults>
    {
        public abstract string Name { get; }

        public int TotalRunsCount { get; private set; }
        public bool HasResults { get { return _testResults.Count > 0; } }

        public TimeSpan Maximum { get { return _testResults.Max(x => x.Duration); } }
        public TimeSpan Minimum { get { return _testResults.Min(x => x.Duration); } }
        public TimeSpan Average { get { return new TimeSpan((long)_testResults.Average(x => x.Duration.Ticks)); } }
        public TimeSpan Median { get { return new TimeSpan((long)_testResults.Median(x => x.Duration.Ticks)); } }
        public TimeSpan StandardDeviation { get { return new TimeSpan((long)_testResults.StandardDeviation(x => x.Duration.Ticks)); } }

        public TimeSpan ThreadMaximum { get { return _testResults.Max(x => x.ThreadDuration); } }
        public TimeSpan ThreadMinimum { get { return _testResults.Min(x => x.ThreadDuration); } }
        public TimeSpan ThreadAverage { get { return new TimeSpan((long)_testResults.Average(x => x.ThreadDuration.Ticks)); } }
        public TimeSpan ThreadMedian { get { return new TimeSpan((long)_testResults.Median(x => x.ThreadDuration.Ticks)); } }
        public TimeSpan ThreadStandardDeviation { get { return new TimeSpan((long)_testResults.StandardDeviation(x => x.ThreadDuration.Ticks)); } }

        internal string LastError { get { return _failedRuns.Last().ErrorMessage; } }

        private readonly ICollection<TestResults> _testResults;
        private readonly ICollection<TestFailure> _failedRuns;

        public TestBase()
        {
            _testResults = new List<TestResults>();
            _failedRuns = new List<TestFailure>();
        }

        protected virtual void Setup() { }
        protected abstract int DoTest();
        protected virtual void Cleanup() { }

        /// <summary>
        /// Runs the test as many times as needed to match the minimumRunTime.
        /// The average of the required runs is used for the result.
        /// </summary>
        /// <param name="ignoreResults"></param>
        /// <param name="minimumRunTime"></param>
        /// <returns></returns>
        internal int Run(bool ignoreResults, TimeSpan minimumRunTime)
        {
            TotalRunsCount++;

            int result = 0;

#if !DEBUG
            try
            {
#endif
                int subSteps = 0;
                var sw = new Stopwatch();
                var esw = new ExecutionStopwatch();
                esw.Start();

                while (esw.Elapsed < minimumRunTime)
                {
                    subSteps++;

                    Setup();

                    sw.Start();
                    result ^= DoTest();
                    sw.Stop();

                    Cleanup();
                }

                esw.Stop();

                if (!ignoreResults)
                {
                    var duration = new TimeSpan(sw.Elapsed.Ticks / subSteps);
                    var threadDuration = new TimeSpan(esw.Elapsed.Ticks / subSteps);
                    _testResults.Add(new TestResults(TotalRunsCount, duration, threadDuration));
                }
#if !DEBUG
            }
            catch (Exception e)
            {
                _failedRuns.Add(new TestFailure(TotalRunsCount, e.Message, e.AggregateInnerExceptionMessages()));
                result = -1;
            }
#endif

            return result;
        }

        /// <summary>
        /// Runs tests x amount of times and takes the average of all the runs as the result.
        /// </summary>
        /// <param name="ignoreResults"></param>
        /// <param name="subSteps"></param>
        /// <returns></returns>
        internal int Run(bool ignoreResults, int subSteps)
        {
            TotalRunsCount++;

            int result = 0;

#if !DEBUG
            try
            {
#endif
                var sw = new Stopwatch();
                var esw = new ExecutionStopwatch();
                esw.Start();

                for (int i = 0; i < subSteps; i++)
                {
                    Setup();

                    sw.Start();
                    result ^= DoTest();
                    sw.Stop();

                    Cleanup();
                }

                esw.Stop();

                if (!ignoreResults)
                {
                    var duration = new TimeSpan(sw.Elapsed.Ticks / subSteps);
                    var threadDuration = new TimeSpan(esw.Elapsed.Ticks / subSteps);
                    _testResults.Add(new TestResults(TotalRunsCount, duration, threadDuration));
                }
#if !DEBUG
            }
            catch (Exception e)
            {
                _failedRuns.Add(new TestFailure(TotalRunsCount, e.Message, e.AggregateInnerExceptionMessages()));
                result = -1;
            }
#endif

            return result;
        }

        internal void Clear()
        {
            _testResults.Clear();
        }

        public IEnumerator<TestResults> GetEnumerator()
        {
            return _testResults.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("#{0} (M{1}|F{2}): Test={3}", TotalRunsCount, _testResults.Count, _failedRuns.Count, Name);

            if (HasResults)
            {
                sb.AppendFormat(", Min={0}, Max={1}, Avg={2}", Minimum, Maximum, Average);
            }

            return sb.ToString();
        }
    }
}
