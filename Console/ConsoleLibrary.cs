// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.IO;
using Tint.Libraries;
using Tint.Runtime;

namespace Tint.Shell
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
    }
}
