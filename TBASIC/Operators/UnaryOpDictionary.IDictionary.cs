/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License
 * for non-commercial use.
**/
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tbasic.Operators
{
    internal partial class UnaryOpDictionary : IDictionary<string, UnaryOperator>
    {
        public UnaryOperator this[string key]
        {
            get {
                return unaryOps[key];
            }
            set {
                unaryOps[key] = value;
            }
        }

        public int Count
        {
            get {
                return unaryOps.Count;
            }
        }

        public ICollection<string> Keys
        {
            get {
                return unaryOps.Keys;
            }
        }

        public ICollection<UnaryOperator> Values
        {
            get {
                return unaryOps.Values;
            }
        }

        public void Add(string key, UnaryOperator value)
        {
            unaryOps.Add(key, value);
        }

        public void Clear()
        {
            unaryOps.Clear();
        }

        public bool Contains(KeyValuePair<string, UnaryOperator> item)
        {
            return unaryOps.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return unaryOps.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, UnaryOperator>> GetEnumerator()
        {
            return unaryOps.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return unaryOps.Remove(key);
        }

        public bool TryGetValue(string key, out UnaryOperator value)
        {
            return unaryOps.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return unaryOps.GetEnumerator();
        }

        bool ICollection<KeyValuePair<string, UnaryOperator>>.IsReadOnly
        {
            get {
                return ((ICollection<KeyValuePair<string, UnaryOperator>>)unaryOps).IsReadOnly;
            }
        }

        void ICollection<KeyValuePair<string, UnaryOperator>>.Add(KeyValuePair<string, UnaryOperator> item)
        {
            ((ICollection<KeyValuePair<string, UnaryOperator>>)unaryOps).Add(item);
        }

        void ICollection<KeyValuePair<string, UnaryOperator>>.CopyTo(KeyValuePair<string, UnaryOperator>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, UnaryOperator>>)unaryOps).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, UnaryOperator>>.Remove(KeyValuePair<string, UnaryOperator> item)
        {
            return ((ICollection<KeyValuePair<string, UnaryOperator>>)unaryOps).Remove(item);
        }
    }
}
