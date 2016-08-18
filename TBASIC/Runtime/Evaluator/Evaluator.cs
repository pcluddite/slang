﻿// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Tbasic.Parsing;
using Tbasic.Components;
using Tbasic.Errors;
using Tbasic.Operators;

namespace Tbasic.Runtime
{
    /// <summary>
    /// General purpose evaluator for functions, variables, math, booleans, etc.
    /// </summary>
    internal partial class Evaluator : IEvaluator
    {
        private LinkedList<object> _tokens = new LinkedList<object>();
        private StringSegment _expression = StringSegment.Empty;
        private bool _parsed;
        
        public Evaluator(Executer exec)
        {
            CurrentExecution = exec;
        }
        
        public Evaluator(StringSegment expression, Executer exec)
        {
            CurrentExecution = exec;
            Expression = expression;
        }

        #region Properties
        
        public StringSegment Expression
        {
            get { return _expression; }
            set {
                _expression = value.Trim();
                _parsed = false;
                _tokens.Clear();
            }
        }

        public ObjectContext CurrentContext
        {
            get {
                return CurrentExecution.Context;
            }
        }

        public Executer CurrentExecution { get; set; }

        public bool ShouldParse
        {
            get {
                return !_parsed;
            }
            set {
                if (_parsed && value) {
                    _tokens.Clear();
                    _parsed = !value;
                }
            }
        }

        #endregion
        
        public object Evaluate()
        {
            if (StringSegment.IsNullOrEmpty(Expression)) 
                return 0;
            
            if (!_parsed) {
                Scanner scanner = new DefaultScanner(_expression);
                while (!scanner.EndOfStream)
                    NextToken(scanner);
                _parsed = true;
            }

            return ConvertToSimpleType(EvaluateList());
        }

        public bool EvaluateBool()
        {
            return Convert.ToBoolean(Evaluate());
        }
        
        private int NextToken(Scanner scanner)
        {
            int startIndex = scanner.IntPosition;

            // check group
            if (scanner.Next("(")) {
                return AddObjectToExprList("(", startIndex, scanner);
            }

            // check string
            string str_parsed;
            if (scanner.NextString(out str_parsed)) {
                return AddObjectToExprList(str_parsed, startIndex, scanner);
            }

            // check unary op
            UnaryOperator unaryOp;
            if (scanner.NextUnaryOp(CurrentContext, _tokens.Last?.Value, out unaryOp)) {
                return AddObjectToExprList(unaryOp, startIndex, scanner);
            }

            // check function
            Function func;
            if (scanner.NextFunction(CurrentExecution, out func)) {
                return AddObjectToExprList(func, startIndex, scanner);
            }

            // check null
            if (scanner.Next("null")) {
                return AddObjectToExprList("null", startIndex, scanner);
            }

            // check variable
            Variable variable;
            if (scanner.NextVariable(CurrentExecution, out variable)) {
                return AddObjectToExprList(variable, startIndex, scanner);
            }

            // check hexadecimal
            long hex;
            if (scanner.NextHexadecimal(out hex)) {
                return AddObjectToExprList(hex, startIndex, scanner);
            }

            // check boolean
            bool b;
            if (scanner.NextBool(out b)) {
                return AddObjectToExprList(b, startIndex, scanner);
            }

            // check numeric
            Number num;
            if (scanner.NextUnsignedNumber(out num)) {
                return AddObjectToExprList(num, startIndex, scanner);
            }

            // check binary operator
            BinaryOperator binOp;
            if (scanner.NextBinaryOp(CurrentContext, out binOp)) {
                return AddObjectToExprList(binOp, startIndex, scanner);
            }

            // couldn't be parsed

            if (CurrentContext.FindFunctionContext(_expression.ToString()) == null) {
                throw new ArgumentException("Invalid expression '" + _expression + "'");
            }
            else {
                throw new FormatException("Poorly formed function call");
            }
        }

        private int AddObjectToExprList(object val, int startIndex, Scanner scanner)
        {
            if (Equals(val, "(")) {

                scanner.IntPosition = GroupParser.IndexGroup(_expression, startIndex) + 1;

                Evaluator eval = new Evaluator(
                    _expression.Subsegment(startIndex + 1, scanner.IntPosition - startIndex - 2),
                    CurrentExecution // share the wealth
                );
                _tokens.AddLast(eval);
            }
            else {
                _tokens.AddLast(val);
            }

            return scanner.IntPosition;
        }
        
        private object EvaluateList()
        {
            LinkedList<object> list = new LinkedList<object>(_tokens);

            // evaluate unary operators

            LinkedListNode<object> x = list.First;
            while (x != null) {
                UnaryOperator? op = x.Value as UnaryOperator?;
                if (op != null) {
                    x.Value = PerformUnaryOp(op.Value, x.Previous?.Value, x.Next.Value);
                    list.Remove(x.Next);
                }
                x = x.Next;
            }

            // queue and evaluate binary operators
            BinaryOpQueue opqueue = new BinaryOpQueue(list);
            
            x = list.First.Next; // skip the first operand
            while (x != null) {
                BinaryOperator? op = x.Value as BinaryOperator?;
                if (op == null) {
                    throw ThrowHelper.MissingBinaryOp(x.Previous.Value, x.Value);
                }
                else {
                    if (x.Next == null)
                        throw new ArgumentException("Expression cannot end in a binary operation [" + x.Value + "]");
                }
                x = x.Next.Next; // skip the operand
            }

            BinOpNodePair nodePair;
            while (opqueue.Dequeue(out nodePair)) {
                nodePair.Node.Previous.Value = PerformBinaryOp(
                    nodePair.Operator,
                    nodePair.Node.Previous.Value,
                    nodePair.Node.Next.Value
                    );
                list.Remove(nodePair.Node.Next);
                list.Remove(nodePair.Node);
            }

            IEvaluator expr = list.First.Value as IEvaluator;
            if (expr == null) {
                return list.First.Value;
            }
            else {
                return expr.Evaluate();
            }
        }
        
        #region static methods

        /// <summary>
        /// Static version of the Expression Evaluator
        /// </summary>
        /// <param name="expressionString">expression to be evaluated</param>
        /// <param name="exec">the current execution</param>
        /// <returns></returns>
        public static object Evaluate(StringSegment expressionString, Executer exec)
        {
            Evaluator expression = new Evaluator(expressionString, exec);
            return expression.Evaluate();
        }

        /// <summary>
        /// Tries to convert an object to a given type. First this tries to do a straight up cast. If that doesn't work and strict is turned off, it will try to be converted with IConvertible. If the object is a string and parseStrings is turned on, it will try to parse that string.
        /// </summary>
        /// <typeparam name="T">the type to convert to</typeparam>
        /// <param name="obj">the object to convert</param>
        /// <param name="result">the result of the conversion</param>
        /// <param name="strict">wether the type should attempt to be converted</param>
        /// <param name="parseStrings">determines whether strings should try to be converted</param>
        /// <returns></returns>
        internal static bool TryConvert<T>(object obj, out T result, bool strict = false, bool parseStrings = true)
        {
            try {
                result = (T)obj;
                return true;
            }
            catch (InvalidCastException) {
                if (!strict)
                    return TryConvertNonStrict(obj, out result, parseStrings);
                result = default(T);
                return false;
            }
        }

        internal static bool TryConvertNonStrict<T>(object obj, out T result, bool parseStrings = true)
        {
            result = default(T);
            if (parseStrings) {
                string str = obj as string; // maybe we can convert it from a string?
                if (str != null) {
                    obj = ConvertFromString(str);
                    if (obj == null)
                        return false;
                    return TryConvert(obj, out result); // it's a good old fashion type now. try again.
                }
            }
            IConvertible convertible = obj as IConvertible;
            if (convertible != null) {
                try {
                    result = (T)convertible.ToType(typeof(T), CultureInfo.CurrentCulture);
                    return true;
                }
                catch (Exception ex) when (ex is FormatException || ex is InvalidCastException || ex is OverflowException) {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        internal static object ConvertFromString(string str)
        {
            // now we've just got to parse the supported types until we find a match...
            Number n;
            if (Number.TryParse(str, out n))
                return n;
            bool b;
            if (bool.TryParse(str, out b))
                return b;
            return null;
        }

        public static object PerformUnaryOp(UnaryOperator op, object left, object right)
        {
            object operand = op.Side == UnaryOperator.OperandSide.Left ? left : right;
            IEvaluator tempv = operand as IEvaluator;
            if (tempv != null)
                operand = tempv.Evaluate();

            try {
                return op.ExecuteOperator(operand);
            }
            catch(InvalidCastException) when (operand is IOperator) {
                throw new ArgumentException("Unary operand cannot be " + operand.GetType().Name);
            }
            catch (Exception ex) when (ex is InvalidCastException || ex is FormatException || ex is ArgumentException || ex is OverflowException) {
                throw new ArgumentException("Unary operator '" + op.OperatorString + "' not defined.");
            }
        }

        /// <summary>
        /// This routine will actually execute an operation and return its value
        /// </summary>
        /// <param name="op">Operator Information</param>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <returns>v1 (op) v2</returns>
        public static object PerformBinaryOp(BinaryOperator op, object left, object right)
        {
            IEvaluator tv = left as IEvaluator;
            if (tv != null) 
                left = tv.Evaluate();

            try {
                switch (op.OperatorString) { // short circuit evaluation 1/6/16
                    case "AND":
                        if (Convert.ToBoolean(left, CultureInfo.CurrentCulture)) {
                            tv = right as IEvaluator;
                            if (tv != null) {
                                right = tv.Evaluate();
                            }
                            if (Convert.ToBoolean(right, CultureInfo.CurrentCulture)) {
                                return true;
                            }
                        }
                        return false;
                    case "OR":
                        if (Convert.ToBoolean(left, CultureInfo.CurrentCulture)) {
                            return true;
                        }
                        else {
                            tv = right as IEvaluator;
                            if (tv != null) {
                                right = tv.Evaluate();
                            }
                            if (Convert.ToBoolean(right, CultureInfo.CurrentCulture)) {
                                return true;
                            }
                        }
                        return false;
                }

                tv = right as IEvaluator;
                if (tv != null)
                    right = tv.Evaluate();
                
                return op.ExecuteOperator(left, right);
            }
            catch (Exception ex) when (ex is InvalidCastException || ex is FormatException || ex is ArgumentException || ex is OverflowException) {
                throw new FormatException(string.Format(
                        "Operator [{0}] cannot be applied to objects of type '{1}' and '{2}'",
                        op.OperatorString, GetTypeName(right), GetTypeName(left)
                    ));
            }
        }

        public static string GetTypeName(object value)
        {
            Type t = value.GetType();
            if (t.IsArray) {
                return "array";
            }
            else {
                return t.Name.ToLower();
            }
        }

        public static string GetStringRepresentation(object obj, bool escapeStrings = false)
        {
            if (obj == null)
                return "";
            
            string str_obj = obj as string;
            if (str_obj != null) {
                if (escapeStrings) {
                    return ToEscapedString(str_obj);
                }
                else {
                    return str_obj;
                }
            }
            else if (obj.GetType().IsArray) {
                StringBuilder sb = new StringBuilder("{ ");
                object[] _aObj = (object[])obj;
                if (_aObj.Length > 0) {
                    for (int i = 0; i < _aObj.Length - 1; i++) {
                        sb.AppendFormat("{0}, ", GetStringRepresentation(_aObj[i], escapeStrings: true));
                    }
                    sb.AppendFormat("{0} ", GetStringRepresentation(_aObj[_aObj.Length - 1], escapeStrings: true));
                }
                sb.Append("}");
                return sb.ToString();
            }
            return obj.ToString();
        }

        private static string ToEscapedString(string str)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('\"');
            for (int index = 0; index < str.Length; index++) {
                char c = str[index];
                switch (c) {
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\"': sb.Append("\\\""); break;
                    case '\'': sb.Append("\\'"); break;
                    default:
                        if (c < ' ') {
                            sb.Append("\\u");
                            sb.Append(Convert.ToString(c, 16).PadLeft(4, '0'));
                        }
                        else {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append('\"');
            return sb.ToString();
        }

        public static object ConvertToSimpleType(object _oObj)
        {
            if (_oObj == null) {
                return 0;
            }

            if (_oObj is bool)
                return _oObj;

            Number? _nObj = Number.AsNumber(_oObj);
            if (_nObj != null) {
                return _nObj.Value;
            }
            
            IntPtr? _pObj = _oObj as IntPtr?;
            if (_pObj != null) {
                return ConvertToSimpleType(_pObj.Value);
            }

            return _oObj;
        }

        public static object ConvertToSimpleType(IntPtr ptr)
        {
            if (IntPtr.Size == sizeof(long)) { // 64-bit
                return ptr.ToInt64();
            }
            else { // 32-bit
                return ptr.ToInt32();
            }
        }

        #endregion

        public override string ToString()
        {
            return Expression.ToString();
        }
    }
}
