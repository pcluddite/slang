﻿using System;
using Tbasic.Libraries;
using Tbasic.Runtime;

namespace Tbasic.Terminal
{
    public class ConsoleLibrary : Library
    {
        private TBasic _exec;

        public ConsoleLibrary(TBasic exec)
        {
            _exec = exec;
            Add("CLS", Console.Clear);
            Add("CLEAR", Console.Clear);
            Add("STOP", Exit); // exit is already declared in stdlib 8/22/16
        }

        public object Exit(RuntimeData fData)
        {
            fData.StackExecuter.RequestExit();
            return null;
        }
    }
}
