// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections;
using System.Collections.Generic;
using TLang.Types;

namespace TLang.Libraries
{
    /// <summary>
    /// A library for storing and processing TBasic functions
    /// </summary>
    public class Library : IDictionary<string, CallData>
    {
        private Dictionary<string, CallData> lib = new Dictionary<string, CallData>(StringComparer.CurrentCultureIgnoreCase);
        
        /// <summary>
        /// Initializes a new Tbasic Library object
        /// </summary>
        public Library()
        {
        }

        /// <summary>
        /// Initializes a new Tbasic Library object incorporating the functions from another library
        /// </summary>
        public Library(Library other)
        {
            lib = new Dictionary<string, CallData>(other.lib);
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
        /// <param name="other">the Tbasic Library</param>
        public void AddLibrary(Library other)
        {
            foreach (var kv_entry in other.lib)
                lib.Add(kv_entry.Key, kv_entry.Value);
        }

        /// <summary>
        /// Adds a function to this dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, TBasicFunction value)
        {
            lib.Add(key, new CallData(value, evaluate: true));
        }

        /// <summary>
        /// Adds a function to this dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, CallData value)
        {
            lib.Add(key, value);
        }

        /// <summary>
        /// Adds a function to this dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add(string key, TBasicFunction value, bool evaluate)
        {
            lib.Add(key, new CallData(value, evaluate));
        }

        /// <summary>
        /// Adds any delegate you might get your hands on
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void AddDelegate(string key, Delegate value, bool evaluate = true)
        {
            TBasicFunction func = value as TBasicFunction;
            if (func != null)
                Add(key, func);
            if (value.Method.ReturnType == typeof(void))

            lib.Add(key, new CallData(value, CountParameters(value), evaluate));
        }

        /// <summary>
        /// Adds a native function that takes no parameters and does not return a result
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add(string key, Action value, bool evaluate = true)
        {
            lib.Add(key, new CallData(value, 0, evaluate));
        }

        /// <summary>
        /// Adds a native function that takes one parameter and does not return a result
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add<T>(string key, Action<T> value, bool evaluate = true)
        {
            lib.Add(key, new CallData(value, 1, evaluate));
        }

        /// <summary>
        /// Adds a native function that takes two parameters and does not return a result
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add<T1, T2>(string key, Action<T1, T2> value, bool evaluate = true)
        {
            lib.Add(key, new CallData(value, 2, evaluate));
        }

        /// <summary>
        /// Adds a native function that takes three parameters and does not return a result
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add<T1, T2, T3>(string key, Action<T1, T2, T3> value, bool evaluate = true)
        {
            lib.Add(key, new CallData(value, 3, evaluate));
        }

        /// <summary>
        /// Adds a native function that takes four parameters and does not return a result
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add<T1, T2, T3, T4>(string key, Action<T1, T2, T3, T4> value, bool evaluate = true)
        {
            lib.Add(key, new CallData(value, 4, evaluate));
        }

        private static int CountParameters(Delegate d)
        {
            return d.Method.GetParameters().Length;
        }
        
        /// <summary>
        /// Adds a native function that takes no parameters and returns a result
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add<TResult>(string key, Func<TResult> value, bool evaluate = true)
        {
            lib.Add(key, new CallData(value, 0, evaluate));
        }

        /// <summary>
        /// Adds a native function that takes one parameter and returns a result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add<T, TResult>(string key, Func<T, TResult> value, bool evaluate = true)
        {
            lib.Add(key, new CallData(value, 1, evaluate));
        }

        /// <summary>
        /// Adds a function that takes two parameters and returns a result
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add<T1, T2, TResult>(string key, Func<T1, T2, TResult> value, bool evaluate = true)
        {
            lib.Add(key, new CallData(value, 2, evaluate));
        }

        /// <summary>
        /// Adds a function that takes three parameters and returns a result
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add<T1, T2, T3, TResult>(string key, Func<T1, T2, T3, TResult> value, bool evaluate = true)
        {
            lib.Add(key, new CallData(value, 3, evaluate));
        }

        /// <summary>
        /// Adds a function that takes four parameters and returns a result
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluate">whether or not this function should have its parameters evaluated</param>
        public void Add<T1, T2, T3, T4, TResult>(string key, Func<T1, T2, T3, T4, TResult> value, bool evaluate = true)
        {
            lib.Add(key, new CallData(value, 4, evaluate));
        }

        /// <summary>
        /// Determines if a key exists in this dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return lib.ContainsKey(key);
        }

        /// <summary>
        /// Gets a collection of this dictionary's keys
        /// </summary>
        public ICollection<string> Keys
        {
            get { return lib.Keys; }
        }

        /// <summary>
        /// Removes a key and its associated value from the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return lib.Remove(key);
        }

        /// <summary>
        /// Tries to get a value from this dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out CallData value)
        {
            return lib.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets a collection of this dictionary's values
        /// </summary>
        public ICollection<CallData> Values
        {
            get { return lib.Values; }
        }

        /// <summary>
        /// Gets or sets a function at a given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CallData this[string key]
        {
            get {
                return lib[key];
            }
            set {
                lib[key] = value;
            }
        }

        /// <summary>
        /// Clears all items from this dictionary
        /// </summary>
        public void Clear()
        {
            lib.Clear();
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, CallData>> GetEnumerator()
        {
            return ((IDictionary<string, CallData>)lib).GetEnumerator();
        }

        /// <summary>
        /// Returns the number of elements in this collection
        /// </summary>
        public int Count
        {
            get { return lib.Count; }
        }
        
        bool IDictionary<string, CallData>.TryGetValue(string key, out CallData value)
        {
            return ((IDictionary<string, CallData>)lib).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, CallData>)lib).GetEnumerator();
        }

        bool ICollection<KeyValuePair<string, CallData>>.IsReadOnly
        {
            get {
                return ((IDictionary<string, CallData>)lib).IsReadOnly;
            }
        }

        void ICollection<KeyValuePair<string, CallData>>.CopyTo(KeyValuePair<string, CallData>[] array, int arrayIndex)
        {
            ((IDictionary<string, CallData>)lib).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, CallData>>.Remove(KeyValuePair<string, CallData> item)
        {
            return ((IDictionary<string, CallData>)lib).Remove(item);
        }

        void ICollection<KeyValuePair<string, CallData>>.Add(KeyValuePair<string, CallData> item)
        {
            ((IDictionary<string, CallData>)lib).Add(item);
        }

        bool ICollection<KeyValuePair<string, CallData>>.Contains(KeyValuePair<string, CallData> item)
        {
            return ((IDictionary<string, CallData>)lib).Contains(item);
        }
    }
}
