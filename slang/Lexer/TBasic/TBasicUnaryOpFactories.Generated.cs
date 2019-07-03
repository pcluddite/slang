/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Collections.Generic;

namespace Slang.Lexer.TBasic
{
    /// <summary>
    /// Implementation of the new operator
    /// </summary>
    public class TBasicNewOperator : UnaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "NEW";

        public override bool EvaluateOperand => false;
    }

    /// <summary>
    /// Implementation of the positive operator
    /// </summary>
    public class TBasicPositiveOperator : UnaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "+";

        public override bool EvaluateOperand => true;
    }

    /// <summary>
    /// Implementation of the negative operator
    /// </summary>
    public class TBasicNegativeOperator : UnaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "-";

        public override bool EvaluateOperand => true;
    }

    /// <summary>
    /// Implementation of the boolean not operator
    /// </summary>
    public class TBasicNotOperator : UnaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "NOT";

        public override bool EvaluateOperand => true;
    }

    /// <summary>
    /// Implementation of the inverse bit operator
    /// </summary>
    public class TBasicBitNotOperator : UnaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "~";

        public override bool EvaluateOperand => true;
    }
}

