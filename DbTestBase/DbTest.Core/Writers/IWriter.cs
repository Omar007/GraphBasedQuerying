using System.Collections.Generic;
using System.IO;
using PerformanceFramework;

namespace DbTest.Core.Writers
{
    internal interface IWriter
    {
        void Write(DirectoryInfo dir, string filePrefix, IEnumerable<TestBase> tests);
    }
}
