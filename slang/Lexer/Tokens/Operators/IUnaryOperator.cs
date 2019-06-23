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
    public interface IUnaryOperator : IOperator, IEquatable<IUnaryOperator>
    {        
        /// <summary>
        /// Gets whether or not the operand should be evaluated
        /// </summary>
        bool EvaluateOperand { get; }
}
