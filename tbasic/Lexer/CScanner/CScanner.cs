/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slang.Components;

namespace Slang.Lexer
{
    internal class CScanner : DefaultScanner
    {
        public CScanner(StringStream stream) : base(stream)
        {
        }
    }
}
