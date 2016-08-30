﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======

// Autogenerated code

using System;
using System.Collections.Generic;
using Tbasic.Operators;
using Tbasic.Errors;

namespace Tbasic.Runtime
{
    public partial class ObjectContext
    {
		#region Generated _functions methods 

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
        /// Tries to get a Function from this context
        /// </summary>
        /// <returns>true if the Function was found, otherwise false.</returns>
        public bool TryGetFunction(string name, out TBasicFunction value)
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
        public TBasicFunction GetFunction(string name)
        {
            TBasicFunction value;
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
            _functions.Add(name, value);
        }

        /// <summary>
        /// Lists all the Functions currently defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, TBasicFunction>> GetLocalFunctions()
        {
            return _functions;
        }

        /// <summary>
        /// Lists all the Functions currently defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, TBasicFunction>> GetAllFunctions()
        {
            ObjectContext context = this;
            while (context != null) {
                foreach (var value in context.GetLocalFunctions()) {
                    yield return value;
                }
                context = context._super;
            }
        }

		#endregion

		#region Generated _commands methods 

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
        /// Tries to get a Command from this context
        /// </summary>
        /// <returns>true if the Command was found, otherwise false.</returns>
        public bool TryGetCommand(string name, out TBasicFunction value)
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
        public TBasicFunction GetCommand(string name)
        {
            TBasicFunction value;
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
            _commands.Add(name, value);
        }

        /// <summary>
        /// Lists all the Commands currently defined in this context
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, TBasicFunction>> GetLocalCommands()
        {
            return _commands;
        }

        /// <summary>
        /// Lists all the Commands currently defined
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, TBasicFunction>> GetAllCommands()
        {
            ObjectContext context = this;
            while (context != null) {
                foreach (var value in context.GetLocalCommands()) {
                    yield return value;
                }
                context = context._super;
            }
        }

		#endregion

		#region Generated _blocks methods 

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
            _blocks.Add(name, value);
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

		#endregion

		#region Generated _binaryOps methods 

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
            _binaryOps.Add(name, value);
        }

		#endregion

		#region Generated _unaryOps methods 

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
            _unaryOps.Add(name, value);
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
