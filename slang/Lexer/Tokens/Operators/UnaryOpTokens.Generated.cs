/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;

namespace Slang.Lexer.Tokens
{
    /// <summary>
    /// Implementation of the NewOperator
    /// </summary>
    public partial class NewOperator : UnaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "NEW";

        public override bool EvaluateOperand => false;
    }

    /// <summary>
    /// Implementation of the PositiveOperator
    /// </summary>
    public partial class PositiveOperator : UnaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "+";

        public override bool EvaluateOperand => true;
    }

    /// <summary>
    /// Implementation of the NegativeOperator
    /// </summary>
    public partial class NegativeOperator : UnaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "+";

        public override bool EvaluateOperand => true;
    }

    /// <summary>
    /// Implementation of the NotOperator
    /// </summary>
    public partial class NotOperator : UnaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "NOT";

        public override bool EvaluateOperand => true;
    }

    /// <summary>
    /// Implementation of the BitNotOperator
    /// </summary>
    public partial class BitNotOperator : UnaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "~";

        public override bool EvaluateOperand => true;
    }
}

