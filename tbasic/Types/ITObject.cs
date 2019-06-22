/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System;

namespace Slang.Types
{
    /// <summary>
    /// Interface for objects
    /// </summary>
    public interface ITObject : IConvertible
    {
        /// <summary>
        /// Gets the native .NET type this is a wrapper for. Returns null if it is not a .NET type
        /// </summary>
        Type Native { get; }
    }
}
