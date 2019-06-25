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
        IEnumerable<IToken> Subtokens { get; }
        bool HasSubtokens { get; }
        IEnumerable<char> Text { get; }
    }
}
