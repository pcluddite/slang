/** +++====+++
*  
*  Copyright (c) Timothy Baxendale
*
*  +++====+++
**/
using System.IO;

namespace Tbasic.Lexer
{
    public interface ITokenFactory
    {
        int MatchToken(StreamReader reader, out IToken token);
    }
}
