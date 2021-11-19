using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using NPC.Compiler.AST;
using NPC.Compiler.Contract;
using NPC.Compiler.Datas;

namespace NPC.Compiler.Implementation
{
    class Parser : IParser
    {
        readonly ILexer lexer;

        Token currentToken;
        Token nextToken => lexer.PeekNextToken();
        Token prevToken;

        public Parser(ILexer l)
        {
            lexer = l;
            prevToken = null;
            currentToken = lexer.GetNextToken();
        }

        void Error(string msg, int posInLineAdjust = 1)
        {
            var start = (lexer.CurrentPosInLine + posInLineAdjust - prevToken?.lexeme?.Length).Value;
            var length = lexer.CurrentPosInLine - start;
            throw new SyntaxErrorException(lexer.CurrentLine, start, length, lexer.CurrentLineContext, msg);
        }

        void Error()
        {
            Error($"invalid Token: {currentToken}");
        }

        void Error(TokenType expecting, int posInLineAdjust = 1)
        {
            Error($"expecting: { expecting}", posInLineAdjust);
        }

        void Error(params TokenType[] expecting)
        {
            Error($"expecting: {string.Join(", ", expecting)}");
        }

        void Eat(TokenType t)
        {
            if (t == TokenType.ANY || currentToken.type == t)
            {
                prevToken = currentToken;
                currentToken = lexer.GetNextToken();
            }
            else
            {
                Error(t);
            }
        }

        void Eat(params TokenType[] t)
        {
            if (t.Contains(currentToken.type))
            {
                prevToken = currentToken;
                currentToken = lexer.GetNextToken();
            }
            else
            {
                Error(t);
            }
        }

        Operand Array()
        {
            Eat(TokenType.LBRACKET);
            if (currentToken.type == TokenType.RBRACKET)
            {
                Eat(TokenType.RBRACKET);
                return new Operand(new Token[0]);
            }
            List<Token> tokens = new List<Token>();
            ValType type = ValType.Invalid;
            do
            {
                ValType currType = currentToken.type.ToValType();
                if (type != ValType.Invalid)
                {
                    if (type != currType)
                    {
                        Error($"value type {currType} is not compatible with {type}");
                    }
                }
                else
                {
                    type = currType;
                }
                tokens.Add(currentToken);
                Eat(TokenType.INTERGER, TokenType.BOOL, TokenType.STRING, TokenType.GUID, TokenType.DATETIME);
            } while (currentToken.type != TokenType.RBRACKET);
            Eat(TokenType.RBRACKET);
            return new Operand(tokens.ToArray());
        }

        Operand Operand()
        {
            if (currentToken.type == TokenType.LBRACKET)
            {
                return Array();
            }
            Token t = currentToken;
            Eat(TokenType.INTERGER, TokenType.BOOL, TokenType.STRING, TokenType.GUID, TokenType.DATETIME, TokenType.NULL);
            return new Operand(t);
        }

        Operator Operator()
        {
            /*
             * (TYPE) ? (NEGATE) ? operand(EQUAL | NOTEQUAL | LARGER | LARGEREQUAL | LESSER | LESSEREQUAL | IN | ISEMPTY) operand
             */
            Operator op = currentToken.type.ToOperator();
            Eat(TokenType.EQUAL, TokenType.NOTEQUAL, TokenType.LARGER, TokenType.LARGEREQUAL, TokenType.LESSER, TokenType.LESSEREQUAL, TokenType.CONTAINS, TokenType.IN, TokenType.ISEMPTY);
            return op;
        }

        Condition[] Condition()
        {
            /*
             * condition : LPAREN (NOT)? (TYPE)? operand operator operand ( (AND|OR) (TYPE)? (NEGATE)? operand operator operand )*? RPAREN
             */
            Eat(TokenType.LPAREN);

            List<Condition> conditions = new List<Condition>();
            ParseConditionClause(conditions, Conjunction.None);

            if (currentToken.type == TokenType.AND || currentToken.type == TokenType.OR)
            {
                do
                {
                    Conjunction conjunction = currentToken.type.ToConjunction();
                    Eat(TokenType.AND, TokenType.OR);
                    ParseConditionClause(conditions, conjunction);
                } while (currentToken.type != TokenType.RPAREN);
            }
            Eat(TokenType.RPAREN);
            conditions.First().Conjunction = Conjunction.None;
            return conditions.ToArray();

            void ParseConditionClause(List<Condition> conditions, Conjunction conjunction)
            {
                ValType type = ValType.Invalid;
                bool isNegated = false;
                Operand lhs;
                Operator op;
                Operand rhs = null;

                if (currentToken.type == TokenType.NOT)
                {
                    isNegated = true;
                    Eat(TokenType.NOT);
                }
                if (currentToken.type == TokenType.TYPE)
                {
                    type = currentToken.lexeme.ToValType();
                    Eat(TokenType.TYPE);
                }
                lhs = Operand();
                if (type == ValType.Invalid)
                {
                    type = lhs.type;
                }
                op = Operator();
                if (op != Datas.Operator.IsEmpty)
                {
                    rhs = Operand();
                }

                // type check
                //switch (type)
                //{
                //    case ValType.Guid:
                //        break;
                //    case ValType.Bool:
                //        break;
                //    case ValType.Int:
                //        break;
                //    case ValType.String:
                //        break;
                //    case ValType.DateTime:
                //        break;
                //    default:
                //        break;
                //}

                conditions.Add(new AST.Condition(conjunction, isNegated, type, lhs, op, rhs));
            }
        }

        ReturnStatement ReturnStatement()
        {
            Token ident = currentToken;
            Eat(TokenType.STRING);
            Eat(TokenType.ASSIGN);
            if (currentToken.type == TokenType.LBRACKET)
            {
                Eat(TokenType.LBRACKET);
                List<Token> results = new List<Token>();
                do
                {
                    if (currentToken.type != TokenType.STRING &&
                        currentToken.type != TokenType.RBRACKET)
                    {
                        Error(TokenType.RBRACKET, prevToken.lexeme.Length + 2);
                    }
                    var rtstmt = currentToken;
                    Eat(TokenType.STRING);
                    results.Add(rtstmt);
                } while (currentToken.type != TokenType.RBRACKET);
                Eat(TokenType.RBRACKET);
                return new ReturnStatement(ident, results.ToArray());
            }
            else
            {
                Token result = currentToken;
                Eat(TokenType.STRING);
                return new ReturnStatement(ident, result);
            }
        }

        ReturnStatement[] ReturnBlock()
        {
            /*
             * returnblock : returnstatement*
             * returnstatement : STRING ASSIGN STRING SEMICOLON | STRING ASSIGN LBRACKET STRING* RBRACKET
             */
            List<ReturnStatement> returnStatements = new List<ReturnStatement>();
            Eat(TokenType.LBRACE);
            do
            {
                returnStatements.Add(ReturnStatement());
            } while (currentToken.type != TokenType.RBRACE);
            Eat(TokenType.RBRACE);
            return returnStatements.ToArray();
        }

        IfStatement IfStatement()
        {
            /*
             * ifstatement : IF LPAREN condition RPAREN statement ( ELSE statement )?
             */

            switch (currentToken.type)
            {
                case TokenType.IF:
                    {
                        Eat(TokenType.IF);
                        Condition[] conditions = this.Condition();
                        ReturnStatement[] ifBody = this.ReturnBlock();

                        if (currentToken.type == TokenType.ELSE)
                        {
                            Eat(TokenType.ELSE);
                            ReturnStatement[] elseBody = this.ReturnBlock();
                            return new IfStatement(conditions, ifBody, new IfStatement(new Condition[0], elseBody, null));
                        }
                        else if (currentToken.type == TokenType.ELIF)
                        {
                            IfStatement elifBody = this.IfStatement();
                            elifBody.isElif = true;
                            return new IfStatement(conditions, ifBody, elifBody);
                        }

                        return new IfStatement(conditions, ifBody, null);
                    }
                case TokenType.ELIF:
                    {
                        Eat(TokenType.ELIF);
                        Condition[] conditions = this.Condition();
                        ReturnStatement[] ifBody = this.ReturnBlock();

                        if (currentToken.type == TokenType.ELSE)
                        {
                            Eat(TokenType.ELSE);
                            ReturnStatement[] elseBody = this.ReturnBlock();
                            return new IfStatement(conditions, ifBody, new IfStatement(new Condition[0], elseBody, null));
                        }
                        else if (currentToken.type == TokenType.ELIF)
                        {
                            IfStatement elifBody = this.IfStatement();
                            return new IfStatement(conditions, ifBody, elifBody);
                        }

                        return new IfStatement(conditions, ifBody, null);
                    }
                default:
                    Error("Expecting IF or ELIF!");
                    return null;
            }
        }

        public Policy Parse()
        {
            List<IfStatement> stmts = new List<IfStatement>();
            do
            {
                stmts.Add(IfStatement());
            } while (currentToken.type != TokenType.EOF && currentToken.type != TokenType.SEMICOLON);

            return new Policy(stmts.ToArray());
        }
    }
}
