// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Runtime;

namespace Tbasic.Libraries
{
    /// <summary>
    /// A library containing several mathmatical functions
    /// </summary>
    public class MathLibrary : Library
    {
        private static Random rand = new Random();

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public MathLibrary(ObjectContext context)
        {
            Add("POW", Pow);
            Add("IPART", iPart);
            Add("FPART", fPart);
            Add("ROUND", Round);
            Add("EVAL", Eval);
            Add("RANDOM", Random);
            Add("ABS", Abs);
            Add("SIN", Sin);
            Add("ASIN", Asin);
            Add("SINH", Sinh);
            Add("COS", Cos);
            Add("ACOS", Acos);
            Add("COSH", Cosh);
            Add("TAN", Tan);
            Add("ATAN", Atan);
            Add("LOG", Log);
            Add("LN", Ln);
            context.SetConstant("@PI", Math.PI); // pi
            context.SetConstant("@E", Math.E); // euler's number
        }

        private void Log(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(atLeast: 2, atMost: 3);
            if (stackFrame.ParameterCount == 2) {
                stackFrame.Data = Math.Log10(stackFrame.GetParameter<Number>(1));
            }
            else {
                stackFrame.Data = Math.Log(stackFrame.GetParameter<Number>(1), stackFrame.GetParameter<Number>(2));
            }
        }

        private void Ln(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Log(stackFrame.GetParameter<Number>(1));
        }

        private void Abs(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Abs(stackFrame.GetParameter<Number>(1));
        }

        private void Sin(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Sin(stackFrame.GetParameter<Number>(1));
        }

        private void Asin(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Asin(stackFrame.GetParameter<Number>(1));
        }

        private void Sinh(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Sinh(stackFrame.GetParameter<Number>(1));
        }

        private void Cos(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Cos(stackFrame.GetParameter<Number>(1));
        }

        private void Acos(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Acos(stackFrame.GetParameter<Number>(1));
        }

        private void Cosh(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Cosh(stackFrame.GetParameter<Number>(1));
        }

        private void Tan(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Tan(stackFrame.GetParameter<Number>(1));
        }

        private void Atan(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Atan(stackFrame.GetParameter<Number>(1));
        }

        private void Tanh(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = Math.Tanh(stackFrame.GetParameter<Number>(1));
        }

        /// <summary>
        /// Returns a pseudo-random double between 0 and 1
        /// </summary>
        /// <returns>a pseudo-random double between 0 and 1</returns>
        public static double Random()
        {
            return rand.NextDouble();
        }

        /// <summary>
        /// Returns a pseudo-random double between 0 and a max value
        /// </summary>
        /// <param name="max">the exclusive upper bound</param>
        /// <returns></returns>
        public static double Random(int max)
        {
            return Random() * max;
        }

        /// <summary>
        /// Returns a pseudo-random double between a specified upper and lower bound
        /// </summary>
        /// <param name="lowerBound">the inclusive lower bound</param>
        /// <param name="upperBound">the exclusive upper bound</param>
        /// <returns></returns>
        public static double Random(int lowerBound, int upperBound)
        {
            return Random(upperBound - lowerBound) + lowerBound;
        }

        private void Random(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(atLeast: 1, atMost: 3);
            if (stackFrame.ParameterCount == 1) {
                stackFrame.Data = Random();
            }
            else if (stackFrame.ParameterCount == 2) {
                stackFrame.Data = Random(stackFrame.GetParameter<int>(1));
            }
            else {
                stackFrame.Data = Random(stackFrame.GetParameter<int>(1), stackFrame.GetParameter<int>(2));
            }
        }

        /// <summary>
        /// Rounds a double value to a given number of places
        /// </summary>
        /// <param name="number">the number to round</param>
        /// <param name="places">the number of places</param>
        /// <returns>the rounded double</returns>
        public static double Round(double number, int places)
        {
            return Math.Round(number, places);
        }

        private void Round(TFunctionData stackFrame)
        {
            if (stackFrame.ParameterCount == 2) {
                stackFrame.AddParameter(2);
            }
            stackFrame.AssertParamCount(3);
            stackFrame.Data = Round(stackFrame.GetParameter<Number>(1), stackFrame.GetParameter<Number>(2));
        }

        /// <summary>
        /// Returns the integer part of a double value
        /// </summary>
        /// <param name="d">the double to truncate</param>
        /// <returns>the truncated double</returns>
        public static int iPart(double d)
        {
            return (int)d;
        }

        private void iPart(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = iPart(stackFrame.GetParameter<Number>(1));
        }

        /// <summary>
        /// Returns the fractional part of a double value
        /// </summary>
        /// <param name="d">the double to truncate</param>
        /// <returns>the truncated double</returns>
        public static double fPart(double d)
        {
            return d - (int)d;
        }

        private void fPart(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            stackFrame.Data = fPart(stackFrame.GetParameter<Number>(1));
        }

        /// <summary>
        /// Evaluates a mathmatical expression
        /// </summary>
        /// <param name="expr">the expression to evaluate</param>
        /// <returns>the evaluated expression</returns>
        public static object Eval(string expr)
        {
            Executer e = new Executer(); // local execution
            e.Global.AddLibrary(new MathLibrary(e.Global)); // only allow math libs
            e.Global.SetFunction("eval", null); // that's a no-no
            return Evaluator.Evaluate(new StringSegment(expr), e);
        }

        private void Eval(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(2);
            try {
                stackFrame.Data = Eval(stackFrame.GetParameter<string>(1));
            }
            catch(Exception ex) {
                throw TbasicException.WrapException(ex);
            }
        }

        private static void Pow(TFunctionData stackFrame)
        {
            stackFrame.AssertParamCount(3);
            stackFrame.Data = Pow(stackFrame.GetParameter<Number>(1), stackFrame.GetParameter<Number>(2));
        }

        /// <summary>
        /// Raises a number to a given exponent
        /// </summary>
        /// <param name="dBase">the base</param>
        /// <param name="dPower">the exponent</param>
        /// <returns>the evaluated base raised to the given exponent</returns>
        public static double Pow(double dBase, double dPower)
        {
            return Math.Pow(dBase, dPower);
        }
    }
}