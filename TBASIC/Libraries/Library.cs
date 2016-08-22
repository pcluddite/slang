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
using System.Reflection;

namespace Tbasic.Libraries
{
    /// <summary>
    /// A library for storing and processing TBasic functions
    /// </summary>
    public class Library : IDictionary<string, TBasicFunction>
    {
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

        /// <summary>
        /// Adds a function to this dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, TBasicFunction value)
        {
            lib.Add(key, new CallData(value));
        }

        /// <summary>
        /// Adds any delegate you might get your hands on
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddDelegate(string key, Delegate value)
        {
            TBasicFunction func = value as TBasicFunction;
            if (func != null)
                Add(key, func);
            if (value.Method.ReturnType == typeof(void))

            lib.Add(key, new CallData(value, CountParameters(value)));
        }

        /// <summary>
        /// Adds a native function that takes no parameters and does not return a result
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, Action value)
        {
            lib.Add(key, new CallData(value, 0));
        }

        /// <summary>
        /// Adds a native function that takes one parameter and does not return a result
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T>(string key, Action<T> value)
        {
            lib.Add(key, new CallData(value, 1));
        }

        /// <summary>
        /// Adds a native function that takes two parameters and does not return a result
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T1, T2>(string key, Action<T1, T2> value)
        {
            lib.Add(key, new CallData(value, 2));
        }

        /// <summary>
        /// Adds a native function that takes three parameters and does not return a result
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T1, T2, T3>(string key, Action<T1, T2, T3> value)
        {
            lib.Add(key, new CallData(value, 3));
        }

        /// <summary>
        /// Adds a native function that takes four parameters and does not return a result
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T1, T2, T3, T4>(string key, Action<T1, T2, T3, T4> value)
        {
            lib.Add(key, new CallData(value, 4));
        }

        private static int CountParameters(Delegate d)
        {
            return d.Method.GetParameters().Length;
        }
        
        private static int CheckParameters(Delegate d)
        {
            if (!typeof(IConvertible).IsAssignableFrom(d.Method.ReturnType) && d.Method.ReturnType != typeof(object)) {
                throw new ArgumentException("Delegate must have an IConvertible return type");
            }
            ParameterInfo[] info = d.Method.GetParameters();
            for (int index = 0; index < info.Length; ++index) {
                if (!typeof(IConvertible).IsAssignableFrom(info[index].ParameterType)) {
                    throw new ArgumentException(string.Format("{0} cannot be {1} because {1} does not implement IConvertible", info[index].Name, info[index].ParameterType.Name));
                }
            }
            return info.Length;
        }

        /// <summary>
        /// Adds a native function that takes no parameters and returns a result
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<TResult>(string key, Func<TResult> value)
        {
            lib.Add(key, new CallData(value, 0));
        }

        /// <summary>
        /// Adds a native function that takes one parameter and returns a result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T, TResult>(string key, Func<T, TResult> value)
        {
            lib.Add(key, new CallData(value, 1));
        }

        /// <summary>
        /// Adds a function that takes two parameters and returns a result
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T1, T2, TResult>(string key, Func<T1, T2, TResult> value)
        {
            lib.Add(key, new CallData(value, 2));
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
        public void Add<T1, T2, T3, TResult>(string key, Func<T1, T2, T3, TResult> value)
        {
            lib.Add(key, new CallData(value, 3));
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
        public void Add<T1, T2, T3, T4, TResult>(string key, Func<T1, T2, T3, T4, TResult> value)
        {
            lib.Add(key, new CallData(value, 4));
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

        /// <summary>
        /// Gets a collection of this dictionary's values
        /// </summary>
        public ICollection<TBasicFunction> Values
        {
            get { return (from info in lib.Values
                          select info.Function).ToArray(); }
        }

        /// <summary>
        /// Gets or sets a function at a given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TBasicFunction this[string key]
        {
            get {
                return lib[key].Function;
            }
            set {
                lib[key] = new CallData(value);
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
        /// Returns the number of elements in this collection
        /// </summary>
        public int Count
        {
            get { return lib.Count; }
        }

        void ICollection<KeyValuePair<string, TBasicFunction>>.Add(KeyValuePair<string, TBasicFunction> item)
        {
            Add(item.Key, item.Value);
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
        
        /// <summary>
        /// Gets an enumerator for this dictionary
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, TBasicFunction>> GetEnumerator()
        {
            foreach (var kv in lib)
                yield return new KeyValuePair<string, TBasicFunction>(kv.Key, kv.Value.Function);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
