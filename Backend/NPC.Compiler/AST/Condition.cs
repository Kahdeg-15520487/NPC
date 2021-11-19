using NPC.Compiler.Datas;

using System;
using System.Collections.Generic;
using System.Text;

namespace NPC.Compiler.AST
{
    public class Condition : IVisitable
    {
        public Conjunction Conjunction;
        public bool Negate;
        public ValType Type;
        public Operand LHS;
        public Operator Operator;
        public Operand RHS;

        public Condition(Conjunction conjunction, bool negate, ValType type, Operand lHS, Operator op, Operand rHS)
        {
            this.Conjunction = conjunction;
            this.Negate = negate;
            this.Type = type;
            this.LHS = lHS;
            this.Operator = op;
            this.RHS = rHS;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Conjunction != Conjunction.None)
            {
                sb.AppendFormat("{0} ", Conjunction);
            }
            sb.AppendFormat("{0} ", Type);
            sb.AppendFormat("{0} ", LHS);
            sb.AppendFormat("{0} ", Operator);
            if (Operator != Operator.IsEmpty)
            {
                sb.AppendFormat("{0} ", RHS);
            }
            return sb.ToString();
        }
    }
}
