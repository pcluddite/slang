// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;

namespace Tbasic.Runtime
{
    internal struct Number : IConvertible, IComparable, IComparable<Number>, IComparable<double>, IEquatable<Number>, IEquatable<double>
    {
        public double Value;
        public const int SIZE = sizeof(double);
        
        public Number(double value)
        {
            Value = value;
        }

        public bool HasFraction()
        {
            return Value % 1 != 0;
        }

        public int ToInt()
        {
            return (int)Value;
        }

        public object ToPrimitive()
        {
            if (HasFraction())
                return Value;
            return ToInt();
        }

        public static bool TryParse(string s, out Number result)
        {
            double d;
            if (double.TryParse(s, out d)) {
                result = new Number(d);
                return true;
            }
            else {
                result = default(Number);
                return false;
            }
        }

        public static Number Parse(string s)
        {
            return double.Parse(s);
        }

        public static bool IsNumber(object o, ExecuterOption opts)
        {
            double d;
            return ObjectConvert.TryConvert(o, out d, opts);
        }

        public static Number? AsNumber(object o, ExecuterOption opts)
        {
            double d;
            if (ObjectConvert.TryConvert(o, out d, opts)) {
                return d;
            }
            else {
                return null;
            }
        }

        public static Number Convert(object o)
        {
            double d;
            if (!ObjectConvert.TryConvert(o, out d, strict: false, parseStrings: false)) // this should always obey these rules
                throw new InvalidCastException();
            return d;
        }

        public static implicit operator Number(double d)
        {
            return new Number(d);
        }

        public static implicit operator Number(int n)
        {
            return new Number(n);
        }

        public static implicit operator int(Number n)
        {
            if (n.HasFraction())
                throw new InvalidCastException("Number contains a fractional part");
            return (int)n.Value;
        }
        
        public override string ToString()
        {
            if (HasFraction())
                return Value.ToString();
            return ((long)Value).ToString();
        }

        #region IComparable

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

        public int CompareTo(Number other)
        {
            return Value.CompareTo(other.Value);
        }

        public int CompareTo(double other)
        {
            return Value.CompareTo(other);
        }

        #endregion

        #region IEquatable

        public bool Equals(Number other)
        {
            return Value == other.Value;
        }

        public bool Equals(double other)
        {
            return Value == other;
        }

        public override bool Equals(object obj)
        {
            Number? n = obj as Number?;
            if (n != null)
                return Equals(n.Value);

            double? d = obj as double?;
            if (d != null)
                return Equals(d.Value);

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion

        #region boolean ops

        public static bool operator ==(Number left, Number right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(Number left, Number right)
        {
            return left.Value != right.Value;
        }

        public static bool operator <(Number left, Number right)
        {
            return left.Value < right.Value;
        }

        public static bool operator <=(Number left, Number right)
        {
            return left.Value <= right.Value;
        }

        public static bool operator >(Number left, Number right)
        {
            return left.Value > right.Value;
        }

        public static bool operator >=(Number left, Number right)
        {
            return left.Value >= right.Value;
        }

        #endregion

        #region arithmetic ops

        public static Number operator +(Number left, Number right)
        {
            return new Number(left.Value + right.Value);
        }

        public static Number operator -(Number left, Number right)
        {
            return new Number(left.Value - right.Value);
        }

        public static Number operator *(Number left, Number right)
        {
            return new Number(left.Value * right.Value);
        }

        public static Number operator /(Number left, Number right)
        {
            return new Number(left.Value / right.Value);
        }

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
