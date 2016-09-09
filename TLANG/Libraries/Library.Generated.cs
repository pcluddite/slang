// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using TLang.Types;

namespace TLang.Libraries
{
    public partial class Library
	{
        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<TResult>(string key, Func<TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 0;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
        public void Add(string key, Action value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 0;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<T1, TResult>(string key, Func<T1, TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 1;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
        public void Add<T1>(string key, Action<T1> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 1;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<T1, T2, TResult>(string key, Func<T1, T2, TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 2;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
        public void Add<T1, T2>(string key, Action<T1, T2> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 2;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<T1, T2, T3, TResult>(string key, Func<T1, T2, T3, TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 3;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
        public void Add<T1, T2, T3>(string key, Action<T1, T2, T3> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 3;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<T1, T2, T3, T4, TResult>(string key, Func<T1, T2, T3, T4, TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 4;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
        public void Add<T1, T2, T3, T4>(string key, Action<T1, T2, T3, T4> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 4;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<T1, T2, T3, T4, T5, TResult>(string key, Func<T1, T2, T3, T4, T5, TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 5;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
        public void Add<T1, T2, T3, T4, T5>(string key, Action<T1, T2, T3, T4, T5> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 5;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
		/// <typeparam name="T6">the type for argument 6</typeparam>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<T1, T2, T3, T4, T5, T6, TResult>(string key, Func<T1, T2, T3, T4, T5, T6, TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 6;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
		/// <typeparam name="T6">the type for argument 6</typeparam>
        public void Add<T1, T2, T3, T4, T5, T6>(string key, Action<T1, T2, T3, T4, T5, T6> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 6;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
		/// <typeparam name="T6">the type for argument 6</typeparam>
		/// <typeparam name="T7">the type for argument 7</typeparam>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<T1, T2, T3, T4, T5, T6, T7, TResult>(string key, Func<T1, T2, T3, T4, T5, T6, T7, TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 7;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
		/// <typeparam name="T6">the type for argument 6</typeparam>
		/// <typeparam name="T7">the type for argument 7</typeparam>
        public void Add<T1, T2, T3, T4, T5, T6, T7>(string key, Action<T1, T2, T3, T4, T5, T6, T7> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 7;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
		/// <typeparam name="T6">the type for argument 6</typeparam>
		/// <typeparam name="T7">the type for argument 7</typeparam>
		/// <typeparam name="T8">the type for argument 8</typeparam>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(string key, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 8;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
		/// <typeparam name="T6">the type for argument 6</typeparam>
		/// <typeparam name="T7">the type for argument 7</typeparam>
		/// <typeparam name="T8">the type for argument 8</typeparam>
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8>(string key, Action<T1, T2, T3, T4, T5, T6, T7, T8> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 8;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
		/// <typeparam name="T6">the type for argument 6</typeparam>
		/// <typeparam name="T7">the type for argument 7</typeparam>
		/// <typeparam name="T8">the type for argument 8</typeparam>
		/// <typeparam name="T9">the type for argument 9</typeparam>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(string key, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 9;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
		/// <typeparam name="T6">the type for argument 6</typeparam>
		/// <typeparam name="T7">the type for argument 7</typeparam>
		/// <typeparam name="T8">the type for argument 8</typeparam>
		/// <typeparam name="T9">the type for argument 9</typeparam>
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 9;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that returns a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
		/// <typeparam name="T6">the type for argument 6</typeparam>
		/// <typeparam name="T7">the type for argument 7</typeparam>
		/// <typeparam name="T8">the type for argument 8</typeparam>
		/// <typeparam name="T9">the type for argument 9</typeparam>
		/// <typeparam name="T10">the type for argument 10</typeparam>
        /// <typeparam name="TResult">the type that this function returns</typeparam>
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(string key, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 10;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

        /// <summary>
        /// Adds a native function that does not return a result
        /// </summary>
        /// <param name="key">the name of the function</param>
        /// <param name="value">the delegate to the function</param>
        /// <param name="evaluate">True if function should have its arguments evaluated before they are passed, false otherwise</param>
		/// <param name="requiredArgs">The number of arguments that this function is required to have. Values less than zero mean all arguments are required.</param>
		/// <typeparam name="T1">the type for argument 1</typeparam>
		/// <typeparam name="T2">the type for argument 2</typeparam>
		/// <typeparam name="T3">the type for argument 3</typeparam>
		/// <typeparam name="T4">the type for argument 4</typeparam>
		/// <typeparam name="T5">the type for argument 5</typeparam>
		/// <typeparam name="T6">the type for argument 6</typeparam>
		/// <typeparam name="T7">the type for argument 7</typeparam>
		/// <typeparam name="T8">the type for argument 8</typeparam>
		/// <typeparam name="T9">the type for argument 9</typeparam>
		/// <typeparam name="T10">the type for argument 10</typeparam>
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string key, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> value, bool evaluate = true, int requiredArgs = -1)
        {
			if (requiredArgs < 0)
				requiredArgs = 10;
            lib.Add(key, new CallData(value, requiredArgs, evaluate));
        }

	}
}
