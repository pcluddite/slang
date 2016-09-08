// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.IO;
using Tbasic.Libraries;
using Tbasic.Runtime;

namespace Tbasic.Terminal
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
            Add<string, IEnumerable<string>>("LS", Ls);
        }

        public static IEnumerable<string> Ls(string directory)
        {
            if (directory == null) {
                directory = Directory.GetCurrentDirectory();
            }
            foreach(string dir in Directory.GetDirectories(directory)) {
                yield return dir;
            }
            foreach (string dir in Directory.GetFiles(directory)) {
                yield return dir;
            }
        }

        public static object Exit(StackData stackdat)
        {
            stackdat.Runtime.RequestExit();
            return null;
        }

        public static object Hello(StackData stackdat)
        {
            return "Hey";
        }
    }
}
