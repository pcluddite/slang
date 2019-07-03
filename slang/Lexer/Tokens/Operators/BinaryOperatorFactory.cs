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
    /// Represents an operator that takes two operands
    /// </summary>
    public abstract class BinaryOperatorFactory : IOperatorFactory
    {
        public abstract string OperatorString { get; }

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public abstract OperandPosition EvaulatedOperand { get; }

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public abstract int Precedence { get; }

        public virtual int MatchToken(StringStream stream, out Token token)
        {
            int c = '\0';
            token = default;
            for(int idx = 0; idx < OperatorString.Length && c != -1; ++idx) {
                if (OperatorString[idx] != (c = stream.Read()))
                    return 0;
            }
            token = new Token(this, OperatorString, TokenType.BinaryOperator);
            return OperatorString.Length;
        }
    }

    /// <summary>
    /// Operand positions
    /// </summary>
    [Flags]
    public enum OperandPosition
    {
        /// <summary>
        /// Neither operand
        /// </summary>
        Neither = 0x00,
        /// <summary>
        /// The left operand
        /// </summary>
        Left = 0x01,
        /// <summary>
        /// The right operand
        /// </summary>
        Right = 0x02,
        /// <summary>
        /// Both operands
        /// </summary>
        Both = Left | Right,
    }
}
