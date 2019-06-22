/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using Tbasic.Components;

namespace Tbasic.Lexer.Tokens
{
    public interface ITokenFactory
    {
        int MatchToken(StringStream reader, out IToken token);
    }
}
