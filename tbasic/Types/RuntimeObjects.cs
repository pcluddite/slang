// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;

namespace Tbasic.Types
{
    /// <summary>
    /// An interface for a Tbasic object that can be handled at runtime
    /// </summary>
    public interface IRuntimeObject
    {
        /// <summary>
        /// Gets the type of this object
        /// </summary>
        TbasicType TypeCode { get; }
        /// <summary>
        /// The object's value
        /// </summary>
        object Value { get; }
    }

    /// <summary>
    /// Represents a bool in the runtime
    /// </summary>
    public struct TbasicBoolean : IRuntimeObject
    {
        /// <summary>
        /// The bool value
        /// </summary>
        public bool Value { get; }

        /// <summary>
        /// Initializes a new TbasicBoolean
        /// </summary>
        public TbasicBoolean(bool value)
        {
            Value = value;
        }

        /// <summary>
        /// Implicitly converts a bool to a TbasicBoolean
        /// </summary>
        public static implicit operator TbasicBoolean(bool value)
        {
            return new TbasicBoolean(value);
        }

        TbasicType IRuntimeObject.TypeCode
        {
            get {
                return TbasicType.Boolean;
            }
        }

        object IRuntimeObject.Value
        {
            get {
                return Value;
            }
        }
    }

    /// <summary>
    /// Represents a Enum in the runtime
    /// </summary>
    public struct TbasicEnumValue : IRuntimeObject
    {
        /// <summary>
        /// The Enum value
        /// </summary>
        public Enum Value { get; }

        /// <summary>
        /// Initializes a new TbasicEnumValue
        /// </summary>
        public TbasicEnumValue(Enum value)
        {
            Value = value;
        }

        /// <summary>
        /// Implicitly converts a Enum to a TbasicEnumValue
        /// </summary>
        public static implicit operator TbasicEnumValue(Enum value)
        {
            return new TbasicEnumValue(value);
        }

        TbasicType IRuntimeObject.TypeCode
        {
            get {
                return TbasicType.EnumValue;
            }
        }

        object IRuntimeObject.Value
        {
            get {
                return Value;
            }
        }
    }

    /// <summary>
    /// Represents a object in the runtime
    /// </summary>
    public struct TbasicNative : IRuntimeObject
    {
        /// <summary>
        /// The object value
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes a new TbasicNative
        /// </summary>
        public TbasicNative(object value)
        {
            Value = value;
        }

        TbasicType IRuntimeObject.TypeCode
        {
            get {
                return TbasicType.Native;
            }
        }

        object IRuntimeObject.Value
        {
            get {
                return Value;
            }
        }
    }
}

