/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System.Collections.Generic;

namespace Tbasic.Lexer
{
    internal class TerminalScanner : DefaultScanner
    {
        public TerminalScanner(string buffer)
            : base(buffer)
        {
        }
    }
}
