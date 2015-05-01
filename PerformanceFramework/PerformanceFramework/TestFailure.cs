using System.Collections.Generic;

namespace PerformanceFramework
{
    public class TestFailure
    {
        public int RunNumber { get; private set; }
        public string ErrorMessage { get; private set; }
        public IEnumerable<string> InnerErrorMessages { get; private set; }

        internal TestFailure(int runNumber, string errorMsg, IEnumerable<string> innerErrorMsgs)
        {
            RunNumber = runNumber;
            ErrorMessage = errorMsg;
            InnerErrorMessages = innerErrorMsgs;
        }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}
