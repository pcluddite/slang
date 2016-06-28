/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License for
 * non-commercial use.
 *
 **/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tbasic.Operators
{
    internal partial class BinOpDictionary : IDictionary<string, BinaryOperator>
    {
        public BinaryOperator this[string key]
        {
            get {
                return binaryOps[key];
            }
            set {
                binaryOps[key] = value;
            }
        }

        public int Count
        {
            get {
                return binaryOps.Count;
            }
        }

        public ICollection<string> Keys
        {
            get {
                return binaryOps.Keys;
            }
        }

        public ICollection<BinaryOperator> Values
        {
            get {
                return binaryOps.Values;
            }
        }

        public void Add(string key, BinaryOperator value)
        {
            binaryOps.Add(key, value);
        }

        public void Clear()
        {
            binaryOps.Clear();
        }

        public bool Contains(KeyValuePair<string, BinaryOperator> item)
        {
            return binaryOps.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return binaryOps.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, BinaryOperator>> GetEnumerator()
        {
            return binaryOps.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return binaryOps.Remove(key);
        }

        public bool TryGetValue(string key, out BinaryOperator value)
        {
            return binaryOps.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return binaryOps.GetEnumerator();
        }

        bool ICollection<KeyValuePair<string, BinaryOperator>>.IsReadOnly
        {
            get {
                return ((ICollection<KeyValuePair<string, BinaryOperator>>)binaryOps).IsReadOnly;
            }
        }

        void ICollection<KeyValuePair<string, BinaryOperator>>.Add(KeyValuePair<string, BinaryOperator> item)
        {
            ((ICollection<KeyValuePair<string, BinaryOperator>>)binaryOps).Add(item);
        }

        void ICollection<KeyValuePair<string, BinaryOperator>>.CopyTo(KeyValuePair<string, BinaryOperator>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, BinaryOperator>>)binaryOps).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, BinaryOperator>>.Remove(KeyValuePair<string, BinaryOperator> item)
        {
            return ((ICollection<KeyValuePair<string, BinaryOperator>>)binaryOps).Remove(item);
        }
    }
}
