// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;

namespace Tbasic.Libraries
{
    /// <summary>
    /// Indicates that this is a tbasic function
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class TBasicInstructionAttribute : Attribute
    {
        /// <summary>
        /// Gets wether this should be called as a statement or a function
        /// </summary>
        public TBasicCallType CallType { get; private set; }

        /// <summary>
        /// Gets wether this should be called as a statement or a function
        /// </summary>
        public Type DelegateType { get; private set; }

        /// <summary>
        /// Gets or sets the number of required parameters. If this method has more parameters, they will be default initialized.
        /// </summary>
        public int RequiredParameters { get; set; }
        
        /// <summary>
        /// Gets the name of this function in tbasic
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Constructs this attribute
        /// </summary>
        /// <param name="callType">the type of instruction this is (statement or function)</param>
        /// <param name="delegateType">the type of delegate</param>
        /// <param name="name">the name of this function in tbasic</param>
        public TBasicInstructionAttribute(string name, Type delegateType, TBasicCallType callType)
        {
            Name = name;
            DelegateType = delegateType;
            CallType = callType;
        }
    }

    /// <summary>
    /// Determines how a function should be called
    /// </summary>
    public enum TBasicCallType
    {
        /// <summary>
        /// Call this as a normal function
        /// </summary>
        Function,
        /// <summary>
        /// Call this as a statement. Arguments should not be parsed or evaluated automatically.
        /// </summary>
        Statement
    }
}
