﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using TLang.Errors;
using TLang.Types;

namespace TLang.Runtime
{
    // Autogenerated. Do not modify this file.
    public partial class ObjectContext
    {
        #region Generated Function.CallData methods 

        /// <summary>
        /// Sets a Function in this context. If the Function exists, it is set in
        /// the context in which it was declared. Otherwise, it is declared in this context.
        /// </summary>
        public void SetFunction(string name, CallData value)
        {
            ObjectContext c = FindFunctionContext(name);
            if (c != null) {
                c._functions[name] = value;
            }
            else {
                AddFunction(name, value);
            }
        }

        /// <summary>
        /// Adds a Function to this context. If the Function exists, an exception is thrown
        /// </summary>
        public void AddFunction(string name, CallData value)
        {
			try {
				_functions.Add(name, value);
			}
			catch(ArgumentException) {
				throw new DuplicateDefinitionException(name);
			}
        }

        /// <summary>
        /// Tries to get a Function from this context
        /// </summary>
        /// <returns>true if the Function was found, otherwise false.</returns>
        public bool TryGetFunction(string name, out CallData value)
        {
            if (_functions.TryGetValue(name, out value)) {
                return true;
            }
            else if (_super == null) {
                return false;
            }
            else {
                return _super.TryGetFunction(name, out value);
            }
        }

        /// <summary>
        /// Gets a Function if it exists, throws an ArgumentException otherwise
        /// </summary>
        /// <param name="name">the Function as a string</param>
        /// <exception cref="ArgumentException">thrown if the Function is undefined</exception>
        /// <returns></returns>
        public CallData GetFunction(string name)
        {
            CallData value;
            if (_functions.TryGetValue(name, out value)) {
                return value;
            }
            else if (_super == null) {
                throw ThrowHelper.UndefinedObject(name);
            }
            else {
                return _super.GetFunction(name);
            }
        }

        /// <summary>
        /// Searches for the context in which a Function is declared. If the Function cannot be found, null is returned.
        /// </summary>
        /// <param name="name">the block name</param>
        /// <returns>the ObjectContext in which the block is declared</returns>
        public ObjectContext FindFunctionContext(string name)
        {
            if (_functions.ContainsKey(name)) {
                return this;
            }
            else if (_super == null) {
                return null;
            }
            else {
                return _super.FindFunctionContext(name);
            }
        }

        /// <summary>
        /// Lists all the Functions currently defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, CallData>> GetLocalFunctions()
        {
            return _functions;
        }

        /// <summary>
        /// Lists all the Functions currently defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, CallData>> GetAllFunctions()
        {
            ObjectContext context = this;
            while (context != null) {
                foreach (var value in context.GetLocalFunctions()) {
                    yield return value;
                }
                context = context._super;
            }
        }

        /// <summary>
        /// Removes a Function from this context
        /// </summary>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemoveFunction(string name)
        {
            return _functions.Remove(name);
        }

        /// <summary>
		/// Adds an alias for a Function in this context
		/// </summary>
		/// <param name="name">the name of the Function</param>
		/// <param name="alias">the alternative name for the Function</param>
		public void AddFunctionAlias(string name, string alias)
		{
			AddFunction(alias, GetFunction(name));
		}

        /// <summary>
		/// Assigns a new name for a Function
		/// </summary>
		/// <param name="name">the current name of the Function</param>
		/// <param name="newname">the new name for the Function</param>
		public void RenameFunction(string name, string newname)
		{
			ObjectContext context = FindFunctionContext(name);
			if (context == null)
				throw ThrowHelper.UndefinedObject(name);
			context._functions.Add(newname,  context._functions[name]);
			context._functions.Remove(name);
		}

        #endregion

        #region Generated Command.CallData methods 

        /// <summary>
        /// Sets a Command in this context. If the Command exists, it is set in
        /// the context in which it was declared. Otherwise, it is declared in this context.
        /// </summary>
        public void SetCommand(string name, CallData value)
        {
            ObjectContext c = FindCommandContext(name);
            if (c != null) {
                c._commands[name] = value;
            }
            else {
                AddCommand(name, value);
            }
        }

        /// <summary>
        /// Adds a Command to this context. If the Command exists, an exception is thrown
        /// </summary>
        public void AddCommand(string name, CallData value)
        {
			try {
				_commands.Add(name, value);
			}
			catch(ArgumentException) {
				throw new DuplicateDefinitionException(name);
			}
        }

        /// <summary>
        /// Tries to get a Command from this context
        /// </summary>
        /// <returns>true if the Command was found, otherwise false.</returns>
        public bool TryGetCommand(string name, out CallData value)
        {
            if (_commands.TryGetValue(name, out value)) {
                return true;
            }
            else if (_super == null) {
                return false;
            }
            else {
                return _super.TryGetCommand(name, out value);
            }
        }

        /// <summary>
        /// Gets a Command if it exists, throws an ArgumentException otherwise
        /// </summary>
        /// <param name="name">the Command as a string</param>
        /// <exception cref="ArgumentException">thrown if the Command is undefined</exception>
        /// <returns></returns>
        public CallData GetCommand(string name)
        {
            CallData value;
            if (_commands.TryGetValue(name, out value)) {
                return value;
            }
            else if (_super == null) {
                throw ThrowHelper.UndefinedObject(name);
            }
            else {
                return _super.GetCommand(name);
            }
        }

        /// <summary>
        /// Searches for the context in which a Command is declared. If the Command cannot be found, null is returned.
        /// </summary>
        /// <param name="name">the block name</param>
        /// <returns>the ObjectContext in which the block is declared</returns>
        public ObjectContext FindCommandContext(string name)
        {
            if (_commands.ContainsKey(name)) {
                return this;
            }
            else if (_super == null) {
                return null;
            }
            else {
                return _super.FindCommandContext(name);
            }
        }

        /// <summary>
        /// Lists all the Commands currently defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, CallData>> GetLocalCommands()
        {
            return _commands;
        }

        /// <summary>
        /// Lists all the Commands currently defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, CallData>> GetAllCommands()
        {
            ObjectContext context = this;
            while (context != null) {
                foreach (var value in context.GetLocalCommands()) {
                    yield return value;
                }
                context = context._super;
            }
        }

        /// <summary>
        /// Removes a Command from this context
        /// </summary>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemoveCommand(string name)
        {
            return _commands.Remove(name);
        }

        /// <summary>
		/// Adds an alias for a Command in this context
		/// </summary>
		/// <param name="name">the name of the Command</param>
		/// <param name="alias">the alternative name for the Command</param>
		public void AddCommandAlias(string name, string alias)
		{
			AddCommand(alias, GetCommand(name));
		}

        /// <summary>
		/// Assigns a new name for a Command
		/// </summary>
		/// <param name="name">the current name of the Command</param>
		/// <param name="newname">the new name for the Command</param>
		public void RenameCommand(string name, string newname)
		{
			ObjectContext context = FindCommandContext(name);
			if (context == null)
				throw ThrowHelper.UndefinedObject(name);
			context._commands.Add(newname,  context._commands[name]);
			context._commands.Remove(name);
		}

        #endregion

        #region Generated Function.TBasicFunction methods 

        /// <summary>
        /// Sets a Function in this context. If the Function exists, it is set in
        /// the context in which it was declared. Otherwise, it is declared in this context.
        /// </summary>
        public void SetFunction(string name, TBasicFunction value)
        {
            ObjectContext c = FindFunctionContext(name);
            if (c != null) {
                c._functions[name] = value;
            }
            else {
                AddFunction(name, value);
            }
        }

        /// <summary>
        /// Adds a Function to this context. If the Function exists, an exception is thrown
        /// </summary>
        public void AddFunction(string name, TBasicFunction value)
        {
			try {
				_functions.Add(name, value);
			}
			catch(ArgumentException) {
				throw new DuplicateDefinitionException(name);
			}
        }

        /// <summary>
        /// Tries to get a Function from this context
        /// </summary>
        /// <returns>true if the Function was found, otherwise false.</returns>
        public bool TryGetFunction(string name, out TBasicFunction value)
        {
            CallData tmp;
            if (_functions.TryGetValue(name, out tmp)) {
                value = (TBasicFunction)tmp;
                return true;
            }
            else if (_super == null) {
                value = default(TBasicFunction);
                return false;
            }
            else {
                return _super.TryGetFunction(name, out value);
            }
        }

        #endregion

        #region Generated Command.TBasicFunction methods 

        /// <summary>
        /// Sets a Command in this context. If the Command exists, it is set in
        /// the context in which it was declared. Otherwise, it is declared in this context.
        /// </summary>
        public void SetCommand(string name, TBasicFunction value)
        {
            ObjectContext c = FindCommandContext(name);
            if (c != null) {
                c._commands[name] = value;
            }
            else {
                AddCommand(name, value);
            }
        }

        /// <summary>
        /// Adds a Command to this context. If the Command exists, an exception is thrown
        /// </summary>
        public void AddCommand(string name, TBasicFunction value)
        {
			try {
				_commands.Add(name, value);
			}
			catch(ArgumentException) {
				throw new DuplicateDefinitionException(name);
			}
        }

        /// <summary>
        /// Tries to get a Command from this context
        /// </summary>
        /// <returns>true if the Command was found, otherwise false.</returns>
        public bool TryGetCommand(string name, out TBasicFunction value)
        {
            CallData tmp;
            if (_commands.TryGetValue(name, out tmp)) {
                value = (TBasicFunction)tmp;
                return true;
            }
            else if (_super == null) {
                value = default(TBasicFunction);
                return false;
            }
            else {
                return _super.TryGetCommand(name, out value);
            }
        }

        #endregion

        #region Generated Block.BlockCreator methods 

        /// <summary>
        /// Sets a Block in this context. If the Block exists, it is set in
        /// the context in which it was declared. Otherwise, it is declared in this context.
        /// </summary>
        public void SetBlock(string name, BlockCreator value)
        {
            ObjectContext c = FindBlockContext(name);
            if (c != null) {
                c._blocks[name] = value;
            }
            else {
                AddBlock(name, value);
            }
        }

        /// <summary>
        /// Adds a Block to this context. If the Block exists, an exception is thrown
        /// </summary>
        public void AddBlock(string name, BlockCreator value)
        {
			try {
				_blocks.Add(name, value);
			}
			catch(ArgumentException) {
				throw new DuplicateDefinitionException(name);
			}
        }

        /// <summary>
        /// Tries to get a Block from this context
        /// </summary>
        /// <returns>true if the Block was found, otherwise false.</returns>
        public bool TryGetBlock(string name, out BlockCreator value)
        {
            if (_blocks.TryGetValue(name, out value)) {
                return true;
            }
            else if (_super == null) {
                return false;
            }
            else {
                return _super.TryGetBlock(name, out value);
            }
        }

        /// <summary>
        /// Gets a Block if it exists, throws an ArgumentException otherwise
        /// </summary>
        /// <param name="name">the Block as a string</param>
        /// <exception cref="ArgumentException">thrown if the Block is undefined</exception>
        /// <returns></returns>
        public BlockCreator GetBlock(string name)
        {
            BlockCreator value;
            if (_blocks.TryGetValue(name, out value)) {
                return value;
            }
            else if (_super == null) {
                throw ThrowHelper.UndefinedObject(name);
            }
            else {
                return _super.GetBlock(name);
            }
        }

        /// <summary>
        /// Searches for the context in which a Block is declared. If the Block cannot be found, null is returned.
        /// </summary>
        /// <param name="name">the block name</param>
        /// <returns>the ObjectContext in which the block is declared</returns>
        public ObjectContext FindBlockContext(string name)
        {
            if (_blocks.ContainsKey(name)) {
                return this;
            }
            else if (_super == null) {
                return null;
            }
            else {
                return _super.FindBlockContext(name);
            }
        }

        /// <summary>
        /// Lists all the Blocks currently defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, BlockCreator>> GetLocalBlocks()
        {
            return _blocks;
        }

        /// <summary>
        /// Lists all the Blocks currently defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, BlockCreator>> GetAllBlocks()
        {
            ObjectContext context = this;
            while (context != null) {
                foreach (var value in context.GetLocalBlocks()) {
                    yield return value;
                }
                context = context._super;
            }
        }

        /// <summary>
        /// Removes a Block from this context
        /// </summary>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemoveBlock(string name)
        {
            return _blocks.Remove(name);
        }

        /// <summary>
		/// Adds an alias for a Block in this context
		/// </summary>
		/// <param name="name">the name of the Block</param>
		/// <param name="alias">the alternative name for the Block</param>
		public void AddBlockAlias(string name, string alias)
		{
			AddBlock(alias, GetBlock(name));
		}

        /// <summary>
		/// Assigns a new name for a Block
		/// </summary>
		/// <param name="name">the current name of the Block</param>
		/// <param name="newname">the new name for the Block</param>
		public void RenameBlock(string name, string newname)
		{
			ObjectContext context = FindBlockContext(name);
			if (context == null)
				throw ThrowHelper.UndefinedObject(name);
			context._blocks.Add(newname,  context._blocks[name]);
			context._blocks.Remove(name);
		}

        #endregion

        #region Generated Type.TClass methods 

        /// <summary>
        /// Sets a Type in this context. If the Type exists, it is set in
        /// the context in which it was declared. Otherwise, it is declared in this context.
        /// </summary>
        public void SetType(string name, TClass value)
        {
            ObjectContext c = FindTypeContext(name);
            if (c != null) {
                c._prototypes[name] = value;
            }
            else {
                AddType(name, value);
            }
        }

        /// <summary>
        /// Adds a Type to this context. If the Type exists, an exception is thrown
        /// </summary>
        public void AddType(string name, TClass value)
        {
			try {
				_prototypes.Add(name, value);
			}
			catch(ArgumentException) {
				throw new DuplicateDefinitionException(name);
			}
        }

        /// <summary>
        /// Tries to get a Type from this context
        /// </summary>
        /// <returns>true if the Type was found, otherwise false.</returns>
        public bool TryGetType(string name, out TClass value)
        {
            if (_prototypes.TryGetValue(name, out value)) {
                return true;
            }
            else if (_super == null) {
                return false;
            }
            else {
                return _super.TryGetType(name, out value);
            }
        }

        /// <summary>
        /// Gets a Type if it exists, throws an ArgumentException otherwise
        /// </summary>
        /// <param name="name">the Type as a string</param>
        /// <exception cref="ArgumentException">thrown if the Type is undefined</exception>
        /// <returns></returns>
        public TClass GetType(string name)
        {
            TClass value;
            if (_prototypes.TryGetValue(name, out value)) {
                return value;
            }
            else if (_super == null) {
                throw ThrowHelper.UndefinedObject(name);
            }
            else {
                return _super.GetType(name);
            }
        }

        /// <summary>
        /// Searches for the context in which a Type is declared. If the Type cannot be found, null is returned.
        /// </summary>
        /// <param name="name">the block name</param>
        /// <returns>the ObjectContext in which the block is declared</returns>
        public ObjectContext FindTypeContext(string name)
        {
            if (_prototypes.ContainsKey(name)) {
                return this;
            }
            else if (_super == null) {
                return null;
            }
            else {
                return _super.FindTypeContext(name);
            }
        }

        /// <summary>
        /// Lists all the Types currently defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, TClass>> GetLocalTypes()
        {
            return _prototypes;
        }

        /// <summary>
        /// Lists all the Types currently defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, TClass>> GetAllTypes()
        {
            ObjectContext context = this;
            while (context != null) {
                foreach (var value in context.GetLocalTypes()) {
                    yield return value;
                }
                context = context._super;
            }
        }

        /// <summary>
        /// Removes a Type from this context
        /// </summary>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemoveType(string name)
        {
            return _prototypes.Remove(name);
        }

        /// <summary>
		/// Adds an alias for a Type in this context
		/// </summary>
		/// <param name="name">the name of the Type</param>
		/// <param name="alias">the alternative name for the Type</param>
		public void AddTypeAlias(string name, string alias)
		{
			AddType(alias, GetType(name));
		}

        /// <summary>
		/// Assigns a new name for a Type
		/// </summary>
		/// <param name="name">the current name of the Type</param>
		/// <param name="newname">the new name for the Type</param>
		public void RenameType(string name, string newname)
		{
			ObjectContext context = FindTypeContext(name);
			if (context == null)
				throw ThrowHelper.UndefinedObject(name);
			context._prototypes.Add(newname,  context._prototypes[name]);
			context._prototypes.Remove(name);
		}

        #endregion

        #region Generated BinaryOperator.BinaryOperator methods 

        /// <summary>
        /// Sets a BinaryOperator in this context. If the BinaryOperator exists, it is set in
        /// the context in which it was declared. Otherwise, it is declared in this context.
        /// </summary>
        public void SetBinaryOperator(string name, BinaryOperator value)
        {
            ObjectContext c = FindBinaryOperatorContext(name);
            if (c != null) {
                c._binaryOps[name] = value;
            }
            else {
                AddBinaryOperator(name, value);
            }
        }

        /// <summary>
        /// Adds a BinaryOperator to this context. If the BinaryOperator exists, an exception is thrown
        /// </summary>
        public void AddBinaryOperator(string name, BinaryOperator value)
        {
			try {
				_binaryOps.Add(name, value);
			}
			catch(ArgumentException) {
				throw new DuplicateDefinitionException(name);
			}
        }

        /// <summary>
        /// Tries to get a BinaryOperator from this context
        /// </summary>
        /// <returns>true if the BinaryOperator was found, otherwise false.</returns>
        public bool TryGetBinaryOperator(string name, out BinaryOperator value)
        {
            if (_binaryOps.TryGetValue(name, out value)) {
                return true;
            }
            else if (_super == null) {
                return false;
            }
            else {
                return _super.TryGetBinaryOperator(name, out value);
            }
        }

        /// <summary>
        /// Gets a BinaryOperator if it exists, throws an ArgumentException otherwise
        /// </summary>
        /// <param name="name">the BinaryOperator as a string</param>
        /// <exception cref="ArgumentException">thrown if the BinaryOperator is undefined</exception>
        /// <returns></returns>
        public BinaryOperator GetBinaryOperator(string name)
        {
            BinaryOperator value;
            if (_binaryOps.TryGetValue(name, out value)) {
                return value;
            }
            else if (_super == null) {
                throw ThrowHelper.UndefinedObject(name);
            }
            else {
                return _super.GetBinaryOperator(name);
            }
        }

        /// <summary>
        /// Searches for the context in which a BinaryOperator is declared. If the BinaryOperator cannot be found, null is returned.
        /// </summary>
        /// <param name="name">the block name</param>
        /// <returns>the ObjectContext in which the block is declared</returns>
        public ObjectContext FindBinaryOperatorContext(string name)
        {
            if (_binaryOps.ContainsKey(name)) {
                return this;
            }
            else if (_super == null) {
                return null;
            }
            else {
                return _super.FindBinaryOperatorContext(name);
            }
        }

        /// <summary>
        /// Removes a BinaryOperator from this context
        /// </summary>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemoveBinaryOperator(string name)
        {
            return _binaryOps.Remove(name);
        }

        /// <summary>
		/// Adds an alias for a BinaryOperator in this context
		/// </summary>
		/// <param name="name">the name of the BinaryOperator</param>
		/// <param name="alias">the alternative name for the BinaryOperator</param>
		public void AddBinaryOperatorAlias(string name, string alias)
		{
			AddBinaryOperator(alias, GetBinaryOperator(name));
		}

        /// <summary>
		/// Assigns a new name for a BinaryOperator
		/// </summary>
		/// <param name="name">the current name of the BinaryOperator</param>
		/// <param name="newname">the new name for the BinaryOperator</param>
		public void RenameBinaryOperator(string name, string newname)
		{
			ObjectContext context = FindBinaryOperatorContext(name);
			if (context == null)
				throw ThrowHelper.UndefinedObject(name);
			context._binaryOps.Add(newname,  context._binaryOps[name]);
			context._binaryOps.Remove(name);
		}

        #endregion

        #region Generated UnaryOperator.UnaryOperator methods 

        /// <summary>
        /// Sets a UnaryOperator in this context. If the UnaryOperator exists, it is set in
        /// the context in which it was declared. Otherwise, it is declared in this context.
        /// </summary>
        public void SetUnaryOperator(string name, UnaryOperator value)
        {
            ObjectContext c = FindUnaryOperatorContext(name);
            if (c != null) {
                c._unaryOps[name] = value;
            }
            else {
                AddUnaryOperator(name, value);
            }
        }

        /// <summary>
        /// Adds a UnaryOperator to this context. If the UnaryOperator exists, an exception is thrown
        /// </summary>
        public void AddUnaryOperator(string name, UnaryOperator value)
        {
			try {
				_unaryOps.Add(name, value);
			}
			catch(ArgumentException) {
				throw new DuplicateDefinitionException(name);
			}
        }

        /// <summary>
        /// Tries to get a UnaryOperator from this context
        /// </summary>
        /// <returns>true if the UnaryOperator was found, otherwise false.</returns>
        public bool TryGetUnaryOperator(string name, out UnaryOperator value)
        {
            if (_unaryOps.TryGetValue(name, out value)) {
                return true;
            }
            else if (_super == null) {
                return false;
            }
            else {
                return _super.TryGetUnaryOperator(name, out value);
            }
        }

        /// <summary>
        /// Gets a UnaryOperator if it exists, throws an ArgumentException otherwise
        /// </summary>
        /// <param name="name">the UnaryOperator as a string</param>
        /// <exception cref="ArgumentException">thrown if the UnaryOperator is undefined</exception>
        /// <returns></returns>
        public UnaryOperator GetUnaryOperator(string name)
        {
            UnaryOperator value;
            if (_unaryOps.TryGetValue(name, out value)) {
                return value;
            }
            else if (_super == null) {
                throw ThrowHelper.UndefinedObject(name);
            }
            else {
                return _super.GetUnaryOperator(name);
            }
        }

        /// <summary>
        /// Searches for the context in which a UnaryOperator is declared. If the UnaryOperator cannot be found, null is returned.
        /// </summary>
        /// <param name="name">the block name</param>
        /// <returns>the ObjectContext in which the block is declared</returns>
        public ObjectContext FindUnaryOperatorContext(string name)
        {
            if (_unaryOps.ContainsKey(name)) {
                return this;
            }
            else if (_super == null) {
                return null;
            }
            else {
                return _super.FindUnaryOperatorContext(name);
            }
        }

        /// <summary>
        /// Removes a UnaryOperator from this context
        /// </summary>
        /// <returns>true if the remove was successful, false otherwise</returns>
        public bool RemoveUnaryOperator(string name)
        {
            return _unaryOps.Remove(name);
        }

        /// <summary>
		/// Adds an alias for a UnaryOperator in this context
		/// </summary>
		/// <param name="name">the name of the UnaryOperator</param>
		/// <param name="alias">the alternative name for the UnaryOperator</param>
		public void AddUnaryOperatorAlias(string name, string alias)
		{
			AddUnaryOperator(alias, GetUnaryOperator(name));
		}

        /// <summary>
		/// Assigns a new name for a UnaryOperator
		/// </summary>
		/// <param name="name">the current name of the UnaryOperator</param>
		/// <param name="newname">the new name for the UnaryOperator</param>
		public void RenameUnaryOperator(string name, string newname)
		{
			ObjectContext context = FindUnaryOperatorContext(name);
			if (context == null)
				throw ThrowHelper.UndefinedObject(name);
			context._unaryOps.Add(newname,  context._unaryOps[name]);
			context._unaryOps.Remove(name);
		}

        #endregion

        #region Misc Generated
        /// <summary>
        /// Searches for the context in which a Variable is declared. If the Variable cannot be found, null is returned.
        /// </summary>
        /// <param name="name">the block name</param>
        /// <returns>the ObjectContext in which the block is declared</returns>
        public ObjectContext FindVariableContext(string name)
        {
            if (_variables.ContainsKey(name)) {
                return this;
            }
            else if (_super == null) {
                return null;
            }
            else {
                return _super.FindVariableContext(name);
            }
        }
        /// <summary>
        /// Searches for the context in which a Constant is declared. If the Constant cannot be found, null is returned.
        /// </summary>
        /// <param name="name">the block name</param>
        /// <returns>the ObjectContext in which the block is declared</returns>
        public ObjectContext FindConstantContext(string name)
        {
            if (_constants.ContainsKey(name)) {
                return this;
            }
            else if (_super == null) {
                return null;
            }
            else {
                return _super.FindConstantContext(name);
            }
        }

        #endregion
    }
}
