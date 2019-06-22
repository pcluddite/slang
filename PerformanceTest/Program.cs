/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.IO;
using Slang.Runtime;

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
            Executor runtime = new Executor();
            runtime.Global.LoadStandardLibrary();
            using (StreamReader reader = new StreamReader(File.OpenRead("D:\\tbasic\\samples\\SpeedTest.tbs"))) {
                runtime.Execute(reader);
            }
        }
    }
}
