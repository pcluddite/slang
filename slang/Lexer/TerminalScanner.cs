/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System.Collections.Generic;

namespace Slang.Lexer
{
    internal class TerminalScanner : DefaultScanner
    {
        public TerminalScanner(string buffer)
            : base(buffer)
        {
        }
    }
}
