// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Tbasic.Errors;
using Tbasic.Parsing;
using Tbasic.Types;

namespace Tbasic.Runtime
{
    /// <summary>
    /// Manages parameters and other data passed to a function or subroutine
    /// </summary>
    public partial class StackData : ICloneable
	{
        /// <summary>
        /// Adds a parameter to the end of the parameter list
        /// </summary>
        /// <param name="value">the IRuntimeObject to add</param>
        public void Add(IRuntimeObject value)
        {
            _params.Add(value);
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<IRuntimeObject> collection)
        {
            _params.AddRange(collection);
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(params IRuntimeObject[] collection)
        {
            _params.AddRange(collection);
        }

        /// <summary>
        /// Assigns a new value to a parameter at a given index
        /// </summary>
        /// <param name="index">The index of the argument</param>
        /// <param name="value">the new IRuntimeObject data to assign</param>
        public void Set(int index, IRuntimeObject value)
        {
            if ((uint)index >= (uint)_params.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            _params[index] = value;
        }

        /// <summary>
        /// Adds a parameter to the end of the parameter list
        /// </summary>
        /// <param name="value">the string to add</param>
        public void Add(string value)
        {
            _params.Add((TbasicString)value);
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<string> collection)
        {
			foreach(string value in collection) {
				_params.Add((TbasicString)value);
			}
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(params string[] collection)
        {
			foreach(string value in collection) {
				_params.Add((TbasicString)value);
			}
        }

        /// <summary>
        /// Assigns a new value to a parameter at a given index
        /// </summary>
        /// <param name="index">The index of the argument</param>
        /// <param name="value">the new string data to assign</param>
        public void Set(int index, string value)
        {
            if ((uint)index >= (uint)_params.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            _params[index] = (TbasicString)value;
        }

        /// <summary>
        /// Adds a parameter to the end of the parameter list
        /// </summary>
        /// <param name="value">the Number to add</param>
        public void Add(Number value)
        {
            _params.Add( value);
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<Number> collection)
        {
			foreach(Number value in collection) {
				_params.Add(value);
			}
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(params Number[] collection)
        {
			foreach(Number value in collection) {
				_params.Add(value);
			}
        }

        /// <summary>
        /// Assigns a new value to a parameter at a given index
        /// </summary>
        /// <param name="index">The index of the argument</param>
        /// <param name="value">the new Number data to assign</param>
        public void Set(int index, Number value)
        {
            if ((uint)index >= (uint)_params.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            _params[index] =  value;
        }

        /// <summary>
        /// Adds a parameter to the end of the parameter list
        /// </summary>
        /// <param name="value">the bool to add</param>
        public void Add(bool value)
        {
            _params.Add((TbasicBoolean)value);
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<bool> collection)
        {
			foreach(bool value in collection) {
				_params.Add((TbasicBoolean)value);
			}
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(params bool[] collection)
        {
			foreach(bool value in collection) {
				_params.Add((TbasicBoolean)value);
			}
        }

        /// <summary>
        /// Assigns a new value to a parameter at a given index
        /// </summary>
        /// <param name="index">The index of the argument</param>
        /// <param name="value">the new bool data to assign</param>
        public void Set(int index, bool value)
        {
            if ((uint)index >= (uint)_params.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            _params[index] = (TbasicBoolean)value;
        }

        /// <summary>
        /// Adds a parameter to the end of the parameter list
        /// </summary>
        /// <param name="value">the object to add</param>
        public void Add(object value)
        {
            _params.Add((TbasicNative)value);
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<object> collection)
        {
			foreach(object value in collection) {
				_params.Add((TbasicNative)value);
			}
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(params object[] collection)
        {
			foreach(object value in collection) {
				_params.Add((TbasicNative)value);
			}
        }

        /// <summary>
        /// Assigns a new value to a parameter at a given index
        /// </summary>
        /// <param name="index">The index of the argument</param>
        /// <param name="value">the new object data to assign</param>
        public void Set(int index, object value)
        {
            if ((uint)index >= (uint)_params.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            _params[index] = (TbasicNative)value;
        }

	}
}
