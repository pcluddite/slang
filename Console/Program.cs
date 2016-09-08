// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text;
using TLang.Parsing;
using TLang.Runtime;
using TLang.Errors;

#if DEBUG
using System.Diagnostics;
#endif

namespace TLang.Terminal
{
    internal class Program
    {
        public const ConsoleColor BG_COLOR = ConsoleColor.DarkBlue;
        public const ConsoleColor FG_COLOR = ConsoleColor.White;

        public static void Main(string[] args)
        {
            ResetColor();
            Console.Clear();
            Console.Title = "Terminal";
            Console.WriteLine("T Language Terminal [{0}]", TRuntime.VERSION);
            Console.WriteLine("Copyright (c) Timothy Baxendale. All Rights Reserved.");
            Console.WriteLine();

            Console.Write("Initializing standard library...");

            TRuntime runtime = new TRuntime();
            runtime.Scanner = Scanners.Terminal;
            runtime.Global.LoadStandardLibrary();

            foreach (var kv in runtime.Global.GetAllFunctions()) {
                runtime.Global.SetCommand(kv.Key, kv.Value);
            }

            runtime.Global.AddCommandLibrary(new ConsoleLibrary());

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

        public static void ResetColor()
        {
            Console.ForegroundColor = FG_COLOR;
            Console.BackgroundColor = BG_COLOR;
        }

        private static string ObjectToString(object o)
        {
            if (o == null)
                return "null";

            if (o is string)
                return "\"" + ToCString(o.ToString(), '\"') + "\"";

            if (o.ToString() != o.GetType().ToString()) // checks if ToString() has been implemented
                return o.ToString();

            return JsonConvert.SerializeObject(o, Formatting.Indented); // Maybe json has a way of representing this? (good for arrays)
        }

        internal static string ToCString(string str, char quote)
        {
            StringBuilder sb = new StringBuilder();
            char last = '\0';
            foreach (char cur in str) {
                switch (cur) {
                    case '\\':
                        sb.Append('\\');
                        sb.Append(cur);
                        break;
                    case '\'':
                    case '"':
                        if (quote == cur) {
                            sb.Append('\\');
                        }
                        sb.Append(cur);
                        break;
                    case '/':
                        if (last == '<') {
                            sb.Append('\\');
                        }
                        sb.Append(cur);
                        break;
                    case '\b': sb.Append("\\b"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\r': sb.Append("\\r"); break;
                    default:
                        if (cur < ' ') {
                            sb.Append("\\u" + ((int)cur).ToString("x4", CultureInfo.InvariantCulture));
                        }
                        else {
                            sb.Append(cur);
                        }
                        break;

                }
                last = cur;
            }
            return sb.ToString();
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
