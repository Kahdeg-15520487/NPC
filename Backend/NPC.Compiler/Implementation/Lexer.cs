using System;
using System.Text;

using NPC.Compiler.Contract;
using NPC.Compiler.Datas;

namespace NPC.Compiler.Implementation
{
    class Lexer : ILexer
    {
        string text;
        int pos;
        char current_char;
        int current_line;
        int current_pos_in_line;
        string[] lines;

        public int CurrentLine { get => current_line; }
        public int CurrentPosInLine { get => current_pos_in_line; }
        public string CurrentLineSource {
            get {
                return text.Substring(pos - current_pos_in_line, current_pos_in_line);
            }
        }
        public string CurrentLineContext {
            get {
                if (CurrentLine == 0)
                {
                    return lines[current_line];
                }
                else
                {
                    return $"{lines[CurrentLine - 1]}{Environment.NewLine}{lines[current_line]}";
                }
            }
        }

        public Lexer(string t)
        {
            text = t;
            pos = 0;
            current_char = text[pos];
            current_line = 0;
            current_pos_in_line = 0;
            lines = t.Split(Environment.NewLine);
        }

        public Lexer(Lexer other)
        {
            text = other.text;
            pos = other.pos;
            current_char = other.current_char;
            current_line = other.current_line;
            current_pos_in_line = other.current_pos_in_line;
            lines = other.text.Split(Environment.NewLine);
        }

        public void Error()
        {
            throw new SyntaxErrorException(CurrentLine, CurrentPosInLine + 1, 1, CurrentLineContext, "Unexpected token at ");
        }

        public void Error(string msg)
        {
            throw new SyntaxErrorException(CurrentLine, CurrentPosInLine + 1, 1, CurrentLineContext, msg);
        }

        void Advance()
        {
            pos++;
            current_pos_in_line++;
            if (pos > text.Length - 1)
            {
                current_char = '\0';
            }
            else
            {
                current_char = text[pos];
            }
        }
        void BackTrack()
        {
            pos--;
            current_pos_in_line--;
            if (pos < 0)
            {
                current_char = '\0';
            }
            else
            {
                current_char = text[pos];
            }
        }
        char Peek()
        {
            if (pos > text.Length - 1)
            {
                current_char = '\0';
            }
            return text[pos + 1];
        }

        int SkipWhitespace()
        {
            int count = 0;
            while (current_char != '\0' && char.IsWhiteSpace(current_char))
            {
                if (current_char == '\r')
                {
                    count = 0;
                    current_pos_in_line = 0;
                    current_line++;
                }
                Advance();
                count++;
            }
            return count;
        }

        void SkipComment()
        {
            while (current_char != '\0' && current_char != '\n')
            {
                Advance();
            }
        }

        Token Interger()
        {
            StringBuilder result = new StringBuilder();
            while (current_char != '\0' && current_char.IsNumeric())
            {
                result.Append(current_char);
                Advance();
            }

            //check for dot for float number
            if (current_char == '.')
            {
                Error();
                //if (Peek().IsNumeric())
                //{
                //    result.Append(current_char);
                //    Advance();
                //    while (current_char != '\0' && current_char.IsNumeric())
                //    {
                //        result.Append(current_char);
                //        Advance();
                //    }

                //    return new Token(TokenType.FLOAT, result.ToString());
                //}
                //else
                //{
                //    //invalid integer, ex: "12."
                //    Error();
                //    return new Token(TokenType.INTERGER, result.ToString());
                //}
            }

            return new Token(TokenType.INTERGER, result.ToString());
        }

        Token String()
        {
            string result = "";
            Advance();
            while (current_char != '\0' && current_char != '"')
            {
                result += current_char;
                Advance();
            }
            if (current_char != '"')
            {
                Error();
            }
            Advance();
            return new Token(TokenType.STRING, result);
        }

        Token GuidToken()
        {
            string result = "";
            Advance();
            while (current_char != '\0' && current_char != '"')
            {
                result += current_char;
                Advance();
            }
            if (current_char != '"')
            {
                Error();
            }
            Advance();
            if (!Guid.TryParse(result, out Guid parsed))
            {
                current_pos_in_line -= result.Length + 6;
                Error($"'{result}' is not a valid GUID");
            }
            return new Token(TokenType.GUID, result.ToString());
        }

        Token DateTimeToken()
        {
            string result = "";
            Advance();
            while (current_char != '\0' && current_char != '"')
            {
                result += current_char;
                Advance();
            }
            if (current_char != '"')
            {
                Error();
            }
            Advance();
            if (!DateTime.TryParse(result, out DateTime parsed))
            {
                current_pos_in_line -= result.Length + 6;
                Error($"'{result}' is not a valid DateTime");
            }
            return new Token(TokenType.DATETIME, parsed.ToString());
        }

        Token Ident()
        {
            StringBuilder temp = new StringBuilder();
            while (current_char != '\0' && current_char.IsIdent())
            {
                temp.Append(current_char);
                Advance();
            }

            string result = temp.ToString();

            switch (result)
            {
                case "true":
                case "false":
                    return new Token(TokenType.BOOL, result);

                case "and":
                    return new Token(TokenType.AND, result);
                case "or":
                    return new Token(TokenType.OR, result);
                case "not":
                    return new Token(TokenType.NOT, result);

                case "null":
                    return new Token(TokenType.NULL, result);

                case "isempty":
                    return new Token(TokenType.ISEMPTY, result);
                case "in":
                    return new Token(TokenType.IN, result);
                case "contains":
                    return new Token(TokenType.CONTAINS, result);

                case "if":
                    return new Token(TokenType.IF, result);
                case "else":
                    return new Token(TokenType.ELSE, result);
                case "elif":
                    return new Token(TokenType.ELIF, result);
                case "match":
                    return new Token(TokenType.MATCH, result);
                case "return":
                    return new Token(TokenType.RETURN, result);
                case "typeof":
                    return new Token(TokenType.TYPEOF, result);
                case "is":
                    return new Token(TokenType.IS, result);

                case "INT":
                case "int":
                case "GUID":
                case "guid":
                case "DATETIME":
                case "datetime":
                case "STR":
                case "str":
                case "BOOL":
                case "bool":
                case "NULL":
                    return new Token(TokenType.TYPE, result);

            }

            return new Token(TokenType.IDENT, result);
        }

        public Token GetNextToken()
        {
            while (current_char != '\0')
            {
                if (char.IsWhiteSpace(current_char))
                {
                    current_pos_in_line += SkipWhitespace();
                    continue;
                }

                if (current_char == '/' && Peek() == '/')
                {
                    SkipComment();
                    current_line++;
                    continue;
                }

                if (current_char == '=')
                {
                    Advance();
                    if (current_char == '=')
                    {
                        Advance();
                        return new Token(TokenType.EQUAL, "==");
                    }
                    return new Token(TokenType.ASSIGN, "=");
                }

                if (current_char == '!' && Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.NOTEQUAL, "!=");
                }

                if (current_char == '>')
                {
                    Advance();
                    if (current_char == '=')
                    {
                        Advance();
                        return new Token(TokenType.LARGEREQUAL, ">=");
                    }
                    return new Token(TokenType.LARGER, ">");
                }

                if (current_char == '<')
                {
                    Advance();
                    if (current_char == '=')
                    {
                        Advance();
                        return new Token(TokenType.LESSEREQUAL, "<=");
                    }
                    return new Token(TokenType.LESSER, "<");
                }

                if (current_char == '(')
                {
                    Advance();
                    return new Token(TokenType.LPAREN, "(");
                }

                if (current_char == ')')
                {
                    Advance();
                    return new Token(TokenType.RPAREN, ")");
                }

                if (current_char == '{')
                {
                    Advance();
                    return new Token(TokenType.LBRACE, "{");
                }

                if (current_char == '}')
                {
                    Advance();
                    return new Token(TokenType.RBRACE, "}");
                }

                if (current_char == '[')
                {
                    Advance();
                    return new Token(TokenType.LBRACKET, "[");
                }

                if (current_char == ']')
                {
                    Advance();
                    return new Token(TokenType.RBRACKET, "]");
                }

                if (current_char == ',')
                {
                    Advance();
                    return new Token(TokenType.COMMA, ",");
                }

                if (current_char == ';')
                {
                    Advance();
                    return new Token(TokenType.SEMICOLON, ";");
                }

                if (current_char == ':')
                {
                    Advance();
                    return new Token(TokenType.COLON, ":");
                }

                if (current_char == '_')
                {
                    Advance();
                    return new Token(TokenType.UNDERSCORE, "_");
                }

                if (current_char == 'g')
                {
                    Advance();
                    if (current_char == '"')
                    {
                        return GuidToken();
                    }
                    else
                    {
                        BackTrack();
                    }
                }

                if (current_char == 'd')
                {
                    Advance();
                    if (current_char == '"')
                    {
                        return DateTimeToken();
                    }
                    else
                    {
                        BackTrack();
                    }
                }

                if (current_char == '"')
                {
                    return String();
                }

                if (current_char.IsNumeric())
                {
                    return Interger();
                }

                if (current_char.IsIdent())
                {
                    return Ident();
                }

                Error();
            }
            return new Token(TokenType.EOF, null);
        }

        public Token PeekNextToken()
        {
            Lexer peeker = new Lexer(this);
            return peeker.GetNextToken();
        }
    }
}
