using System;
using System.Collections.Generic;
using System.Text;

namespace NPC.Compiler.AST
{
    public class IfStatement : ASTNode
    {
        public Condition[] condition;
        public ReturnStatement[] ifBody;
        public IfStatement elseBody;
        public bool isElif;

        public IfStatement(Condition[] condition, ReturnStatement[] ifBody, IfStatement elseBody, bool isElif = false)
        {
            this.condition = condition;
            this.ifBody = ifBody;
            this.elseBody = elseBody;
            this.isElif = isElif;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (condition.Length == 0)
            {
                // else body of an if
                sb.AppendLine("else");
            }
            else
            {
                if (isElif)
                {
                    sb.Append("elif (");
                }
                else
                {
                    sb.Append("if (");
                }
                foreach (Condition c in condition)
                {
                    sb.Append(c);
                }
                sb.AppendLine(")");
            }
            sb.AppendLine("{");
            foreach (ReturnStatement rt in ifBody)
            {
                sb.AppendLine($"\t{rt}");
            }
            sb.AppendLine("}");
            if (elseBody != null)
            {
                sb.AppendLine(elseBody.ToString());
            }

            return sb.ToString();
        }
    }
}
