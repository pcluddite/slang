﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using Tbasic.Runtime;

namespace Tbasic.Types
{
    // Autogenerated. Do not modify this file.

    /// <summary>
    /// Represents a number that can be used in operations quickly at the cost of precision
    /// </summary>
	#region FastNumber
    public struct FastNumber : INumber, IComparable<FastNumber>, IComparable<double>, IEquatable<FastNumber>, IEquatable<double>
    {
        /// <summary>
        /// Gets or sets the value this FastNumber represents
        /// </summary>
        public double Value;
        /// <summary>
        /// Gets the size of the type that represents this number
        /// </summary>
        public const int SIZE = sizeof(double);
        
        /// <summary>
        /// Constructs a new number
        /// </summary>
        public FastNumber(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Determines if this number has a fractional part
        /// </summary>
        public bool HasFraction()
        {
            return Value % 1 != 0;
        }

		/// <summary>
		/// Gets a bool indicating whether or not the value of this number is 0
		/// </summary>
		public bool IsZero()
		{
			return Value == 0;
		}

		/// <summary>
		/// Converts this FastNumber to an int.
		/// </summary>
		/// <exception cref="ArgumentException">the value contains a fractional part</exception>
		/// <exception cref="OverflowException">the value was too large to store in an int</exception>
		public int ForceToInt()
		{
			if (HasFraction())
				throw new ArgumentException("Number contains a fractional part and cannot be used as an int");
			return (int)Value;
		}

        /// <summary>
        /// Converts this number to whichever primitive type seems most appropriate (either an integer type or a double)
        /// </summary>
        public object ToPrimitive()
        {
			if (HasFraction()) {
				if (Value <= int.MaxValue) {
					return (int)Value;
				}
				else {
					return (long)Value;
				}
			}
            return Value;
        }

        /// <summary>
        /// Tries to parse a string as a number
        /// </summary>
        public static bool TryParse(string s, out FastNumber result)
        {
            double d;
            if (double.TryParse(s, out d)) {
                result = new FastNumber(d);
                return true;
            }
            else {
                result = default(FastNumber);
                return false;
            }
        }

        /// <summary>
        /// Parses a string as a number
        /// </summary>
        public static FastNumber Parse(string s)
        {
            return double.Parse(s);
        }

        /// <summary>
        /// Determines if an object can be stored in a FastNumber
        /// </summary>
        public static bool IsFastNumber(object o, ExecuterOption opts)
        {
            double d;
            return TypeConvert.TryConvert(o, out d, opts);
        }

        /// <summary>
        /// Attempts to convert an object to a FastNumber. Returns null if no conversion is possible.
        /// </summary>
        public static FastNumber? AsFastNumber(object o, ExecuterOption opts)
        {
            double d;
            if (TypeConvert.TryConvert(o, out d, opts)) {
                return new FastNumber(d);
            }
            else {
                return null;
            }
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
        /// Compares this FastNumber with another object
        /// </summary>
        public int CompareTo(object obj)
        {
            FastNumber? n = obj as FastNumber?;
            if (n != null)
                return CompareTo(n);

			double? primitive = obj as double?;
            if (primitive != null)
                return CompareTo(primitive);

			INumber inum = obj as INumber;
			if (inum != null)
				return CompareTo(inum);

            throw new ArgumentException(string.Format("can only compare types {0} or {1}", typeof(FastNumber).Name, typeof(double).Name));
        }

        /// <summary>
        /// Compares this FastNumber to another FastNumber
        /// </summary>
        public int CompareTo(FastNumber other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Compares this FastNumber to a double
        /// </summary>
        public int CompareTo(double other)
        {
            return Value.CompareTo(other);
        }

        /// <summary>
        /// Compares this FastNumber to another INumber
        /// </summary>
        public int CompareTo(INumber other)
        {
            return Value.CompareTo((double)Convert.ChangeType(other, typeof(double)));
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Determines if this number is equal to another
        /// </summary>
        public bool Equals(FastNumber other)
        {
            return Value == other.Value;
        }

        /// <summary>
        /// Determines if this number is equal to a double
        /// </summary>
        public bool Equals(double other)
        {
            return Value == other;
        }

		/// <summary>
        /// Determines if this number is equal to another INumber
        /// </summary>
        public bool Equals(INumber other)
        {
            return Value == (double)Convert.ChangeType(other, typeof(double));
        }
        
        /// <summary>
        /// Determines if these two objects are equal
        /// </summary>
        public override bool Equals(object obj)
        {
            FastNumber? n = obj as FastNumber?;
            if (n != null)
                return Equals(n.Value);

            double? primitive = obj as double?;
            if (primitive != null)
                return Equals(primitive);

			INumber inum = obj as INumber;
			if (inum != null)
				return Equals(inum);

            return base.Equals(obj);
        }

        /// <summary>
        /// Gets the hash code for this number
        /// </summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion

        #region IConvertable
        
        TypeCode IConvertible.GetTypeCode()
        {
            return Value.GetTypeCode();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)Value).ToType(conversionType, provider);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        Boolean IConvertible.ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToBoolean(provider);
        }

        Char IConvertible.ToChar(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToChar(provider);
        }

        SByte IConvertible.ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToSByte(provider);
        }

        Byte IConvertible.ToByte(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToByte(provider);
        }

        Int16 IConvertible.ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt16(provider);
        }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt16(provider);
        }

        Int32 IConvertible.ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt32(provider);
        }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt32(provider);
        }

        Int64 IConvertible.ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt64(provider);
        }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt64(provider);
        }

        Single IConvertible.ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToSingle(provider);
        }

        Double IConvertible.ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDouble(provider);
        }

        Decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDecimal(provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDateTime(provider);
        }

        #endregion

        #region Comparison operators

		/// <summary>
        /// Determines if one number is equal to another
        /// </summary>
        public static bool operator ==(FastNumber left, FastNumber right)
        {
            return left.Value == right.Value;
        }

		/// <summary>
        /// Determines if one number is not equal to another
        /// </summary>
        public static bool operator !=(FastNumber left, FastNumber right)
        {
            return left.Value != right.Value;
        }

		/// <summary>
        /// Determines if one number is greater than another
        /// </summary>
        public static bool operator >(FastNumber left, FastNumber right)
        {
            return left.Value > right.Value;
        }

		bool INumber.GreaterThan(INumber operand)
		{
			return Value > Convert.ToDouble(operand);
		}

		/// <summary>
        /// Determines if one number is less than another
        /// </summary>
        public static bool operator <(FastNumber left, FastNumber right)
        {
            return left.Value < right.Value;
        }

		bool INumber.LessThan(INumber operand)
		{
			return Value < Convert.ToDouble(operand);
		}

		/// <summary>
        /// Determines if one number is greater than or equal to another
        /// </summary>
        public static bool operator >=(FastNumber left, FastNumber right)
        {
            return left.Value >= right.Value;
        }

		bool INumber.GreaterThanOrEqualTo(INumber operand)
		{
			return Value >= Convert.ToDouble(operand);
		}

		/// <summary>
        /// Determines if one number is less than or equal to another
        /// </summary>
        public static bool operator <=(FastNumber left, FastNumber right)
        {
            return left.Value <= right.Value;
        }

		bool INumber.LessThanOrEqualTo(INumber operand)
		{
			return Value <= Convert.ToDouble(operand);
		}

        #endregion

        #region Arithemetic operators

		/// <summary>
        /// Adds one FastNumber with another
        /// </summary>
        public static FastNumber operator +(FastNumber left, FastNumber right)
        {
            return new FastNumber(left.Value + right.Value);
        }

		/// <summary>
        /// Adds a FastNumber with a double
        /// </summary>
        public static FastNumber operator +(FastNumber left, double right)
        {
            return new FastNumber(left.Value + right);
        }

		/// <summary>
        /// Adds a FastNumber with a double
        /// </summary>
        public static FastNumber operator +(double left, FastNumber right)
        {
            return new FastNumber(left + right.Value);
        }

		INumber INumber.Add(INumber operand)
		{
			return new FastNumber(Value + Convert.ToDouble(operand));
		}

		/// <summary>
        /// Subtracts one FastNumber with another
        /// </summary>
        public static FastNumber operator -(FastNumber left, FastNumber right)
        {
            return new FastNumber(left.Value - right.Value);
        }

		/// <summary>
        /// Subtracts a FastNumber with a double
        /// </summary>
        public static FastNumber operator -(FastNumber left, double right)
        {
            return new FastNumber(left.Value - right);
        }

		/// <summary>
        /// Subtracts a FastNumber with a double
        /// </summary>
        public static FastNumber operator -(double left, FastNumber right)
        {
            return new FastNumber(left - right.Value);
        }

		INumber INumber.Subtract(INumber operand)
		{
			return new FastNumber(Value - Convert.ToDouble(operand));
		}

		/// <summary>
        /// Multiplys one FastNumber with another
        /// </summary>
        public static FastNumber operator *(FastNumber left, FastNumber right)
        {
            return new FastNumber(left.Value * right.Value);
        }

		/// <summary>
        /// Multiplys a FastNumber with a double
        /// </summary>
        public static FastNumber operator *(FastNumber left, double right)
        {
            return new FastNumber(left.Value * right);
        }

		/// <summary>
        /// Multiplys a FastNumber with a double
        /// </summary>
        public static FastNumber operator *(double left, FastNumber right)
        {
            return new FastNumber(left * right.Value);
        }

		INumber INumber.Multiply(INumber operand)
		{
			return new FastNumber(Value * Convert.ToDouble(operand));
		}

		/// <summary>
        /// Divides one FastNumber with another
        /// </summary>
        public static FastNumber operator /(FastNumber left, FastNumber right)
        {
            return new FastNumber(left.Value / right.Value);
        }

		/// <summary>
        /// Divides a FastNumber with a double
        /// </summary>
        public static FastNumber operator /(FastNumber left, double right)
        {
            return new FastNumber(left.Value / right);
        }

		/// <summary>
        /// Divides a FastNumber with a double
        /// </summary>
        public static FastNumber operator /(double left, FastNumber right)
        {
            return new FastNumber(left / right.Value);
        }

		INumber INumber.Divide(INumber operand)
		{
			return new FastNumber(Value / Convert.ToDouble(operand));
		}

		/// <summary>
        /// Mods one FastNumber with another
        /// </summary>
        public static FastNumber operator %(FastNumber left, FastNumber right)
        {
            return new FastNumber(left.Value % right.Value);
        }

		/// <summary>
        /// Mods a FastNumber with a double
        /// </summary>
        public static FastNumber operator %(FastNumber left, double right)
        {
            return new FastNumber(left.Value % right);
        }

		/// <summary>
        /// Mods a FastNumber with a double
        /// </summary>
        public static FastNumber operator %(double left, FastNumber right)
        {
            return new FastNumber(left % right.Value);
        }

		INumber INumber.Mod(INumber operand)
		{
			return new FastNumber(Value % Convert.ToDouble(operand));
		}

        #endregion

		#region Conversion operators

        /// <summary>
        /// Implicitly converts a double to a FastNumber
        /// </summary>
        public static implicit operator FastNumber(double d)
        {
            return new FastNumber(d);
        }

        /// <summary>
        /// Implicitly converts a FastNumber to a double
        /// </summary>
        public static implicit operator double(FastNumber num)
        {
            return num.Value;
        }

        /// <summary>
        /// Converts sbyte to a FastNumber
        /// </summary>
        public static implicit operator FastNumber(sbyte n)
        {
            return new FastNumber(n);
        }
        /// <summary>
        /// Converts short to a FastNumber
        /// </summary>
        public static implicit operator FastNumber(short n)
        {
            return new FastNumber(n);
        }
        /// <summary>
        /// Converts int to a FastNumber
        /// </summary>
        public static implicit operator FastNumber(int n)
        {
            return new FastNumber(n);
        }
        /// <summary>
        /// Converts long to a FastNumber
        /// </summary>
        public static implicit operator FastNumber(long n)
        {
            return new FastNumber(n);
        }
		#endregion
    }
	
	#endregion

    /// <summary>
    /// Represents a number that can be represented precisely at the cost of performance
    /// </summary>
	#region PreciseNumber
    public struct PreciseNumber : INumber, IComparable<PreciseNumber>, IComparable<decimal>, IEquatable<PreciseNumber>, IEquatable<decimal>
    {
        /// <summary>
        /// Gets or sets the value this PreciseNumber represents
        /// </summary>
        public decimal Value;
        /// <summary>
        /// Gets the size of the type that represents this number
        /// </summary>
        public const int SIZE = sizeof(decimal);
        
        /// <summary>
        /// Constructs a new number
        /// </summary>
        public PreciseNumber(decimal value)
        {
            Value = value;
        }

        /// <summary>
        /// Determines if this number has a fractional part
        /// </summary>
        public bool HasFraction()
        {
            return Value % 1 != 0;
        }

		/// <summary>
		/// Gets a bool indicating whether or not the value of this number is 0
		/// </summary>
		public bool IsZero()
		{
			return Value == 0;
		}

		/// <summary>
		/// Converts this PreciseNumber to an int.
		/// </summary>
		/// <exception cref="ArgumentException">the value contains a fractional part</exception>
		/// <exception cref="OverflowException">the value was too large to store in an int</exception>
		public int ForceToInt()
		{
			if (HasFraction())
				throw new ArgumentException("Number contains a fractional part and cannot be used as an int");
			return (int)Value;
		}

        /// <summary>
        /// Converts this number to whichever primitive type seems most appropriate (either an integer type or a decimal)
        /// </summary>
        public object ToPrimitive()
        {
			if (HasFraction()) {
				if (Value <= int.MaxValue) {
					return (int)Value;
				}
				else {
					return (long)Value;
				}
			}
            return Value;
        }

        /// <summary>
        /// Tries to parse a string as a number
        /// </summary>
        public static bool TryParse(string s, out PreciseNumber result)
        {
            decimal d;
            if (decimal.TryParse(s, out d)) {
                result = new PreciseNumber(d);
                return true;
            }
            else {
                result = default(PreciseNumber);
                return false;
            }
        }

        /// <summary>
        /// Parses a string as a number
        /// </summary>
        public static PreciseNumber Parse(string s)
        {
            return decimal.Parse(s);
        }

        /// <summary>
        /// Determines if an object can be stored in a PreciseNumber
        /// </summary>
        public static bool IsPreciseNumber(object o, ExecuterOption opts)
        {
            decimal d;
            return TypeConvert.TryConvert(o, out d, opts);
        }

        /// <summary>
        /// Attempts to convert an object to a PreciseNumber. Returns null if no conversion is possible.
        /// </summary>
        public static PreciseNumber? AsPreciseNumber(object o, ExecuterOption opts)
        {
            decimal d;
            if (TypeConvert.TryConvert(o, out d, opts)) {
                return new PreciseNumber(d);
            }
            else {
                return null;
            }
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
        /// Compares this PreciseNumber with another object
        /// </summary>
        public int CompareTo(object obj)
        {
            PreciseNumber? n = obj as PreciseNumber?;
            if (n != null)
                return CompareTo(n);

			decimal? primitive = obj as decimal?;
            if (primitive != null)
                return CompareTo(primitive);

			INumber inum = obj as INumber;
			if (inum != null)
				return CompareTo(inum);

            throw new ArgumentException(string.Format("can only compare types {0} or {1}", typeof(PreciseNumber).Name, typeof(decimal).Name));
        }

        /// <summary>
        /// Compares this PreciseNumber to another PreciseNumber
        /// </summary>
        public int CompareTo(PreciseNumber other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Compares this PreciseNumber to a decimal
        /// </summary>
        public int CompareTo(decimal other)
        {
            return Value.CompareTo(other);
        }

        /// <summary>
        /// Compares this PreciseNumber to another INumber
        /// </summary>
        public int CompareTo(INumber other)
        {
            return Value.CompareTo((decimal)Convert.ChangeType(other, typeof(decimal)));
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Determines if this number is equal to another
        /// </summary>
        public bool Equals(PreciseNumber other)
        {
            return Value == other.Value;
        }

        /// <summary>
        /// Determines if this number is equal to a decimal
        /// </summary>
        public bool Equals(decimal other)
        {
            return Value == other;
        }

		/// <summary>
        /// Determines if this number is equal to another INumber
        /// </summary>
        public bool Equals(INumber other)
        {
            return Value == (decimal)Convert.ChangeType(other, typeof(decimal));
        }
        
        /// <summary>
        /// Determines if these two objects are equal
        /// </summary>
        public override bool Equals(object obj)
        {
            PreciseNumber? n = obj as PreciseNumber?;
            if (n != null)
                return Equals(n.Value);

            decimal? primitive = obj as decimal?;
            if (primitive != null)
                return Equals(primitive);

			INumber inum = obj as INumber;
			if (inum != null)
				return Equals(inum);

            return base.Equals(obj);
        }

        /// <summary>
        /// Gets the hash code for this number
        /// </summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion

        #region IConvertable
        
        TypeCode IConvertible.GetTypeCode()
        {
            return Value.GetTypeCode();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)Value).ToType(conversionType, provider);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        Boolean IConvertible.ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToBoolean(provider);
        }

        Char IConvertible.ToChar(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToChar(provider);
        }

        SByte IConvertible.ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToSByte(provider);
        }

        Byte IConvertible.ToByte(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToByte(provider);
        }

        Int16 IConvertible.ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt16(provider);
        }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt16(provider);
        }

        Int32 IConvertible.ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt32(provider);
        }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt32(provider);
        }

        Int64 IConvertible.ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToInt64(provider);
        }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToUInt64(provider);
        }

        Single IConvertible.ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToSingle(provider);
        }

        Double IConvertible.ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDouble(provider);
        }

        Decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDecimal(provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)Value).ToDateTime(provider);
        }

        #endregion

        #region Comparison operators

		/// <summary>
        /// Determines if one number is equal to another
        /// </summary>
        public static bool operator ==(PreciseNumber left, PreciseNumber right)
        {
            return left.Value == right.Value;
        }

		/// <summary>
        /// Determines if one number is not equal to another
        /// </summary>
        public static bool operator !=(PreciseNumber left, PreciseNumber right)
        {
            return left.Value != right.Value;
        }

		/// <summary>
        /// Determines if one number is greater than another
        /// </summary>
        public static bool operator >(PreciseNumber left, PreciseNumber right)
        {
            return left.Value > right.Value;
        }

		bool INumber.GreaterThan(INumber operand)
		{
			return Value > Convert.ToDecimal(operand);
		}

		/// <summary>
        /// Determines if one number is less than another
        /// </summary>
        public static bool operator <(PreciseNumber left, PreciseNumber right)
        {
            return left.Value < right.Value;
        }

		bool INumber.LessThan(INumber operand)
		{
			return Value < Convert.ToDecimal(operand);
		}

		/// <summary>
        /// Determines if one number is greater than or equal to another
        /// </summary>
        public static bool operator >=(PreciseNumber left, PreciseNumber right)
        {
            return left.Value >= right.Value;
        }

		bool INumber.GreaterThanOrEqualTo(INumber operand)
		{
			return Value >= Convert.ToDecimal(operand);
		}

		/// <summary>
        /// Determines if one number is less than or equal to another
        /// </summary>
        public static bool operator <=(PreciseNumber left, PreciseNumber right)
        {
            return left.Value <= right.Value;
        }

		bool INumber.LessThanOrEqualTo(INumber operand)
		{
			return Value <= Convert.ToDecimal(operand);
		}

        #endregion

        #region Arithemetic operators

		/// <summary>
        /// Adds one PreciseNumber with another
        /// </summary>
        public static PreciseNumber operator +(PreciseNumber left, PreciseNumber right)
        {
            return new PreciseNumber(left.Value + right.Value);
        }

		/// <summary>
        /// Adds a PreciseNumber with a decimal
        /// </summary>
        public static PreciseNumber operator +(PreciseNumber left, decimal right)
        {
            return new PreciseNumber(left.Value + right);
        }

		/// <summary>
        /// Adds a PreciseNumber with a decimal
        /// </summary>
        public static PreciseNumber operator +(decimal left, PreciseNumber right)
        {
            return new PreciseNumber(left + right.Value);
        }

		INumber INumber.Add(INumber operand)
		{
			return new PreciseNumber(Value + Convert.ToDecimal(operand));
		}

		/// <summary>
        /// Subtracts one PreciseNumber with another
        /// </summary>
        public static PreciseNumber operator -(PreciseNumber left, PreciseNumber right)
        {
            return new PreciseNumber(left.Value - right.Value);
        }

		/// <summary>
        /// Subtracts a PreciseNumber with a decimal
        /// </summary>
        public static PreciseNumber operator -(PreciseNumber left, decimal right)
        {
            return new PreciseNumber(left.Value - right);
        }

		/// <summary>
        /// Subtracts a PreciseNumber with a decimal
        /// </summary>
        public static PreciseNumber operator -(decimal left, PreciseNumber right)
        {
            return new PreciseNumber(left - right.Value);
        }

		INumber INumber.Subtract(INumber operand)
		{
			return new PreciseNumber(Value - Convert.ToDecimal(operand));
		}

		/// <summary>
        /// Multiplys one PreciseNumber with another
        /// </summary>
        public static PreciseNumber operator *(PreciseNumber left, PreciseNumber right)
        {
            return new PreciseNumber(left.Value * right.Value);
        }

		/// <summary>
        /// Multiplys a PreciseNumber with a decimal
        /// </summary>
        public static PreciseNumber operator *(PreciseNumber left, decimal right)
        {
            return new PreciseNumber(left.Value * right);
        }

		/// <summary>
        /// Multiplys a PreciseNumber with a decimal
        /// </summary>
        public static PreciseNumber operator *(decimal left, PreciseNumber right)
        {
            return new PreciseNumber(left * right.Value);
        }

		INumber INumber.Multiply(INumber operand)
		{
			return new PreciseNumber(Value * Convert.ToDecimal(operand));
		}

		/// <summary>
        /// Divides one PreciseNumber with another
        /// </summary>
        public static PreciseNumber operator /(PreciseNumber left, PreciseNumber right)
        {
            return new PreciseNumber(left.Value / right.Value);
        }

		/// <summary>
        /// Divides a PreciseNumber with a decimal
        /// </summary>
        public static PreciseNumber operator /(PreciseNumber left, decimal right)
        {
            return new PreciseNumber(left.Value / right);
        }

		/// <summary>
        /// Divides a PreciseNumber with a decimal
        /// </summary>
        public static PreciseNumber operator /(decimal left, PreciseNumber right)
        {
            return new PreciseNumber(left / right.Value);
        }

		INumber INumber.Divide(INumber operand)
		{
			return new PreciseNumber(Value / Convert.ToDecimal(operand));
		}

		/// <summary>
        /// Mods one PreciseNumber with another
        /// </summary>
        public static PreciseNumber operator %(PreciseNumber left, PreciseNumber right)
        {
            return new PreciseNumber(left.Value % right.Value);
        }

		/// <summary>
        /// Mods a PreciseNumber with a decimal
        /// </summary>
        public static PreciseNumber operator %(PreciseNumber left, decimal right)
        {
            return new PreciseNumber(left.Value % right);
        }

		/// <summary>
        /// Mods a PreciseNumber with a decimal
        /// </summary>
        public static PreciseNumber operator %(decimal left, PreciseNumber right)
        {
            return new PreciseNumber(left % right.Value);
        }

		INumber INumber.Mod(INumber operand)
		{
			return new PreciseNumber(Value % Convert.ToDecimal(operand));
		}

        #endregion

		#region Conversion operators

        /// <summary>
        /// Implicitly converts a decimal to a PreciseNumber
        /// </summary>
        public static implicit operator PreciseNumber(decimal d)
        {
            return new PreciseNumber(d);
        }

        /// <summary>
        /// Implicitly converts a PreciseNumber to a decimal
        /// </summary>
        public static implicit operator decimal(PreciseNumber num)
        {
            return num.Value;
        }

        /// <summary>
        /// Converts sbyte to a PreciseNumber
        /// </summary>
        public static implicit operator PreciseNumber(sbyte n)
        {
            return new PreciseNumber(n);
        }
        /// <summary>
        /// Converts short to a PreciseNumber
        /// </summary>
        public static implicit operator PreciseNumber(short n)
        {
            return new PreciseNumber(n);
        }
        /// <summary>
        /// Converts int to a PreciseNumber
        /// </summary>
        public static implicit operator PreciseNumber(int n)
        {
            return new PreciseNumber(n);
        }
        /// <summary>
        /// Converts long to a PreciseNumber
        /// </summary>
        public static implicit operator PreciseNumber(long n)
        {
            return new PreciseNumber(n);
        }
		#endregion
    }
	
	#endregion
}

