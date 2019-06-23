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
    /// Represents an operator that takes two operands
    /// </summary>
    public interface IBinaryOperator : IOperator, IComparable<IBinaryOperator>, IEquatable<IBinaryOperator>
    {
        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        OperandPosition EvaulatedOperand { get; }
        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        int Precedence { get; }
    }
}
