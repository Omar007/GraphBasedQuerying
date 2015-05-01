using System;
using System.Collections.Generic;

namespace PerformanceFramework
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<string> AggregateInnerExceptionMessages(this Exception e)
        {
            for (var innerException = e.InnerException; innerException != null; innerException = innerException.InnerException)
            {
                yield return innerException.Message;
            }
        }
    }
}
