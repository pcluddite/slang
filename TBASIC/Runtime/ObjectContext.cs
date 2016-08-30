// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using Tbasic.Errors;
using Tbasic.Libraries;
using Tbasic.Operators;

namespace Tbasic.Runtime
{
    /// <summary>
    /// Manages the variables, functions, and commands declared in a given context
    /// </summary>
    public partial class ObjectContext
    {
        #region Private Fields

        private ObjectContext _super;
        private Dictionary<string, object> _variables;
        private Dictionary<string, object> _constants;
        private Dictionary<string, BlockCreator> _blocks;
        private Dictionary<string, TClass> _prototypes;
        private BinOpDictionary _binaryOps;
        private UnaryOpDictionary _unaryOps;
        private Library _functions;
        private Library _commands;

        private bool _bCollected = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent context. If this is the global context, returns null.
        /// </summary>
        public ObjectContext ParentContext
        {
            get {
                return _super;
            }
        }

        #endregion

        #region Constructors

        internal ObjectContext()
        {
        }

        internal ObjectContext(ObjectContext superContext)
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
            _commands = new Library(
                new Library[] {
                    new StatementLibrary()
                });
            _blocks = new Dictionary<string, BlockCreator>(StringComparer.OrdinalIgnoreCase) {
                { "DO"    , (i,c) => new DoBlock(i,c) },
                { "WHILE" , (i,c) => new WhileBlock(i,c) },
                { "IF"    , (i,c) => new IfBlock(i,c) },
                { "SELECT", (i,c) => new SelectBlock(i,c) }
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
        public ObjectContext CreateSubContext()
        {
            if (_bCollected) {
                throw ThrowHelper.ContextCleared();
            }
            return new ObjectContext(this); // They're not allowed to do this themselves so it won't be null
        }

        #endregion

        #region Disposal

        /// <summary>
        /// Clears all variables, functions, and commands in this context and returns its super-context. If
        /// no super-context exists, the object will not be collected, and the same object is returned.
        /// </summary>
        /// <returns>the super context of this context</returns>
        public ObjectContext Collect()
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
        /// <returns>the ObjectContext in which the name is defined</returns>
        public ObjectContext FindContext(string name)
        {
            ObjectContext context = FindVariableContext(name);
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
            return _unaryOps.Values;
        }
        
        /// <summary>
        /// Lists all binary operators defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BinaryOperator> GetAllBinaryOperators()
        {
            return _binaryOps.Values;
        }

        /// <summary>
        /// List the unary operators defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UnaryOperator> GetLocalUnaryOperators()
        {
            return _unaryOps.Values;
        }


        /// <summary>
        /// Lists the binary operators defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BinaryOperator> GetLocalBinaryOperators()
        {
            return _binaryOps.Values;
        }

        /// <summary>
        /// Lists all the operators currently defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IOperator> GetAllOperators()
        {
            ObjectContext context = this;
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
            foreach (var binop in _binaryOps.Values)
                yield return binop;
            foreach (var unop in _unaryOps.Values)
                yield return unop;
        }

        /// <summary>
        /// This is a workaround for some generic programming. I don't like it.
        /// </summary>
        internal IEnumerable<T> GetAllOperators<T>() where T : IOperator
        {
            ObjectContext context = this;
            while (context != null) {
                foreach (var op  in context.GetLocalOperators<T>()) {
                    yield return op;
                }
                context = context._super;
            }
        }

        /// <summary>
        /// This is another workaround for generic programming. I still don't like it.
        /// </summary>
        internal IEnumerable<T> GetLocalOperators<T>() where T : IOperator
        {
            if (typeof(T) == typeof(BinaryOperator)) {
                return (IEnumerable<T>)GetLocalBinaryOperators();
            }
            else {
                return (IEnumerable<T>)GetLocalUnaryOperators();
            }
        }

        #endregion

        #region Add

        internal void AddType(TClass type)
        {
            _prototypes.Add(type.Name, type);
        }

        #endregion

        #region TryGet

        internal bool TryGetType(string typename, out TClass tclass)
        {
            if (_prototypes.TryGetValue(typename, out tclass)) {
                return true;
            }
            else if (_super == null) {
                return false;
            }
            else {
                return _super.TryGetType(typename, out tclass);
            }
        }

        #endregion

        #region Get

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
                            throw ThrowHelper.IndexOutOfRange(Variable.GetFullName(name, indices), indices[n]);
                        }
                    }
                    else {
                        throw ThrowHelper.IndexUnavailable(Variable.GetFullName(name, indices));
                    }
                }
            }
            return obj;
        }

        #endregion

        #region Set

        internal void SetReturns(RuntimeData _sframe)
        {
            SetVariable("@lasterror", _sframe.Status);
            SetVariable("@lasterr", _sframe.Status);
            SetVariable("@err", _sframe.Status);
            SetVariable("@error", _sframe.Status);
        }

        internal void PersistReturns(RuntimeData _sframe)
        {
            ObjectContext context = FindVariableContext("@error");
            if (context != null)
                _sframe.Status = (int)context.GetVariable("@error");
        }

        /// <summary>
        /// Sets a constant that will be declared in this context. Once a constant is set, it cannot be changed.
        /// </summary>
        /// <param name="name">the constant name</param>
        /// <param name="obj">the object data</param>
        public void SetConstant(string name, object obj)
        {
            ObjectContext context = FindVariableContext(name);
            if (context == null) {
                context = FindConstantContext(name);
                if (context == null) {
                    _constants.Add(name, obj); // since you can set constants only once, we don't need to use the context in which a constant was set
                }
                else {
                    throw ThrowHelper.ConstantChange();
                }
            }
            else {
                throw ThrowHelper.AlreadyDefinedAsType(name, "variable", "constant");
            }
        }

        /// <summary>
        /// Sets a variable in this context. If the variable exists, it is set in
        /// the context in which it was declared. Otherwise, it is declared in this context.
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <param name="obj">the object data</param>
        public void SetVariable(string name, object obj)
        {
            ObjectContext c = FindConstantContext(name);
            if (c != null) {
                throw ThrowHelper.ConstantChange();
            }
            c = FindVariableContext(name);
            if (c == null) {
                _variables.Add(name, obj);
#if SHOW_OBJECTS
                Console.WriteLine("{1} = {2} declared in {0}", GetHashCode(), name, obj);
#endif
            }
            else {
                c._variables[name] = obj;
#if SHOW_OBJECTS
                Console.WriteLine("{1} = {2} set in {0}", c.GetHashCode(), name, obj);
#endif
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
            object[] array = GetVariable(name) as object[];
            if (indices != null && indices.Length > 0) {
                for (int n = 0; n < indices.Length - 1; ++n) {
                    if (array != null) {
                        if (indices[n] < array.Length) {
                            array = array[indices[n]] as object[];
                        }
                        else {
                            throw ThrowHelper.IndexOutOfRange(Variable.GetFullName(name, indices), indices[n]);
                        }
                    }
                    else {
                        throw ThrowHelper.IndexUnavailable(Variable.GetFullName(name, indices));
                    }
                }
            }
            array[indices[indices.Length - 1]] = value;
        }
        
        #endregion

        internal static T CopyFrom<T>(ObjectContext other) where T : ObjectContext, new()
        {
            T cloned = new T();
            if (other._super == null) {
                cloned._unaryOps = new UnaryOpDictionary(other._unaryOps);
                cloned._binaryOps = new BinOpDictionary(other._binaryOps);
            }
            else {
                cloned._super = other._super;
                cloned._unaryOps = other._super._unaryOps;
                cloned._binaryOps = other._super._binaryOps;
            }
            cloned._variables = new Dictionary<string, object>(other._variables);
            cloned._prototypes = new Dictionary<string, TClass>(other._prototypes);
            cloned._functions = new Library(other._functions);
            cloned._constants = new Dictionary<string, object>(other._constants);
            cloned._commands = new Library(other._commands);
            cloned._blocks = new Dictionary<string, BlockCreator>(other._blocks);
            return cloned;
        }
    }
}
