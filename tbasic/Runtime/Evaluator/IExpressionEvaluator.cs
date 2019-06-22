/** +++====+++
 *  
 *  Copyright (c) Timothy Baxendale
 *
 *  +++====+++
**/
using System.Collections.Generic;
using Tbasic.Types;

namespace Tbasic.Runtime
{
    /// <summary>
    /// An expression evaluator
    /// </summary>
    internal interface IExpressionEvaluator
    {
        IEnumerable<char> Expression { get; set; }
        object Evaluate();
        Executor Runtime { get; set; }
        Scope CurrentContext { get; set; }
    }
}
