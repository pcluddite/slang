// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Collections;
using Tbasic.Runtime;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Tbasic.Libraries
{
    /// <summary>
    /// A library for storing and processing TBasic functions
    /// </summary>
    public class Library : IDictionary<string, TBasicFunction>
    {
#pragma warning disable CS1591
        private Dictionary<string, CallData> lib = new Dictionary<string, CallData>(StringComparer.OrdinalIgnoreCase);
        
        /// <summary>
        /// Initializes a new Tbasic Library object
        /// </summary>
        public Library()
        {
        }

        /// <summary>
        /// Initializes a new Tbasic Library object
        /// </summary>
        /// <param name="libs">a collection of Library objects that should be incorporated into this one</param>
        public Library(ICollection<Library> libs)
        {
            foreach (Library lib in libs) 
                AddLibrary(lib);
        }

        /// <summary>
        /// Adds a Tbasic Library to this one
        /// </summary>
        /// <param name="lib">the Tbasic Library</param>
        public void AddLibrary(Library lib)
        {
            foreach (var kv_entry in lib)
                Add(kv_entry.Key, kv_entry.Value);
        }

        public void Add(string key, TBasicFunction value)
        {
            lib.Add(key, new CallData(value));
        }

        public void Add(string key, Delegate value)
        {
            TBasicFunction func = value as TBasicFunction;
            if (func != null)
                Add(key, func);
            lib.Add(key, new CallData(value));
        }
        
        public void Add<TResult>(string key, Func<TResult> value)
        {
            Add(key, (Delegate)value);
        }

        public void Add<T, TResult>(string key, Func<T, TResult> value)
        {
            Add(key, (Delegate)value);
        }

        public void Add<T1, T2, TResult>(string key, Func<T1, T2, TResult> value)
        {
            Add(key, (Delegate)value);
        }

        public void Add<T1, T2, T3, TResult>(string key, Func<T1, T2, T3, TResult> value)
        {
            Add(key, (Delegate)value);
        }

        public void Add<T1, T2, T3, T4, TResult>(string key, Func<T1, T2, T3, T4, TResult> value)
        {
            Add(key, (Delegate)value);
        }

        public bool ContainsKey(string key)
        {
            return lib.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return lib.Keys; }
        }

        public bool Remove(string key)
        {
            return lib.Remove(key);
        }

        public bool TryGetValue(string key, out TBasicFunction value)
        {
            CallData info;
            if (lib.TryGetValue(key, out info)) {
                value = info.Function;
                return true;
            }
            else {
                value = null;
                return false;
            }
        }

        public ICollection<TBasicFunction> Values
        {
            get { return (from info in lib.Values
                          select info.Function).ToArray(); }
        }

        public TBasicFunction this[string key]
        {
            get {
                return lib[key].Function;
            }
            set {
                lib[key] = new CallData(value);
            }
        }

        void ICollection<KeyValuePair<string, TBasicFunction>>.Add(KeyValuePair<string, TBasicFunction> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            lib.Clear();
        }

        bool ICollection<KeyValuePair<string, TBasicFunction>>.Contains(KeyValuePair<string, TBasicFunction> item)
        {
            CallData data;
            if (lib.TryGetValue(item.Key, out data))
                return item.Value == data.Function;
            return false;
        }

        void ICollection<KeyValuePair<string, TBasicFunction>>.CopyTo(KeyValuePair<string, TBasicFunction>[] array, int arrayIndex)
        {
            foreach(var kv in this) {
                array[arrayIndex++] = kv;
            }
        }

        public int Count
        {
            get { return lib.Count; }
        }

        bool ICollection<KeyValuePair<string, TBasicFunction>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<string, CallData>>)lib).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<string, TBasicFunction>>.Remove(KeyValuePair<string, TBasicFunction> item)
        {
            CallData data;
            if (lib.TryGetValue(item.Key, out data) && item.Value == data.Function) {
                lib.Remove(item.Key);
                return true;
            }
            return false;
        }

        public IEnumerator<KeyValuePair<string, TBasicFunction>> GetEnumerator()
        {
            foreach (var kv in lib)
                yield return new KeyValuePair<string, TBasicFunction>(kv.Key, kv.Value.Function);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
#pragma warning restore CS1591
    }
}
