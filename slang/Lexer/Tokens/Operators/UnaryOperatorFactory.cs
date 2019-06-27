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
    /// Represents an operator that takes one operand
    /// </summary>
    public class UnaryOperatorFactory : IOperatorFactory
    {        
        /// <summary>
        /// Gets whether or not the operand should be evaluated
        /// </summary>
        bool EvaluateOperand { get; }

        public virtual int MatchToken(StringStream stream, out Token token)
        {
            int c = '\0';
            for(int idx = 0; idx < OperatorString.Length && c != -1; ++idx) {
                if (OperatorString[idx] != (c = stream.Read()))
                    return 0;
            }
            return OperatorString.Length;
        }
    }
}
