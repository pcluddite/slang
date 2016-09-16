// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections;
using System.Collections.Generic;

namespace Tbasic.Types
{
    /// <summary>
    /// Represents a string in the runtime
    /// </summary>
    public sealed class TbasicString : IRuntimeObject, IEnumerable<char>, IList<char>
    {
        /// <summary>
        /// The string value
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new TbasicString
        /// </summary>
        public TbasicString(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets a character at a given index
        /// </summary>
        public char this[int index]
        {
            get {
                return Value[index];
            }
        }

        /// <summary>
        /// Gets the length of this string
        /// </summary>
        public int Length
        {
            get {
                return Value.Length;
            }
        }

        /// <summary>
        /// Implicitly converts a string to a TbasicString
        /// </summary>
        public static implicit operator TbasicString(string value)
        {
            return new TbasicString(value);
        }

        TbasicType IRuntimeObject.TypeCode
        {
            get {
                return TbasicType.String;
            }
        }

        object IRuntimeObject.Value
        {
            get {
                return Value;
            }
        }

        int ICollection<char>.Count
        {
            get {
                throw new NotImplementedException();
            }
        }

        bool ICollection<char>.IsReadOnly
        {
            get {
                return true;
            }
        }

        char IList<char>.this[int index]
        {
            get {
                return this[index];
            }
            set {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the enumerator for this type
        /// </summary>
        public IEnumerator<char> GetEnumerator()
        {
            return ((IEnumerable<char>)Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<char>)Value).GetEnumerator();
        }

        /// <summary>
        /// Finds the index of a given character
        /// </summary>
        public int IndexOf(char item)
        {
            return Value.IndexOf(item);
        }

        void IList<char>.Insert(int index, char item)
        {
            throw new NotSupportedException();
        }

        void IList<char>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection<char>.Add(char item)
        {
            throw new NotSupportedException();
        }

        void ICollection<char>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<char>.Contains(char item)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Copies this string to a character array
        /// </summary>
        public void CopyTo(char[] array, int arrayIndex)
        {
            Value.CopyTo(0, array, arrayIndex, Length);
        }

        bool ICollection<char>.Remove(char item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the string representation of this object
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
    }
}
