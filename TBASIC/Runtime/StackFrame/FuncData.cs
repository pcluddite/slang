// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Errors;
using Tbasic.Parsing;

namespace Tbasic.Runtime
{
    /// <summary>
    /// Manages parameters and other data passed to a function or subroutine
    /// </summary>
    public class FuncData : ICloneable
    {
        private List<object> _params = new List<object>();

        /// <summary>
        /// Gets a list of the parameters passed to the function
        /// </summary>
        public IList<object> Parameters
        {
            get {
                return _params;
            }
        }

        /// <summary>
        /// The executer that called the function
        /// </summary>
        public Executer StackExecuter { get; private set; }

        /// <summary>
        /// Gets or sets the current context of stack executer
        /// </summary>
        public ObjectContext Context
        {
            get {
                return StackExecuter.Context;
            }
            set {
                StackExecuter.Context = value;
            }
        }

        /// <summary>
        /// The Tbasic function as text. This value cannot be changed after it has been set in the constructor.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// The name of the function (the first parameter)
        /// </summary>
        public string Name
        {
            get {
                if (_params.Count > 0) {
                    return (_params[0] ?? string.Empty).ToString();
                }
                else {
                    return string.Empty;
                }
            }
            set {
                if (_params.Count == 0) {
                    _params.Add(value);
                }
                else {
                    _params.Insert(0, value);
                }
            }
        }

        /// <summary>
        /// Gets the number of parameters in this collection
        /// </summary>
        public int ParameterCount
        {
            get {
                return _params.Count;
            }
        }
        
        /// <summary>
        /// Gets or sets the status that the function returned. Default is ErrorSuccess.OK
        /// </summary>
        public int Status { get; set; } = ErrorSuccess.OK;

        /// <summary>
        /// Gets or sets the return data for the function
        /// </summary>
        public object Data { get; set; } = null;

        /// <summary>
        /// Constructs this object
        /// </summary>
        /// <param name="exec">the execution that called the function</param>
        public FuncData(Executer exec)
        {
            StackExecuter = exec;
        }

        /// <summary>
        /// Constructs this object
        /// </summary>
        /// <param name="parameters">the parameters of the function</param>
        /// <param name="exec">the execution that called the function</param>
        public FuncData(Executer exec, IEnumerable<object> parameters)
            : this(exec)
        {
            _params.AddRange(parameters);
        }

        /// <summary>
        /// Constructs this object
        /// </summary>
        /// <param name="text">the line that executed this function, this will be parsed like the Windows Command Prompt</param>
        /// <param name="exec">the execution that called the function</param>
        public FuncData(Executer exec, string text)
            : this(exec)
        {
            CmdLine line = new CmdLine(text);
            Text = text;
            _params.AddRange(line);
        }

        /// <summary>
        /// Assigns new data to a parameter
        /// </summary>
        /// <param name="index">The index of the argument</param>
        /// <param name="data">The new string data to assign</param>
        public void SetAt(int index, object data)
        {
            if (index < _params.Count) {
                _params[index] = data;
            }
        }

        /// <summary>
        /// Throws an ArgumentException if the number of parameters does not match a specified count
        /// </summary>
        /// <param name="count">the number of parameters expected</param>
        /// <exception cref="ArgumentException">thrown if argument count is not the same as the parameter</exception>
        public void AssertCount(int count)
        {
            if (_params.Count != count) {
                throw new ArgumentException(string.Format("{0} does not take {1} parameter{2}", Name.ToUpper(), _params.Count - 1,
                _params.Count == 2 ? "" : "s"));
            }
        }

        /// <summary>
        /// Throws an ArgumentException if the number of parameters does not match a count in a specified range
        /// </summary>
        /// <param name="atLeast">the least number of arguments this function takes</param>
        /// <param name="atMost">the most number of arguments this function takes</param>
        /// <exception cref="ArgumentException">thrown if argument count is not the same as the parameter</exception>
        public void AssertCount(int atLeast, int atMost)
        {
            if (_params.Count < atLeast || _params.Count > atMost) {
                throw new ArgumentException(string.Format("{0} does not take {1} parameter{2}", Name.ToUpper(), _params.Count - 1,
                _params.Count == 2 ? "" : "s"));
            }
        }

        /// <summary>
        /// Throws an ArgumentException if the number of parameters is not at least a certain value
        /// </summary>
        /// <param name="atLeast">the least number of arguments this function takes</param>
        /// <exception cref="ArgumentException">thrown if argument count is not at least a certain number</exception>
        public void AssertAtLeast(int atLeast)
        {
            if (_params.Count < atLeast) {
                throw new ArgumentException(string.Format("{0} must have at least {1} parameter{2}", Name.ToUpper(), atLeast - 1,
                atLeast == 2 ? "" : "s"));
            }
        }

        /// <summary>
        /// Returns the parameter at an index
        /// </summary>
        /// <param name="index">The index of the argument</param>
        /// <exception cref="IndexOutOfRangeException">thrown if the argument is out of range</exception>
        /// <returns></returns>
        public object GetAt(int index)
        {
            try {
                return _params[index];
            }
            catch(ArgumentOutOfRangeException ex) {
                throw new IndexOutOfRangeException(ex.Message);
            }
        }

        /// <summary>
        /// Returns the parameter as an integer if the integer is within a given range.
        /// </summary>
        /// <param name="index">the index of the argument</param>
        /// <param name="lower">the inclusive lower bound</param>
        /// <param name="upper">the inclusive upper bound</param>
        /// <exception cref="ArgumentOutOfRangeException">thrown if the paramater is out of range</exception>
        /// <returns></returns>
        public int GetFromRange(int index, int lower, int upper)
        {
            int n = GetAt<int>(index);
            if (n < lower || n > upper) {
                throw new ArgumentOutOfRangeException(string.Format("Parameter {0} expected to be integer between {1} and {2}", index, lower, upper));
            }
            return n;
        }

        /// <summary>
        /// Gets a parameter at a given index as a specified type
        /// </summary>
        /// <typeparam name="T">the type to convert the object</typeparam>
        /// <param name="index">the zero-based index of the parameter</param>
        /// <exception cref="InvalidCastException">object was not able to be converted to given type</exception>
        /// <returns></returns>
        public T GetAt<T>(int index)
        {
            T ret;
            if (Evaluator.TryConvert(GetAt(index), out ret)) {
                return ret;
            }
            throw new InvalidCastException(string.Format("Expected parameter {0} to be of type {1}", index, typeof(T).Name));
        }

        /// <summary>
        /// Gets an argument from a set of predefined values
        /// </summary>
        /// <param name="index">The index of the argument</param>
        /// <param name="typeName">The type the argument represents</param>
        /// <param name="values">Acceptable string values</param>
        /// <exception cref="ArgumentException">thrown if the argument is not in the acceptable list of values</exception>
        /// <returns></returns>
        public string GetEnum(int index, string typeName, params string[] values)
        {
            string arg = GetAt<string>(index);
            foreach (string val in values) {
                if (val.EqualsIgnoreCase(arg)) {
                    return arg;
                }
            }
            throw new ArgumentException("expected parameter " + index + " to be of type " + typeName);
        }

        /// <summary>
        /// Adds a parameter to the end of this collection
        /// </summary>
        /// <param name="param"></param>
        public void Add(object param)
        {
            _params.Add(param);
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="param"></param>
        public void AddRange(params object[] param)
        {
            AddRange(param);
        }

        /// <summary>
        /// Adds a number of parameters to this collection
        /// </summary>
        /// <param name="param"></param>
        public void AddRange(IEnumerable<object> param)
        {
            foreach (object p in param)
                Add(p);
        }

        /// <summary>
        /// Clones this
        /// </summary>
        /// <returns>A new object with the same data</returns>
        public FuncData Clone()
        {
            FuncData clone = new FuncData(StackExecuter);
            clone.Text = Text;
            if (_params == null) {
                clone._params = new List<object>();
            }
            else {
                clone._params.AddRange(_params);
            }
            clone.Status = Status;
            clone.Data = Data;
            return clone;
        }

        /// <summary>
        /// Copies all properties of another into this one
        /// </summary>
        /// <param name="other"></param>
        public void CopyFrom(FuncData other)
        {
            FuncData clone = other.Clone();
            StackExecuter = clone.StackExecuter;
            Text = clone.Text;
            _params = clone._params;
            Status = clone.Status;
            Data = clone.Data;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
