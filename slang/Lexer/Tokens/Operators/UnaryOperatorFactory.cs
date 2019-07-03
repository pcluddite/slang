/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using Slang.Components;
using System;

namespace Slang.Lexer.Tokens
{
    /// <summary>
    /// Represents an operator that takes one operand
    /// </summary>
    public abstract class UnaryOperatorFactory : IOperatorFactory
    {
        /// <summary>
        /// Represents this operator as a string
        /// </summary>
        public abstract string OperatorString { get; }

        /// <summary>
        /// Gets whether or not the operand should be evaluated
        /// </summary>
        public abstract bool EvaluateOperand { get; }

        public virtual int MatchToken(StringStream stream, out Token token)
        {
            int c = '\0';
            token = default;
            for(int idx = 0; idx < OperatorString.Length && c != -1; ++idx) {
                if (OperatorString[idx] != (c = stream.Read()))
                    return 0;
            }
            token = new Token(this, OperatorString, TokenType.UnaryOperator);
            return OperatorString.Length;
        }
    }
}
