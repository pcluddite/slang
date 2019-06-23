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
    public partial struct NewOperator : IUnaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => "NEW";

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public object Native => throw new NotImplementedException();

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "NEW";

        public bool EvaluateOperand => false;

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IUnaryOperator other)
        {
            return OperatorString == other.OperatorString && EvaluateOperand == other.EvaluateOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            NewOperator? op = obj as NewOperator?;
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
            return OperatorString.GetHashCode() ^ EvaluateOperand.GetHashCode();
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
    /// Implementation of the PositiveOperator
    /// </summary>
    public partial struct PositiveOperator : IUnaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => "+";

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public object Native => throw new NotImplementedException();

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "+";

        public bool EvaluateOperand => true;

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IUnaryOperator other)
        {
            return OperatorString == other.OperatorString && EvaluateOperand == other.EvaluateOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            PositiveOperator? op = obj as PositiveOperator?;
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
            return OperatorString.GetHashCode() ^ EvaluateOperand.GetHashCode();
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
    /// Implementation of the NegativeOperator
    /// </summary>
    public partial struct NegativeOperator : IUnaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => "+";

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public object Native => throw new NotImplementedException();

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "+";

        public bool EvaluateOperand => true;

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IUnaryOperator other)
        {
            return OperatorString == other.OperatorString && EvaluateOperand == other.EvaluateOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            NegativeOperator? op = obj as NegativeOperator?;
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
            return OperatorString.GetHashCode() ^ EvaluateOperand.GetHashCode();
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
    /// Implementation of the NotOperator
    /// </summary>
    public partial struct NotOperator : IUnaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => "NOT";

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public object Native => throw new NotImplementedException();

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "NOT";

        public bool EvaluateOperand => true;

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IUnaryOperator other)
        {
            return OperatorString == other.OperatorString && EvaluateOperand == other.EvaluateOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            NotOperator? op = obj as NotOperator?;
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
            return OperatorString.GetHashCode() ^ EvaluateOperand.GetHashCode();
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
    /// Implementation of the BitNotOperator
    /// </summary>
    public partial struct BitNotOperator : IUnaryOperator
    {
        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public IEnumerable<char> Text => "~";

        /// <summary>
        /// Throws a NotImplementedException
        /// </summary>
        public object Native => throw new NotImplementedException();

        /// <summary>
        /// Gets the string representation of the operator
        /// </summary>
        public string OperatorString => "~";

        public bool EvaluateOperand => true;

        /// <summary>
        /// Determines if this operator is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IUnaryOperator other)
        {
            return OperatorString == other.OperatorString && EvaluateOperand == other.EvaluateOperand;
        }

        /// <summary>
        /// Determines if this object is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            BitNotOperator? op = obj as BitNotOperator?;
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
            return OperatorString.GetHashCode() ^ EvaluateOperand.GetHashCode();
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

