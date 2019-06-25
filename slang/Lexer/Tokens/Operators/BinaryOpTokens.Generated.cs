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
    /// Implementation of the DotOperator
    /// </summary>
    public partial struct DotOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => ".";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 2;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Left;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            DotOperator? op = obj as DotOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the MultiplicationOperator
    /// </summary>
    public partial struct MultiplicationOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "*";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 5;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            MultiplicationOperator? op = obj as MultiplicationOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the DivisionOperator
    /// </summary>
    public partial struct DivisionOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "/";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 5;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            DivisionOperator? op = obj as DivisionOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the ModuloOperator
    /// </summary>
    public partial struct ModuloOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "%";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 5;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ModuloOperator? op = obj as ModuloOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the AddOperator
    /// </summary>
    public partial struct AddOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "+";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 6;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            AddOperator? op = obj as AddOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the SubtractOperator
    /// </summary>
    public partial struct SubtractOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "-";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 6;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            SubtractOperator? op = obj as SubtractOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the ShiftRightOperator
    /// </summary>
    public partial struct ShiftRightOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => ">>";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 7;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ShiftRightOperator? op = obj as ShiftRightOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the ShiftLeftOperator
    /// </summary>
    public partial struct ShiftLeftOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "<<";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 7;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ShiftLeftOperator? op = obj as ShiftLeftOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the GreaterThanOperator
    /// </summary>
    public partial struct GreaterThanOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => ">";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 8;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            GreaterThanOperator? op = obj as GreaterThanOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the LessThanOrEqualOperator
    /// </summary>
    public partial struct LessThanOrEqualOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "<=";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 8;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            LessThanOrEqualOperator? op = obj as LessThanOrEqualOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the LessThanOrEqual2Operator
    /// </summary>
    public partial struct LessThanOrEqual2Operator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "=<";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 8;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            LessThanOrEqual2Operator? op = obj as LessThanOrEqual2Operator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the GreaterThanOrEqualOperator
    /// </summary>
    public partial struct GreaterThanOrEqualOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => ">=";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 8;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            GreaterThanOrEqualOperator? op = obj as GreaterThanOrEqualOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the GreaterThanOrEqual2Operator
    /// </summary>
    public partial struct GreaterThanOrEqual2Operator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "=<";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 8;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            GreaterThanOrEqual2Operator? op = obj as GreaterThanOrEqual2Operator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the EqualityOperator
    /// </summary>
    public partial struct EqualityOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "==";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 9;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            EqualityOperator? op = obj as EqualityOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the SortaEqualsOperator
    /// </summary>
    public partial struct SortaEqualsOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "~=";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 9;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            SortaEqualsOperator? op = obj as SortaEqualsOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the InequalityOperator
    /// </summary>
    public partial struct InequalityOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "!=";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 9;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            InequalityOperator? op = obj as InequalityOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the Inequality2Operator
    /// </summary>
    public partial struct Inequality2Operator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "<>";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 9;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Inequality2Operator? op = obj as Inequality2Operator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the BitAndOperator
    /// </summary>
    public partial struct BitAndOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "&";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 10;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            BitAndOperator? op = obj as BitAndOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the BitXorOperator
    /// </summary>
    public partial struct BitXorOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "^";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 11;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            BitXorOperator? op = obj as BitXorOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the BitOrOperator
    /// </summary>
    public partial struct BitOrOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "|";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 12;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            BitOrOperator? op = obj as BitOrOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the AndOperator
    /// </summary>
    public partial struct AndOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "&&";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 13;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            AndOperator? op = obj as AndOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the And2Operator
    /// </summary>
    public partial struct And2Operator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "AND";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 13;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            And2Operator? op = obj as And2Operator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the OrOperator
    /// </summary>
    public partial struct OrOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "||";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 14;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            OrOperator? op = obj as OrOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the Or2Operator
    /// </summary>
    public partial struct Or2Operator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "OR";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 14;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Both;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Or2Operator? op = obj as Or2Operator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }

    /// <summary>
    /// Implementation of the SetOperator
    /// </summary>
    public partial struct SetOperator : IBinaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => OperatorString;

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public IEnumerable<IToken> Subtokens => throw new NotImplementedException();

        /// <summary>
        /// Returns false
        /// </summary>
        public bool HasSubtokens => false;

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "=";

        /// <summary>
        /// Gets the operator precedence. Lower precedence operators are processed first
        /// </summary>
        public int Precedence => 16;

        /// <summary>
        /// Gets which operand should be evaluated
        /// </summary>
        public OperandPosition EvaulatedOperand => OperandPosition.Right;

        /// <summary>
        /// Compares this operator to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(IBinaryOperator other)
        {
            return Precedence.CompareTo(other.Precedence);
        }

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IBinaryOperator other)
        {
            return OperatorString == other.OperatorString && Precedence == other.Precedence && EvaulatedOperand == other.EvaulatedOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            SetOperator? op = obj as SetOperator?;
            if (op != null)
                return Equals(op.Value);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return OperatorString.GetHashCode() ^ Precedence ^ EvaulatedOperand.GetHashCode();
        }

        /// <summary>
        /// Converts this operator to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperatorString;
        }
    }
}

