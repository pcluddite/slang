/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using Slang.Components;

namespace Slang.Lexer.Tokens
{
    /// <summary>
    /// Token factories must implement a parameterless constructor
    /// </summary>
    public interface ITokenFactory
    {
        int MatchToken(StringStream stream, out Token token);
    }
}
