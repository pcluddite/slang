// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text;
using Tbasic.Parsing;
using Tbasic.Runtime;
using Tbasic.Errors;

namespace Tbasic.Terminal
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "TBASIC Terminal";
            Console.WriteLine("T-BASIC Terminal [{0}]", Executer.VERSION);
            Console.WriteLine("Copyright (c) Timothy Baxendale. All Rights Reserved.");
            Console.WriteLine();

            Console.Write("Initializing standard library...");

            Executer exec = new Executer();
            exec.Global.LoadStandardLibrary();

            foreach (var kv in exec.Global.GetAllFunctions()) {
                exec.Global.SetCommand(kv.Key, kv.Value);
            }

            exec.Global.AddCommandLibrary(new ConsoleLibrary(exec));

            Console.WriteLine("Done.\n");

            int curr = 0;
            string line;
            while(!Executer.ExitRequest) {
                Console.Write(">");
                line = Console.ReadLine();
                FuncData dat;
                try {
                    dat = exec.Execute(new Line(curr++, line));
                    Console.WriteLine(ObjectToString(dat.Data));
                }
                catch(TbasicRuntimeException e) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine();
            }
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
    }
}
