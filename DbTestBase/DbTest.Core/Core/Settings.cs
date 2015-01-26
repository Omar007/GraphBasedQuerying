using System;
using System.IO;

namespace DbTest.Core
{
    public class Settings
    {
        public ProgramSettings ProgramSettings { get; private set; }
        public TestSettings TestSettings { get; private set; }

        public Settings()
            : this(ProgramSettings.Default, TestSettings.Default)
        {

        }

        public Settings(ProgramSettings progSettings, TestSettings testSettings)
        {
            ProgramSettings = progSettings;
            TestSettings = testSettings;
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", ProgramSettings, TestSettings);
        }

        internal static Settings Parse(string[] args)
        {
            var settings = new Settings();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--create":
                        settings.ProgramSettings.CreateDbs = true;
                        break;
                    case "--recreate":
                        settings.ProgramSettings.CreateDbs = true;
                        settings.ProgramSettings.RecreateDbs = true;
                        break;
                    case "--delete":
                        settings.ProgramSettings.DeleteDbs = true;
                        break;
                    case "--notest":
                        settings.ProgramSettings.SkipTests = true;
                        break;
                    case "--repeat":
                        settings.TestSettings.RepeatCount = Int32.Parse(args[i + 1]);
                        i++;
                        break;
                    case "--substeps":
                        settings.TestSettings.SubSteps = Int32.Parse(args[i + 1]);
                        settings.TestSettings.MinimumRunTime = TimeSpan.Zero;
                        i++;
                        break;
                    case "--minimumruntime":
                        settings.TestSettings.MinimumRunTime = TimeSpan.FromMilliseconds(Int32.Parse(args[i + 1]));
                        settings.TestSettings.SubSteps = 0;
                        i++;
                        break;
                    case "--blankruns":
                        settings.TestSettings.BlankRuns = Int32.Parse(args[i + 1]);
                        i++;
                        break;
                    case "--output":
                        settings.TestSettings.OutputDir = Directory.CreateDirectory(args[i + 1]);
                        i++;
                        break;
                    case "--affinity":
                        settings.ProgramSettings.Affinity = (int)Math.Pow(2, Int32.Parse(args[i + 1]));
                        i++;
                        break;
                }
            }

            return settings;
        }
    }
}
