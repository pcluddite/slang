﻿/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;
using System.Linq;
using System.Collections.Generic;
using Slang.Errors;
using Slang.Libraries;
using Slang.Types;
using System.Collections.ObjectModel;

namespace Slang.Runtime
{
    /// <summary>
    /// Manages the variables, functions, and commands declared in a given context
    /// </summary>
    public partial class Scope
    {
        #region Fields/Properties

        private Scope _super;
        private Dictionary<string, object> _variables;
        private Dictionary<string, object> _constants;
        private Dictionary<string, BlockCreator> _blocks;
        private Dictionary<string, TClass> _prototypes;
        private BinOpDictionary _binaryOps;
        private UnaryOpDictionary _unaryOps;
        private Library _functions;
        private Library _commands;

        private bool _bCollected = false;

        /// <summary>
        /// Gets the parent context. If this is the global context, returns null.
        /// </summary>
        public Scope ParentContext
        {
            get {
                return _super;
            }
        }

        #endregion

        #region Constructors

        internal Scope()
        {
        }

        internal Scope(Scope superContext)
        {
            _super = superContext;
            _functions = new Library();
            _commands = new Library();
            _variables = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            _constants = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            _blocks = new Dictionary<string, BlockCreator>(StringComparer.OrdinalIgnoreCase);
            _prototypes = new Dictionary<string, TClass>(StringComparer.OrdinalIgnoreCase);
            if (superContext == null) {
                _binaryOps = new BinOpDictionary();
                _unaryOps = new UnaryOpDictionary();
            }
            else {
                _binaryOps = superContext._binaryOps; // binary operators are always global (for now)
                _unaryOps = superContext._unaryOps; // ^^^ dito
            }
#if SHOW_OBJECTS
            Console.WriteLine();
            Console.WriteLine("ObjectContext initialized.");
            Console.WriteLine("\tHash:\t{0}", GetHashCode());
            if (_super != null) {
                Console.WriteLine("\tSuper Context:\t{0}", _super.GetHashCode());
            }
#endif
        }

        /// <summary>
        /// Loads the standard library of functions, statements, constants and code blocks
        /// </summary>
        public void LoadStandardLibrary()
        {
            _functions = new Library(
                new Library[] { 
                    new MathLibrary(this),
                    new RuntimeLibrary(this),
                    new UserIOLibrary(),
                    new AutoLibrary(),
                    new FileIOLibrary(),
                    new ProcessLibrary(),
                    new WindowLibrary(this),
                    new RegistryLibrary(),
                    new SystemLibrary()
                });
            _commands = new StatementLibrary();
            _blocks = new Dictionary<string, BlockCreator>(StringComparer.OrdinalIgnoreCase) {
                { "DO"    , (i, c) => new DoBlock(i, c) },
                { "WHILE" , (i, c) => new WhileBlock(i, c) },
                { "IF"    , (i, c) => new IfBlock(i, c) }
            };
            LoadStandardOperators();
        }

        /// <summary>
        /// Loads only the standard operators. This is called by LoadStandardLibrary(). Do not call it if you already called that.
        /// </summary>
        public void LoadStandardOperators()
        {
            _binaryOps.LoadStandardOperators();
            _unaryOps.LoadStandardOperators();
        }

        /// <summary>
        /// Adds a library that should be parsed as functions to this context
        /// </summary>
        /// <param name="lib">the library to add</param>
        public void AddLibrary(Library lib)
        {
            _functions.AddLibrary(lib);
        }

        /// <summary>
        /// Adds a library that should be parsed as commands to this context
        /// </summary>
        /// <param name="lib">the library to add</param>
        public void AddCommandLibrary(Library lib)
        {
            _commands.AddLibrary(lib);
        }

        #endregion

        #region CreateSubContext

        /// <summary>
        /// Creates a sub-context nested in this one. The sub-context inherits all variables, functions, and commands of declared in this one.
        /// </summary>
        /// <returns>the new sub-context</returns>
        public Scope CreateSubContext()
        {
            if (_bCollected) {
                throw ThrowHelper.ContextCleared();
            }
            return new Scope(this); // They're not allowed to do this themselves so it won't be null
        }

        #endregion

        #region Disposal

        /// <summary>
        /// Clears all variables, functions, and commands in this context and returns its super-context. If
        /// no super-context exists, the object will not be collected, and the same object is returned.
        /// </summary>
        /// <returns>the super context of this context</returns>
        public Scope Collect()
        {
            if (_bCollected) {
                throw ThrowHelper.ContextCleared();
            }
            else if (_super == null) {
                return this; // You won't ever get rid of me
            }
            _bCollected = true;
#if SHOW_OBJECTS
            foreach (var v in _variables) {
                Console.WriteLine("{0} = {1} collected", v.Key, v.Value);
            }
            foreach (var v in _functions) {
                Console.WriteLine("{0} function collected", v.Key);
            }
            foreach (var v in _commands) {
                Console.WriteLine("{0} command collected", v.Key);
            }
            Console.WriteLine("{0} ObjectContext collected", GetHashCode());
#endif
            _commands.Clear();
            _functions.Clear();
            _variables.Clear(); // YOU'RE FREE!!!!!
            return _super;
        }

        #endregion

        #region FindContext
        
        /// <summary>
        /// Searches for the context in which any name is declared (variable, constant, function, command or block). If no definition cannot be found, null is returned.
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <returns>the Scope in which the name is defined</returns>
        public Scope FindContext(string name)
        {
            Scope context = FindVariableContext(name);
            if (context != null) {
                return context;
            }
            context = FindConstantContext(name);
            if (context != null) {
                return context;
            }
            context = FindFunctionContext(name);
            if (context != null) {
                return context;
            }
            context = FindCommandContext(name);
            if (context != null) {
                return context;
            }
            return FindBlockContext(name);
        }

        #endregion

        #region List

        /// <summary>
        /// List all unary operators defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UnaryOperator> GetAllUnaryOperators()
        {
            return new ReadOnlyCollection<UnaryOperator>(_unaryOps);
        }
        
        /// <summary>
        /// Lists all binary operators defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BinaryOperator> GetAllBinaryOperators()
        {
            return new ReadOnlyCollection<BinaryOperator>(_binaryOps);
        }

        /// <summary>
        /// List the unary operators defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UnaryOperator> GetLocalUnaryOperators()
        {
            return new ReadOnlyCollection<UnaryOperator>(_unaryOps);
        }


        /// <summary>
        /// Lists the binary operators defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BinaryOperator> GetLocalBinaryOperators()
        {
            return new ReadOnlyCollection<BinaryOperator>(_binaryOps);
        }

        /// <summary>
        /// Lists all the operators currently defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IOperator> GetAllOperators()
        {
            Scope context = this;
            while (context != null) {
                foreach (var op in context.GetLocalOperators()) {
                    yield return op;
                }
                context = context._super;
            }
        }

        /// <summary>
        /// Lists the operators defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IOperator> GetLocalOperators()
        {
            foreach (var binop in _binaryOps)
                yield return binop;
            foreach (var unop in _unaryOps)
                yield return unop;
        }

        #endregion

        #region Specialized
        
        /// <summary>
        /// Tries to get a variable or constant from this context
        /// </summary>
        /// <returns>true if the variable was found, otherwise false.</returns>
        public bool TryGetVariable(string name, out object value)
        {
            if (_variables.TryGetValue(name, out value) || _constants.TryGetValue(name, out value)) {
                return true;
            }
            else if (_super == null) {
                return false;
            }
            else {
                return _super.TryGetVariable(name, out value);
            }
        }
        
        /// <summary>
        /// Gets a variable or constant that has been declared in this context
        /// </summary>
        /// <param name="name">variable name</param>
        /// <returns>the variable data</returns>
        public object GetVariable(string name)
        {
            object value;
            if (_constants.TryGetValue(name, out value)) {
                return value;
            }
            else if (_variables.TryGetValue(name, out value)) {
                return value;
            }
            else if (_super == null) {
                throw ThrowHelper.UndefinedObject(name);
            }
            else {
                return _super.GetVariable(name);
            }
        }

        /// <summary>
        /// Gets the value of an array variable at a given index
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <param name="indices">the index (or indices of a multidimensional array)</param>
        /// <returns>the value of the variable</returns>
        public object GetArrayAt(string name, params int[] indices)
        {
            object obj = GetVariable(name);
            if (indices != null && indices.Length > 0) {
                for (int n = 0; n < indices.Length; n++) {
                    object[] _aObj = obj as object[];
                    if (_aObj != null) {
                        if (indices[n] < _aObj.Length) {
                            obj = _aObj[indices[n]];
                        }
                        else {
                            throw ThrowHelper.IndexOutOfRange(VariableEvaluator.GetFullName(name, indices), indices[n]);
                        }
                    }
                    else {
                        throw ThrowHelper.IndexUnavailable(VariableEvaluator.GetFullName(name, indices));
                    }
                }
            }
            return obj;
        }

        internal void SetReturns(StackFrame _sframe)
        {
            GetGlobal()._constants["@error"] = new Number(_sframe.Status);
        }

        internal Scope GetGlobal()
        {
            Scope curr = this;
            while(curr._super != null) {
                curr = curr._super;
            }
            return curr;
        }

        internal void PersistReturns(StackFrame _sframe)
        {
            object statusvar;
            if (TryGetVariable("@error", out statusvar)) {
                _sframe.Status = ((Number)statusvar).ToInt();
            }
        }
        
        /// <summary>
        /// Sets the value of an array variable at a given index
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <param name="value">the new value of the array at that index</param>
        /// <param name="indices">the index (or indices of a multidimensional array)</param>
        /// <returns>the value of the variable</returns>
        public void SetArrayAt(string name, object value, params int[] indices)
        {
            object obj = GetVariable(name);
            object[] array = null;
            if (indices != null && indices.Length > 0) {
                for (int n = 0; n < indices.Length - 1; ++n) {
                    array = obj as object[];
                    if (array != null) {
                        if (indices[n] < array.Length) {
                            obj = array[indices[n]];
                        }
                        else {
                            throw ThrowHelper.IndexOutOfRange(VariableEvaluator.GetFullName(name, indices), indices[n]);
                        }
                    }
                    else {
                        throw ThrowHelper.IndexUnavailable(VariableEvaluator.GetFullName(name, indices));
                    }
                }
            }
            array[indices[indices.Length - 1]] = value;
        }
        
        #endregion

        internal static T CopyTo<T>(Scope source, T dest) where T : Scope
        {
            if (source._super == null) {
                dest._unaryOps = new UnaryOpDictionary(source._unaryOps);
                dest._binaryOps = new BinOpDictionary(source._binaryOps);
            }
            else {
                dest._super = source._super;
                dest._unaryOps = source._super._unaryOps;
                dest._binaryOps = source._super._binaryOps;
            }
            dest._variables = new Dictionary<string, object>(source._variables);
            dest._prototypes = new Dictionary<string, TClass>(source._prototypes);
            dest._functions = new Library(source._functions);
            dest._constants = new Dictionary<string, object>(source._constants);
            dest._commands = new Library(source._commands);
            dest._blocks = new Dictionary<string, BlockCreator>(source._blocks);
            return dest;
        }
    }
}
