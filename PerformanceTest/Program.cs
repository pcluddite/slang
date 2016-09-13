using System;
using System.IO;
using Tbasic.Runtime;

namespace PerformanceTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TRuntime runtime = new TRuntime();
            runtime.Global.LoadStandardLibrary();
            using (StreamReader reader = new StreamReader(File.OpenRead("D:\\tbasic\\samples\\SpeedTest.tbs"))) {
                runtime.Execute(reader);
            }
        }
    }
}
