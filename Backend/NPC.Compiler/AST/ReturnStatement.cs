using NPC.Compiler.Datas;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Compiler.AST
{
    public class ReturnStatement : ASTNode
    {
        public Token Ident;
        public Token[] Results;

        public ReturnStatement(Token ident, params Token[] results)
        {
            Ident = ident;
            Results = results;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            if (Results.Length == 1)
            {
                return $"'{Ident.lexeme}' := '{Results[0].lexeme}'";
            }
            else
            {
                return $"'{Ident.lexeme}' := '{string.Join(", ", Results.Select(r => r.lexeme))}'";
            }
        }
    }
}
