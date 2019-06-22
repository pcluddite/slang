/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using Tbasic.Components;

namespace Tbasic.Lexer
{
    public interface ITokenFactory
    {
        int MatchToken(StringStream reader, out IToken token);
    }
}
