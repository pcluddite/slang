// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Tbasic.Libraries
{
    internal sealed class TBasicLibrary : IDictionary<string, FunctionInfo>
    {
        private Dictionary<string, FunctionInfo> lib = new Dictionary<string, FunctionInfo>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new Tbasic Library object
        /// </summary>
        public TBasicLibrary()
        {
        }
        
        public void AddLibrary(Type libType)
        {
            if (libType.GetCustomAttributes(typeof(TBasicLibrary), inherit: false).Length == 0)
                throw new ArgumentException("The type is not a TBasic library");
            foreach (MethodInfo method in libType.GetMethods()) {
                object[] allAttr = method.GetCustomAttributes(typeof(TBasicInstructionAttribute), inherit: false);
                if (allAttr.Length == 0)
                    continue;
                TBasicInstructionAttribute attr = (TBasicInstructionAttribute)allAttr[0];
                if (attr.DelegateType == typeof(TBasicFunction)) {
                    Add(attr.Name, (TBasicFunction)Delegate.CreateDelegate(typeof(TBasicFunction), method));
                }
                else {
                    Add(attr.Name, new FunctionInfo(Delegate.CreateDelegate(attr.DelegateType, method), attr.RequiredParameters));
                }
                if (method.Name == "AddUntaggedDelegates" && method.ReturnType == typeof(void) &&
                    method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(TBasicLibrary)) {
                    method.Invoke(null, new object[] { this });
                }
            }
        }

        [Obsolete]
        public void AddLibrary(Library lib)
        {
            foreach(var kvp in lib) {
                Add(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Adds a function to this dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, FunctionInfo value)
        {
            lib.Add(key, value);
        }


        /// <summary>
        /// Adds a function to this dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, TBasicFunction value)
        {
            lib.Add(key, new FunctionInfo(value));
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
            lib.Add(key, new FunctionInfo(value, CountParameters(value)));
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
        public void Add<TResult>(string key, Func<TResult> value)
        {
            lib.Add(key, new FunctionInfo(value, 0));
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
            lib.Add(key, new FunctionInfo(value, 1));
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
            lib.Add(key, new FunctionInfo(value, 2));
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
            lib.Add(key, new FunctionInfo(value, 3));
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
            lib.Add(key, new FunctionInfo(value, 4));
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
        public bool TryGetValue(string key, out FunctionInfo value)
        {
            return lib.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets a collection of this dictionary's values
        /// </summary>
        public ICollection<FunctionInfo> Values
        {
            get {
                return lib.Values;
            }
        }

        /// <summary>
        /// Gets or sets a function at a given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public FunctionInfo this[string key]
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
        /// Returns the number of elements in this collection
        /// </summary>
        public int Count
        {
            get { return lib.Count; }
        }

        void ICollection<KeyValuePair<string, FunctionInfo>>.Add(KeyValuePair<string, FunctionInfo> item)
        {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<string, FunctionInfo>>.Contains(KeyValuePair<string, FunctionInfo> item)
        {
            return ((ICollection<KeyValuePair<string, FunctionInfo>>)lib).Contains(item);
        }

        void ICollection<KeyValuePair<string, FunctionInfo>>.CopyTo(KeyValuePair<string, FunctionInfo>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, FunctionInfo>>)lib).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, FunctionInfo>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<string, FunctionInfo>>)lib).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<string, FunctionInfo>>.Remove(KeyValuePair<string, FunctionInfo> item)
        {
            return ((ICollection<KeyValuePair<string, FunctionInfo>>)lib).Remove(item);
        }

        /// <summary>
        /// Gets an enumerator for this dictionary
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, FunctionInfo>> GetEnumerator()
        {
            return lib.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
