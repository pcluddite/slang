// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;

namespace Tbasic.Runtime
{
    /// <summary>
    /// An expression evaluator
    /// </summary>
    internal interface IExpressionEvaluator
    {
        IEnumerable<char> Expression { get; set; }
        object Evaluate();
        TRuntime Runtime { get; set; }
        ObjectContext CurrentContext { get; set; }
    }
}
