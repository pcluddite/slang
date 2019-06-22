/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using Slang.Components;
using Slang.Runtime;

namespace Slang.Lexer.Tokens
{
    /// <summary>
    /// Token factories must implement a parameterless constructor
    /// </summary>
    public interface ITokenFactory
    {
        int MatchToken(StringStream stream, Scope scope, out IToken token);
    }
}
