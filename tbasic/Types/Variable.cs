// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Tbasic.Runtime;

namespace Tbasic.Types
{
    /// <summary>
    /// Reperesents a variable
    /// </summary>
    public struct Variable
    {
        /// <summary>
        /// The context where this variable is located
        /// </summary>
        public ObjectContext Context { get; }
        /// <summary>
        /// Gets the index of this variable if it is an array
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// The value of this variable
        /// </summary>
        public object Value { get; }
        /// <summary>
        /// Gets the name of this variable
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Initializes a new variable
        /// </summary>
        public Variable(ObjectContext context, string name, object value)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            Contract.EndContractBlock();
            
            Context = context;
            Name = name;
            Value = value;
            Index = -1;
        }

        /// <summary>
        /// Initializes a new variable
        /// </summary>
        public Variable(ObjectContext context, string name, object value, int index)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            Contract.EndContractBlock();

            Context = context;
            Name = name;
            Value = value;
            Index = index;
        }
    }
}
