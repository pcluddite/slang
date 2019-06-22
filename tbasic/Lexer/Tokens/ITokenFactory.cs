/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using Slang.Components;

namespace Slang.Lexer.Tokens
{
    public interface ITokenFactory
    {
        int MatchToken(StringStream stream, out IToken token);
    }
}
