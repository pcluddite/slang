// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TLang.Types
{
    internal abstract class OperatorDictionary<T> : IDictionary<string, T>
        where T : IOperator
    {
        protected Dictionary<string, T> operators = new Dictionary<string, T>(StringComparer.CurrentCultureIgnoreCase);

        protected OperatorDictionary()
        {
        }

        protected OperatorDictionary(OperatorDictionary<T> other)
        {
            operators = new Dictionary<string, T>(other.operators, StringComparer.CurrentCultureIgnoreCase);
        }

        public abstract void LoadStandardOperators();

        public T this[string key]
        {
            get {
                return operators[key];
            }

            set {
                operators[key] = value;
            }
        }

        public int Count
        {
            get {
                return operators.Count;
            }
        }

        bool ICollection<KeyValuePair<string, T>>.IsReadOnly
        {
            get {
                return ((ICollection<KeyValuePair<string, T>>)operators).IsReadOnly;
            }
        }

        public ICollection<string> Keys
        {
            get {
                return operators.Keys;
            }
        }

        public ICollection<T> Values
        {
            get {
                return operators.Values;
            }
        }

        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item)
        {
            ((ICollection<KeyValuePair<string, T>>)operators).Add(item);
        }

        public void Add(string key, T value)
        {
            operators.Add(key, value);
        }

        public void Clear()
        {
            operators.Clear();
        }

        public bool Contains(KeyValuePair<string, T> item)
        {
            return operators.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return operators.ContainsKey(key);
        }

        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, T>>)operators).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return operators.GetEnumerator();
        }

        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
        {
            return ((ICollection<KeyValuePair<string, T>>)operators).Remove(item);
        }

        public bool Remove(string key)
        {
            return operators.Remove(key);
        }

        public bool TryGetValue(string key, out T value)
        {
            return operators.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return operators.GetEnumerator();
        }
    }
}
