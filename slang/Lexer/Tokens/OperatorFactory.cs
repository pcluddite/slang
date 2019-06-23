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
using Slang.Runtime;

namespace Slang.Lexer.Tokens
{
    public class OperatorFactory : ITokenFactory
    {
        public int MatchToken(StringStream stream, Scope scope, out IToken token)
        {
        }
    }
}
