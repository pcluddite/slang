/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using Newtonsoft.Json;
using System;
using Tbasic.Lexer;
using Tbasic.Runtime;
using Tbasic.Errors;
using Tbasic.Types;

#if DEBUG
using System.Diagnostics;
#endif

namespace Tbasic.Shell
{
    internal class Program
    {
        public const ConsoleColor BG_COLOR = ConsoleColor.DarkBlue;
        public const ConsoleColor FG_COLOR = ConsoleColor.White;

        public static void Main(string[] args)
        {
            ResetColor();
            Console.Clear();
            Console.Title = "T Interpreter Shell";
            Console.WriteLine("T Interpreter Shell [{0}]", TRuntime.VERSION);
            Console.WriteLine("Copyright (c) Timothy Baxendale. All Rights Reserved.");
            Console.WriteLine();

            Console.Write("Initializing standard library...");
            TRuntime runtime = InitRuntime();

            Console.WriteLine("Done.\n");

            int curr = 0;
            string line;
            while(!TRuntime.ExitRequest) {
                Console.Write(">");
                line = Console.ReadLine();
                StackData dat;
                try {
#if DEBUG
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
#endif
                    dat = runtime.Execute(new Line(curr++, line));
#if DEBUG
                    watch.Stop();
                    WriteTime(watch.ElapsedMilliseconds);
#endif
                    if (dat.ReturnValue != null)
                        Console.WriteLine(ObjectToString(dat.ReturnValue));
                }
                catch(TbasicRuntimeException e) {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    ResetColor();
                }
                Console.WriteLine();
            }
        }

        internal static TRuntime InitRuntime()
        {
            TRuntime runtime = new TRuntime();
            runtime.Scanner = Scanners.Terminal;
            runtime.Preprocessor = Preprocessors.Terminal;
            runtime.Global.LoadStandardLibrary();
            runtime.Global.AddFunctionAlias("GetCurrentDirectory", "PWD");

            CallData cd = runtime.Global.GetFunction("SetCurrentDirectory");
            cd.ShouldEvaluate = false; // this way we don't have to quote everything all the time
            runtime.Global.AddFunction("CD", cd);

            runtime.Global.AddCommandLibrary(new ConsoleLibrary());
            return runtime;
        }

        public static void ResetColor()
        {
            Console.ForegroundColor = FG_COLOR;
            Console.BackgroundColor = BG_COLOR;
        }

        private static string ObjectToString(object o)
        {
            if (o == null)
                return "null";
            
            if (o.ToString() != o.GetType().ToString()) // checks if ToString() has been implemented
                return o.ToString();
            JsonSerializerSettings n = new JsonSerializerSettings();
            return JsonConvert.SerializeObject(o, Formatting.Indented); // Maybe json has a way of representing this? (good for arrays)
        }
        
#if DEBUG
        private static void WriteTime(long elapsedMils)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Elapsed: {0}", elapsedMils);
            ResetColor();
        }
#endif

    }
}
