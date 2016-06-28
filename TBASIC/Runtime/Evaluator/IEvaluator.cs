/**
 * TBASIC
 * Copyright (c) 2013-2016 Timothy Baxendale
 *
 * This project is licensed under the Simplified BSD License for
 * non-commercial use.
 *
 **/

using Tbasic.Components;

namespace Tbasic.Runtime
{
    /// <summary>
    /// An expression evaluator
    /// </summary>
    internal interface IEvaluator
    {
        StringSegment Expression { get; set; }
        object Evaluate();
        Executer CurrentExecution { get; set; }
    }
}
