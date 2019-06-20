/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using System.Collections.Generic;

namespace Tbasic.Lexer
{
    public interface IToken
    {
        IEnumerable<char> Text { get; }
        object Native { get; }
    }
}
