using System;

namespace DbTest.Core
{
    public class ProgramSettings
    {
        public static readonly ProgramSettings Default = new ProgramSettings(true, false, false, false, 1);

        public bool CreateDbs { get; set; }
        public bool RecreateDbs { get; set; }
        public bool DeleteDbs { get; set; }
        public bool SkipTests { get; set; }
        public int Affinity { get; set; }
        
        public ProgramSettings(bool createDbs, bool recreateDbs, bool deleteDbs, bool skipTests, int affinity)
        {
            CreateDbs = createDbs;
            RecreateDbs = recreateDbs;
            DeleteDbs = deleteDbs;
            SkipTests = skipTests;
            Affinity = affinity;
        }

        public override string ToString()
        {
            return String.Format("CreateDbs={0}, RecreateDbs={1}, DeleteDbs={2}, SkipTests={3}, Affinity={4}",
                CreateDbs, RecreateDbs, DeleteDbs, SkipTests, Affinity);
        }
    }
}
