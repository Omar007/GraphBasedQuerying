using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace DbTest.Core
{
    public abstract class ProgramRunner
    {
        public Settings Settings { get; private set; }

        public ProgramRunner(string[] args)
            : this(Settings.Parse(args))
        {

        }

        public ProgramRunner(Settings settings)
        {
            Settings = settings;
        }

        public void Run(IDatabaseConnection connection, IEnumerable<Population> populations)
        {
            //Prevent process from changing cores
            var currentProcess = Process.GetCurrentProcess();
            currentProcess.ProcessorAffinity = new IntPtr(Settings.ProgramSettings.Affinity);
            currentProcess.PriorityClass = ProcessPriorityClass.High;
            //Prevent normal threads from blocking this thread
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            //Set culture to invariant so number output etc is always the same
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine("Running with the following settings:");
            Console.WriteLine(Settings.ToString());

            var dbGen = new DbGenerator(connection, populations);

            if (Settings.ProgramSettings.CreateDbs)
            {
                Console.WriteLine("Creating Databases...");
                dbGen.CreateDatabases(Settings.ProgramSettings.RecreateDbs);
            }

            if (!Settings.ProgramSettings.SkipTests)
            {
                Console.WriteLine("Testing Databases...");
                var tester = new DbTester(connection, populations, Settings.TestSettings);
                tester.RunTests();
            }

            if (Settings.ProgramSettings.DeleteDbs)
            {
                Console.WriteLine("Deleting Databases...");
                dbGen.DeleteDatabases();
            }

            Console.WriteLine("DONE!");
        }
    }
}
