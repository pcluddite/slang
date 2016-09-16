
// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tbasic.Types
{
    /// <summary>
    /// Represents a IRuntimeObject[] in the runtime
    /// </summary>
    public struct TbasicArray : IList<IRuntimeObject>, IRuntimeObject
    {
        /// <summary>
        /// The IRuntimeObject[] value
        /// </summary>
        public IRuntimeObject[] Value { get; }

        /// <summary>
        /// Initializes a new TbasicArray
        /// </summary>
        public TbasicArray(IRuntimeObject[] value)
        {
            Value = value;
        }

        /// <summary>
        /// Implicitly converts a IRuntimeObject[] to a TbasicArray
        /// </summary>
        public static implicit operator TbasicArray(IRuntimeObject[] value)
        {
            return new TbasicArray(value);
        }

        /// <summary>
        /// Gets or sets the array at a given index
        /// </summary>
        public IRuntimeObject this[int index]
        {
            get {
                return Value[index];
            }

            set {
                Value[index] = value;
            }
        }

        /// <summary>
        /// Gets the length of this object
        /// </summary>
        public int Length
        {
            get {
                return Value.Length;
            }
        }

        /// <summary>
        /// Copies this array to another
        /// </summary>
        public void CopyTo(IRuntimeObject[] array, int arrayIndex)
        {
            Value.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        public IEnumerator<IRuntimeObject> GetEnumerator()
        {
            return ((IList<IRuntimeObject>)Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<IRuntimeObject>)Value).GetEnumerator();
        }

        int IList<IRuntimeObject>.IndexOf(IRuntimeObject item)
        {
            return ((IList<IRuntimeObject>)Value).IndexOf(item);
        }

        void IList<IRuntimeObject>.Insert(int index, IRuntimeObject item)
        {
            ((IList<IRuntimeObject>)Value).Insert(index, item);
        }

        void IList<IRuntimeObject>.RemoveAt(int index)
        {
            ((IList<IRuntimeObject>)Value).RemoveAt(index);
        }

        void ICollection<IRuntimeObject>.Add(IRuntimeObject item)
        {
            ((ICollection<IRuntimeObject>)Value).Add(item);
        }

        void ICollection<IRuntimeObject>.Clear()
        {
            ((ICollection<IRuntimeObject>)Value).Clear();
        }

        bool ICollection<IRuntimeObject>.Contains(IRuntimeObject item)
        {
            return ((ICollection<IRuntimeObject>)Value).Contains(item);
        }

        bool ICollection<IRuntimeObject>.Remove(IRuntimeObject item)
        {
            return ((IList<IRuntimeObject>)Value).Remove(item);
        }

        TbasicType IRuntimeObject.TypeCode
        {
            get {
                return TbasicType.Array;
            }
        }

        object IRuntimeObject.Value
        {
            get {
                return Value;
            }
        }

        int ICollection<IRuntimeObject>.Count
        {
            get {
                return Value.Length;
            }
        }

        bool ICollection<IRuntimeObject>.IsReadOnly
        {
            get {
                return true;
            }
        }
    }
}
