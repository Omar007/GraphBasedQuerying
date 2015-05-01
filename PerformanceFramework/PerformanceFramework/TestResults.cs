using System;

namespace PerformanceFramework
{
    public class TestResults
    {
        public int RunNumber { get; private set; }
        public TimeSpan Duration { get; private set; }
        public TimeSpan ThreadDuration { get; private set; }

        public TimeSpan IdleTime { get { return Duration.Subtract(ThreadDuration); } }

        internal TestResults(int runNr, TimeSpan duration, TimeSpan threadDuration)
        {
            RunNumber = runNr;
            Duration = duration;
            ThreadDuration = threadDuration;
        }
    }
}
