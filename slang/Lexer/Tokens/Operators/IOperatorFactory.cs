/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;

namespace Slang.Lexer.Tokens
{
    /// <summary>
    /// Builds an operator
    /// </summary>
    public interface IOperatorFactory : ITokenFactory
    {
        /// <summary>
        /// Represents this operator as a string
        /// </summary>
        string OperatorString { get; }
    }
}
