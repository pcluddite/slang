using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tbasic.Runtime;
using Tbasic.Libraries;

namespace Tbasic.Terminal
{
    public class ConsoleLibrary : Library
    {
        private Executer _exec;

        public ConsoleLibrary(Executer exec)
        {
            _exec = exec;
            Add("CLS", ClearScreen);
            Add("CLEAR", ClearScreen);
            Add("STOP", Exit);
        }

        public object ClearScreen(RuntimeData fData)
        {
            Console.Clear();
            return string.Empty;
        }

        public object Exit(RuntimeData fData)
        {
            fData.StackExecuter.RequestExit();
            return null;
        }
    }
}
