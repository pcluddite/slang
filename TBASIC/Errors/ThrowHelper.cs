// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;

namespace Tbasic.Errors
{
    internal class ThrowHelper
    {
        public static Exception UndefinedObject(string name)
        {
            return new KeyNotFoundException("'" + name + "' does not exist in the current context");
        }

        public static Exception UndefinedFunctionOrCommand(string name)
        {
            return new NotImplementedException("'" + name + "' is not defined as a command or function");
        }

        public static Exception UndefinedFunction(string name)
        {
            return new NotImplementedException("'" + name + "' is not defined as a function");
        }

        public static Exception UnterminatedGroup()
        {
            return new FormatException("Unterminated group");
        }

        public static Exception UnterminatedString()
        {
            return new FormatException("Unterminated string");
        }

        public static Exception UnterminatedEscapeSequence()
        {
            return new FormatException("Unterminated escape sequence");
        }

        public static Exception UnknownEscapeSequence(char escape)
        {
            return new FormatException("Unknown escape sequence \\" + escape);
        }

        public static Exception UnterminatedUnicodeEscape()
        {
            return new FormatException("Unterminated escape sequence. Expected four digit hex to follow '\\u'.");
        }

        public static Exception AlreadyDefinedAsType(string name, string type, string newType)
        {
            return new InvalidCastException(string.Format("An object '{0}' has been defined as a {1} and cannot be redefined as a {2}", name, type, newType));
        }

        public static Exception ConstantChange()
        {
            return new InvalidOperationException("Cannot redefine a constant");
        }

        public static Exception ContextCleared()
        {
            return new InvalidOperationException("Context fell out of scope and was disposed");
        }

        public static Exception InvalidOperator(char opr)
        {
            return new InvalidOperatorException(opr.ToString());
        }

        public static Exception InvalidVariableName(string name)
        {
            return new ScriptParsingException(string.Format("The variable name '{0}' contains invalid characters", name));
        }

        public static Exception InvalidVariableName()
        {
            return new ScriptParsingException("The variable name contains invalid characters");
        }

        public static Exception ArraysCannotBeConstant()
        {
            return new ScriptParsingException("Arrays cannot be defined as constants");
        }

        public static Exception ExpectedToken(string token)
        {
            return new ExpectedTokenExceptiopn(token);
        }

        public static Exception NoOpeningStatement(string str)
        {
            return new ScriptParsingException(string.Format("Cannot find opening statement for '{0}'", str));
        }

        public static Exception NoCondition()
        {
            return new ScriptParsingException("Expected condition");
        }

        public static Exception UnterminatedBlock(string name)
        {
            return new EndOfCodeException("Unterminated '" + name + "' block");
        }

        public static Exception InvalidTypeInExpression(string expr, string expected)
        {
            return new ScriptParsingException(string.Format("Invalid type in expression '{0}', expected '{1}'", expr, expected));
        }

        public static Exception NoIndexSpecified()
        {
            return new FormatException("At least one index was expected between braces");
        }

        public static Exception IndexUnavailable(string _sName)
        {
            return new InvalidOperationException(string.Format("Object '{0}' cannot be indexed", _sName));
        }

        public static Exception IndexOutOfRange(string _sName, int index)
        {
            return new InvalidOperationException(string.Format("Index '{0}' of object '{1}' is out of range", index, _sName));
        }

        public static Exception InvalidExpression(string expr)
        {
            return new ScriptParsingException("Invalid expression [" + expr + "]");
        }

        public static Exception ExpectedSpaceAfterCommand()
        {
            return new FormatException("Expected space after command name");
        }

        public static Exception OperatorUndefined(string opr)
        {
            return new ArgumentException("Operator [" + opr + "] is undefined");
        }

        public static Exception MacroRedefined()
        {
            return new ArgumentException("Cannot redefine a macro");
        }

        public static Exception InvalidDefinitionOperator()
        {
            return new ArgumentException("Expected [=] in definition");
        }

        public static Exception MissingBinaryOp(object left, object right)
        {
            return new ArgumentException(
                string.Format("Missing binary operator - {0} [?] {1}",
                    left is string ? "\"" + left + "\"" : left,
                    right is string ? "\"" + right + "\"" : right
                )
            );
        }

        public static Exception InvalidParamType(int index, string rightType)
        {
            return new InvalidCastException(string.Format("Expected parameter {0} to be of type {1}", index, rightType));
        }
    }
}
