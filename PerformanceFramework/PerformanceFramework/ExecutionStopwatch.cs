using System;
using System.Runtime.InteropServices;

namespace PerformanceFramework
{
    /**
     * Stopwatch that utilizes GetThreadTimes. Minimum precision is 15ms.
     */
    internal class ExecutionStopwatch
    {
        #region Dll Imports
        [DllImport("kernel32.dll")]
        private static extern long GetThreadTimes(IntPtr handle, out long creationTime,
             out long exitTime, out long kernelTime, out long userTime);
        
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();
        #endregion

        public bool IsRunning { get; private set; }

        private long _startThreadTime;
        private long _stopThreadTime;

        public TimeSpan Elapsed
        {
            get
            {
                return TimeSpan.FromMilliseconds(((IsRunning ? GetThreadTimes() : _stopThreadTime) - _startThreadTime) / 10000);
            }
        }

        public ExecutionStopwatch()
        {

        }

        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                _startThreadTime = GetThreadTimes();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _stopThreadTime = GetThreadTimes();
            }
        }

        private long GetThreadTimes()
        {
            long ignore;
            long kernelTime;
            long userTime;

            long returnCode = GetThreadTimes(GetCurrentThread(), out ignore,
                out ignore, out kernelTime, out userTime);

            if (!Convert.ToBoolean(returnCode))
            {
                throw new Exception(String.Format("Failed to get thread times. Error code: {0}",
                    returnCode));
            }

            return kernelTime + userTime;
        }
    }
}
