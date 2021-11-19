using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPC.Compiler;
using NPC.Compiler.Datas;

namespace NPC
{
    public static class ExtensionMethod
    {
        public static bool IsIdent(this char c)
        {
            // Return true if the character is between 0 or 9 inclusive or is an uppercase or
            // lowercase letter or underscore

            return ((c >= '0' && c <= '9') ||
                    (c >= 'A' && c <= 'Z') ||
                    (c >= 'a' && c <= 'z') ||
                     c == '_' ||
                     c == '$' ||
                     c == ':');
        }

        public static bool IsNumeric(this char c)
        {
            return (c >= '0' && c <= '9');
        }

        public static bool IsHexNumeric(this char c)
        {
            return IsNumeric(c) || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
        }

        public static bool IsWhiteSpace(this char c)
        {
            return c == '\t' || c == ' ';
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static ValType ToValType(this string lexeme)
        {
            switch (lexeme)
            {
                case "INT":
                case "int":
                    return ValType.Int;
                case "STR":
                case "str":
                    return ValType.String;
                case "GUID":
                case "guid":
                    return ValType.Guid;
                case "BOOL":
                case "bool":
                    return ValType.Bool;
                case "DATETIME":
                case "datetime":
                    return ValType.DateTime;
                default:
                    return ValType.Invalid;
            }
        }

        public static ValType ToValType(this TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.INTERGER:
                    return ValType.Int;
                case TokenType.STRING:
                    return ValType.String;
                case TokenType.GUID:
                    return ValType.Guid;
                case TokenType.BOOL:
                    return ValType.Bool;
                case TokenType.DATETIME:
                    return ValType.DateTime;
                default:
                    return ValType.Invalid;
            }
        }

        public static Conjunction ToConjunction(this TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.AND:
                    return Conjunction.And;
                case TokenType.OR:
                    return Conjunction.Or;
                default:
                    return Conjunction.None;
            }
        }

        public static Operator ToOperator(this TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.EQUAL:
                    return Operator.Equal;
                case TokenType.LARGER:
                    return Operator.Greater;
                case TokenType.LESSER:
                    return Operator.Less;
                case TokenType.LARGEREQUAL:
                    return Operator.GreaterOrEqual;
                case TokenType.LESSEREQUAL:
                    return Operator.LessOrEqual;
                case TokenType.CONTAINS:
                    return Operator.Contain;
                case TokenType.IN:
                    return Operator.In;
                case TokenType.ISEMPTY:
                    return Operator.IsEmpty;

                default:
                    return Operator.Invalid;
            }
        }
    }
}
