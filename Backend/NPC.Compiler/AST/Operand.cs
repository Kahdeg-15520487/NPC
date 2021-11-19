using NPC.Compiler.Datas;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Compiler.AST
{
    public class Operand : ASTNode
    {
        public Token[] tokens { get; private set; }
        public ValType type { get; private set; }

        public Operand(Token t)
        {
            tokens = new Token[] { t };
            switch (t.type)
            {
                case TokenType.INTERGER:
                    type = ValType.Int;
                    break;
                case TokenType.BOOL:
                    type = ValType.Bool;
                    break;
                case TokenType.STRING:
                    type = ValType.String;
                    break;
                case TokenType.GUID:
                    type = ValType.Guid;
                    break;
                case TokenType.DATETIME:
                    type = ValType.DateTime;
                    break;
            }
        }

        public Operand(params Token[] ts)
        {
            if (ts.Length == 0)
            {
                tokens = new Token[0];
                type = ValType.String;
            }
            else
            {
                tokens = ts.ToList().ToArray();
                switch (ts.First().type)
                {
                    case TokenType.INTERGER:
                        type = ValType.Int;
                        break;
                    case TokenType.BOOL:
                        type = ValType.Bool;
                        break;
                    case TokenType.STRING:
                        type = ValType.String;
                        break;
                    case TokenType.GUID:
                        type = ValType.Guid;
                        break;
                    case TokenType.DATETIME:
                        type = ValType.DateTime;
                        break;
                }
            }
        }

        public bool IsArray { get => tokens.Length != 1; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 91;
                hash = hash * 71 + tokens.Sum(t => hash * 71 + t.GetHashCode());
                hash = hash * 71 + type.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            if (tokens.Length == 0)
            {
                return $"<{type}[]| >";
            }
            else if (tokens.Length == 1)
            {
                return $"<{type}|{tokens.First().lexeme}>";
            }
            else
            {
                return $"<{type}[]|{string.Join(", ", tokens.Select(t => t.lexeme))}";
            }
        }
    }
}
