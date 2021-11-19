using System;
using System.Collections.Generic;
using System.Text;

namespace NPC.Compiler.AST
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }

    public interface IVisitor
    {
        void Visit(IfStatement ifstmt);
        void Visit(Condition condition);
        void Visit(Operand operand);
        void Visit(ReturnStatement rtstmt);
        void Visit(Policy policy);
    }

    public abstract class ASTNode : IVisitable
    {
        public abstract void Accept(IVisitor visitor);
    }
}
