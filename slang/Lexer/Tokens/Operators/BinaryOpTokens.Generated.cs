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
    /// Implementation of the DotOperatorFactory
    /// </summary>
    public partial class DotOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the MultiplicationOperatorFactory
    /// </summary>
    public partial class MultiplicationOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the DivisionOperatorFactory
    /// </summary>
    public partial class DivisionOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the ModuloOperatorFactory
    /// </summary>
    public partial class ModuloOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the AddOperatorFactory
    /// </summary>
    public partial class AddOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the SubtractOperatorFactory
    /// </summary>
    public partial class SubtractOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the ShiftRightOperatorFactory
    /// </summary>
    public partial class ShiftRightOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the ShiftLeftOperatorFactory
    /// </summary>
    public partial class ShiftLeftOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the GreaterThanOperatorFactory
    /// </summary>
    public partial class GreaterThanOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the LessThanOrEqualOperatorFactory
    /// </summary>
    public partial class LessThanOrEqualOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the LessThanOrEqual2OperatorFactory
    /// </summary>
    public partial class LessThanOrEqual2OperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the GreaterThanOrEqualOperatorFactory
    /// </summary>
    public partial class GreaterThanOrEqualOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the GreaterThanOrEqual2OperatorFactory
    /// </summary>
    public partial class GreaterThanOrEqual2OperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the EqualityOperatorFactory
    /// </summary>
    public partial class EqualityOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the SortaEqualsOperatorFactory
    /// </summary>
    public partial class SortaEqualsOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the InequalityOperatorFactory
    /// </summary>
    public partial class InequalityOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the Inequality2OperatorFactory
    /// </summary>
    public partial class Inequality2OperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the BitAndOperatorFactory
    /// </summary>
    public partial class BitAndOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the BitXorOperatorFactory
    /// </summary>
    public partial class BitXorOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the BitOrOperatorFactory
    /// </summary>
    public partial class BitOrOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the AndOperatorFactory
    /// </summary>
    public partial class AndOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the And2OperatorFactory
    /// </summary>
    public partial class And2OperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the OrOperatorFactory
    /// </summary>
    public partial class OrOperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the Or2OperatorFactory
    /// </summary>
    public partial class Or2OperatorFactory : BinaryOperatorFactory
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
    /// Implementation of the SetOperatorFactory
    /// </summary>
    public partial class SetOperatorFactory : BinaryOperatorFactory
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

