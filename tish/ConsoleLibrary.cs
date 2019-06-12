/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;
using System.IO;
using Tbasic.Libraries;
using Tbasic.Runtime;
using Tbasic.Tbasic;

namespace Tbasic.Shell
{
    public class ConsoleLibrary : Library
    {
        public ConsoleLibrary()
        {
            Add("CLS", Console.Clear);
            Add("CLEAR", Console.Clear);
            Add("STOP", Exit); // exit is already declared in stdlib 8/22/16
            Add("HELLO", Hello);
            Add("HI", Hello);
            Add("HEY", Hello);
            Add<string>("tscript", tscript, requiredArgs: 0);
            Add<string, IEnumerable<string>>("LS", Ls, requiredArgs: 0);
        }

        public static IEnumerable<string> Ls(string directory)
        {
            if (directory == null) {
                directory = Directory.GetCurrentDirectory();
            }
            foreach(string dir in Directory.GetDirectories(directory)) {
                yield return $".\\{Path.GetFileName(dir)}";
            }
            foreach (string dir in Directory.GetFiles(directory)) {
                yield return Path.GetFileName(dir);
            }
        }

        public static object Exit(TRuntime runtime, StackData stackdat)
        {
            runtime.RequestExit();
            return null;
        }

        public static object Hello(TRuntime runtime, StackData stackdat)
        {
            return "Hey";
        }

        public static void tscript(string path)
        {
            if (path == null && (path = ScriptHost.PromptOpenScript()) == null)
                return;

            ScriptHost.RunScript(path);
        }
    }
}
