using System;
using System.Collections.Generic;
using System.Text;

namespace NPC.Compiler.AST
{
    public class Policy : ASTNode
    {
        public IfStatement[] Statements;
        public Policy(IfStatement[] statements)
        {
            this.Statements = statements;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return string.Join<IfStatement>(Environment.NewLine, Statements);
        }
    }
}
