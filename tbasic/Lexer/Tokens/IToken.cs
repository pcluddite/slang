/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using System.Collections.Generic;

namespace Slang.Lexer.Tokens
{
    public interface IToken
    {
        IEnumerable<char> Text { get; }
        object Native { get; }
    }
}
