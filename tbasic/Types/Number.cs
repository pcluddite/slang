// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using Tbasic.Errors;
using Tbasic.Runtime;
using System.Globalization;

namespace Tbasic.Types
{
    /// <summary>
    /// Represents a generic number (this is a double at its core)
    /// </summary>
    public struct Number : IConvertible, IComparable, IComparable<Number>, IComparable<double>, IEquatable<Number>, IEquatable<double>, IRuntimeObject
    {
        /// <summary>
        /// Gets or sets the value this Number represetns
        /// </summary>
        public double Value;
        /// <summary>
        /// Gets the size of the type that represents this number
        /// </summary>
        public const int SIZE = sizeof(double);

        TbasicType IRuntimeObject.TypeCode
        {
            get {
                return TbasicType.Number;
            }
        }

        object IRuntimeObject.Value
        {
            get {
                return Value;
            }
        }

        /// <summary>
        /// Constructs a new number
        /// </summary>
        /// <param name="value"></param>
        public Number(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Determines if this number has a fractional part
        /// </summary>
        /// <returns></returns>
        public bool HasFraction()
        {
            return Value % 1 != 0;
        }

        /// <summary>
        /// Converts this number to an integer
        /// </summary>
        /// <returns></returns>
        public int ToInt()
        {
            return (int)Value;
        }

        /// <summary>
        /// Converts this number to whichever primitive type seems most appropriate (either int or double)
        /// </summary>
        /// <returns></returns>
        public object ToPrimitive()
        {
            if (HasFraction())
                return Value;
            return ToInt();
        }

        /// <summary>
        /// Tries to parse a string as a number
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out Number result)
        {
            double d;
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d)) {
                result = new Number(d);
                return true;
            }
            else {
                result = default(Number);
                return false;
            }
        }

        /// <summary>
        /// Parses a string as a number
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Number Parse(string s)
        {
            return double.Parse(s);
        }

        /// <summary>
        /// Determines if an object can be stored in a Number
        /// </summary>
        /// <param name="o"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static bool IsNumber(IRuntimeObject o, ExecuterOption opts)
        {
            double d;
            return TypeConvert.TryConvert(o, out d, opts);
        }

        /// <summary>
        /// Attempts to convert an object to a Number. Returns null if no conversion is possible.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static Number? AsNumber(IRuntimeObject o, ExecuterOption opts)
        {
            if (o == null) {
                if (opts.HasFlag(ExecuterOption.NullIsZero)) {
                    return 0;
                }
                else {
                    return null;
                }
            }
            double d;
            if (TypeConvert.TryConvert(o, out d, opts)) {
                return d;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Converts an object to a Number
        /// </summary>
        public static Number Convert(IRuntimeObject o, ExecuterOption opts)
        {
            Number? n = AsNumber(o, opts);
            if (n == null)
                throw ThrowHelper.InvalidTypeInExpression(o?.GetType().Name ?? "null", nameof(Number));
            return n.Value;
        }

        /// <summary>
        /// Implicitly converts a double to a Number
        /// </summary>
        /// <param name="d"></param>
        public static implicit operator Number(double d)
        {
            return new Number(d);
        }

        /// <summary>
        /// Implicitly converts a Number to a double
        /// </summary>
        /// <param name="num"></param>
        public static implicit operator double(Number num)
        {
            return num.Value;
        }

        /// <summary>
        /// Implicitly converts a double to an Integer
        /// </summary>
        /// <param name="n"></param>
        public static implicit operator Number(int n)
        {
            return new Number(n);
        }

        /// <summary>
        /// Converts this number to an integer
        /// </summary>
        /// <param name="n"></param>
        public static explicit operator int(Number n)
        {
            if (n.HasFraction())
                throw new InvalidCastException("Number contains a fractional part");
            return (int)n.Value;
        }
        
        /// <summary>
        /// Converts this number to a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (HasFraction())
                return Value.ToString();
            return ((long)Value).ToString();
        }

        #region IComparable

        /// <summary>
        /// Compares this Number with another object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            Number? n = obj as Number?;
            if (n != null)
                return CompareTo(n.Value);

            decimal? d = obj as decimal?;
            if (n != null)
                return CompareTo(n.Value);

            throw new ArgumentException(string.Format("can only compare types {0} or {1}", typeof(Number).Name, typeof(double).Name));
        }

        /// <summary>
        /// Compares this Number to another Number
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Number other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Compares this Number to a double
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(double other)
        {
            return Value.CompareTo(other);
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Determines if this number is equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Number other)
        {
            return Value == other.Value;
        }

        /// <summary>
        /// Determines if this number is equal to a double
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(double other)
        {
            return Value == other;
        }
        
        /// <summary>
        /// Determines if these two objects are equal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Number? n = obj as Number?;
            if (n != null)
                return Equals(n.Value);

            double? d = obj as double?;
            if (d != null)
                return Equals(d.Value);

            return false;
        }

        /// <summary>
        /// Gets the hash code for this number
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion

        #region boolean ops

        /// <summary>
        /// Determines if two numbers are equal
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Number left, Number right)
        {
            return left.Value == right.Value;
        }

        /// <summary>
        /// Determines if two numbers are not equal
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Number left, Number right)
        {
            return left.Value != right.Value;
        }

        /// <summary>
        /// Determines if one number is less than another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(Number left, Number right)
        {
            return left.Value < right.Value;
        }

        /// <summary>
        /// Determines if one number is less than or equal to another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(Number left, Number right)
        {
            return left.Value <= right.Value;
        }

        /// <summary>
        /// Determines if one number is greater than another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(Number left, Number right)
        {
            return left.Value > right.Value;
        }

        /// <summary>
        /// Determines if one number is greater than or equal to another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(Number left, Number right)
        {
            return left.Value >= right.Value;
        }

        #endregion

        #region arithmetic ops

        /// <summary>
        /// addition
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Number operator +(Number left, Number right)
        {
            return new Number(left.Value + right.Value);
        }

        /// <summary>
        /// subtraction
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Number operator -(Number left, Number right)
        {
            return new Number(left.Value - right.Value);
        }

        /// <summary>
        /// multiplication
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Number operator *(Number left, Number right)
        {
            return new Number(left.Value * right.Value);
        }

        /// <summary>
        /// division
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Number operator /(Number left, Number right)
        {
            return new Number(left.Value / right.Value);
        }

        /// <summary>
        /// modulus
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Number operator %(Number left, Number right)
        {
            return new Number(left.Value % right.Value);
        }

        #endregion

        #region IConvertable
        
        TypeCode IConvertible.GetTypeCode()
        {
            return Value.GetTypeCode();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToBoolean(provider);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToChar(provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToSByte(provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToByte(provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt16(provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt16(provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt32(provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt32(provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt64(provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt64(provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToSingle(provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDouble(provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDecimal(provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDateTime(provider);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)Value).ToType(conversionType, provider);
        }

        #endregion
    }
}
