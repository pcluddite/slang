﻿/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;

namespace Slang.Lexer.Tokens
{
    /// <summary>
    /// Represents an operator
    /// </summary>
    public interface IOperator : IToken
    {
        /// <summary>
        /// Represents this operator as a string
        /// </summary>
        string OperatorString { get; }
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
