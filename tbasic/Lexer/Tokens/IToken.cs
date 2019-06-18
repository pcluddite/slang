/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using System.Collections.Generic;
using System.IO;

namespace Tbasic.Lexer
{
    public interface IToken
    {
        IEnumerable<char> Text { get; }
        int Match(StreamReader reader);
        bool Pack();
    }
}
