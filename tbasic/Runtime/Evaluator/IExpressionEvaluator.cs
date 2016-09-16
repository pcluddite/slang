// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System.Collections.Generic;
using Tbasic.Types;

namespace Tbasic.Runtime
{
    /// <summary>
    /// An expression evaluator
    /// </summary>
    internal interface IExpressionEvaluator : IRuntimeObject
    {
        IEnumerable<char> Expression { get; set; }
        IRuntimeObject Evaluate();
        TRuntime Runtime { get; set; }
        ObjectContext CurrentContext { get; set; }
    }
}
