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
    /// Implementation of the "property of" operator
    /// </summary>
    public class TBasicDotOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => ".";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 2;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Left;
    }

    /// <summary>
    /// Implementation of the multiplication operator
    /// </summary>
    public class TBasicMultiplicationOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "*";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 5;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the division operator
    /// </summary>
    public class TBasicDivisionOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "/";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 5;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the modulo operator
    /// </summary>
    public class TBasicModuloOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "%";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 5;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the addition operator
    /// </summary>
    public class TBasicAddOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "+";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 6;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the subtraction operator
    /// </summary>
    public class TBasicSubtractOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "-";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 6;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the right shift operator
    /// </summary>
    public class TBasicShiftRightOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => ">>";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 7;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the left shift operator
    /// </summary>
    public class TBasicShiftLeftOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "<<";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 7;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the greater than operator
    /// </summary>
    public class TBasicGreaterThanOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => ">";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 8;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the less than or equal to operator
    /// </summary>
    public class TBasicLessThanOrEqualOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "<=";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 8;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the less than or equal to operator
    /// </summary>
    public class TBasicLessThanOrEqual2OperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "=<";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 8;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the greater than or equal to operator
    /// </summary>
    public class TBasicGreaterThanOrEqualOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => ">=";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 8;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the greater than or equal to operator
    /// </summary>
    public class TBasicGreaterThanOrEqual2OperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "=<";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 8;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the equals operator
    /// </summary>
    public class TBasicEqualityOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "==";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 9;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the loosely equals operator
    /// </summary>
    public class TBasicSortaEqualsOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "~=";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 9;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the not equals operator
    /// </summary>
    public class TBasicInequalityOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "!=";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 9;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the not equals operator
    /// </summary>
    public class TBasicInequality2OperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "<>";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 9;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the bit and operator
    /// </summary>
    public class TBasicBitAndOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "&";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 10;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the bit exclusive or operator
    /// </summary>
    public class TBasicBitXorOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "^";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 11;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the bit or operator
    /// </summary>
    public class TBasicBitOrOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "|";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 12;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the boolean and operator
    /// </summary>
    public class TBasicAndOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "&&";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 13;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the boolean and operator
    /// </summary>
    public class TBasicAnd2OperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "AND";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 13;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the boolean or operator
    /// </summary>
    public class TBasicOrOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "||";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 14;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the boolean or operator
    /// </summary>
    public class TBasicOr2OperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "OR";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 14;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Both;
    }

    /// <summary>
    /// Implementation of the set operator
    /// </summary>
    public class TBasicSetOperatorFactory : TBasicBinaryOperatorFactory
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public override string OperatorString => "=";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public override int Precedence => 16;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public override OperandPosition EvaulatedOperand => OperandPosition.Right;
    }
}

