using NPC.Compiler.AST;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Runtime.Runtime
{
    class PrettyPrint
    {
        private StringBuilder result;
        public PrettyPrint()
        {
            result = new StringBuilder();
        }
        public string Beautify(Policy policy)
        {
            foreach (IfStatement ifStmt in policy.Statements)
            {
                Beautify(ifStmt);
            }

            return result.ToString();
        }

        private void Beautify(IfStatement ifStmt, bool isElif = false)
        {
            if (isElif)
            {
                result.Append("elif (");
            }
            else
            {
                result.Append("if (");
            }

            foreach (Condition cond in ifStmt.condition)
            {
                switch (cond.Conjunction)
                {
                    case Compiler.Datas.Conjunction.And:
                        result.AppendLine("and ");
                        result.Append("    ");
                        break;
                    case Compiler.Datas.Conjunction.Or:
                        result.AppendLine("or ");
                        result.Append("    ");
                        break;
                    default:
                        break;
                }
                if (cond.Negate)
                {
                    result.Append("not ");
                }
                switch (cond.Type)
                {
                    case Compiler.Datas.ValType.Guid:
                        result.Append("guid ");
                        break;
                    case Compiler.Datas.ValType.Bool:
                        result.Append("bool ");
                        break;
                    case Compiler.Datas.ValType.Int:
                        result.Append("int ");
                        break;
                    case Compiler.Datas.ValType.DateTime:
                        result.Append("datetime ");
                        break;
                    default:
                        result.Append("str ");
                        break;
                }

                result.Append(Beautify(cond.LHS.tokens[0]));

                switch (cond.Operator)
                {
                    case Compiler.Datas.Operator.Contain:
                        result.Append("contains ");
                        break;
                    case Compiler.Datas.Operator.Greater:
                        result.Append("> ");
                        break;
                    case Compiler.Datas.Operator.Less:
                        result.Append("< ");
                        break;
                    case Compiler.Datas.Operator.GreaterOrEqual:
                        result.Append(">= ");
                        break;
                    case Compiler.Datas.Operator.LessOrEqual:
                        result.Append("<= ");
                        break;
                    case Compiler.Datas.Operator.In:
                        result.Append("in ");
                        break;
                    case Compiler.Datas.Operator.IsEmpty:
                        result.Append("isempty ");
                        break;
                    default:
                        result.Append("== ");
                        break;
                }

                if (cond.Operator != Compiler.Datas.Operator.IsEmpty)
                {
                    if (cond.RHS.IsArray)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("[ ");
                        sb.Append(string.Join(" ", cond.RHS.tokens.Select(t => Beautify(t))));
                        sb.Append("]");
                        result.AppendFormat("{0} ", sb.ToString());
                    }
                    else
                    {
                        //todo handle null value and empty array
                        result.Append(Beautify(cond.RHS.tokens[0]));
                    }
                }
            }
            result.AppendLine("){");
            foreach (var rtstmt in ifStmt.ifBody)
            {
                result.AppendFormat("\t\"{0}\" = [ \"{1}\" ]", rtstmt.Ident.lexeme, string.Join(" ", rtstmt.Results.Select(r => r.lexeme)));
            }
            result.AppendLine();
            result.AppendLine("}");
            if (ifStmt.elseBody != null)
            {
                Beautify(ifStmt.elseBody, true);
            }
        }

        private string Beautify(Compiler.Datas.Token lhs)
        {
            switch (lhs.type)
            {
                case Compiler.Datas.TokenType.INTERGER:
                case Compiler.Datas.TokenType.BOOL:
                    return string.Format("{0} ", lhs.lexeme);
                case Compiler.Datas.TokenType.STRING:
                    return string.Format("\"{0}\" ", lhs.lexeme);
                case Compiler.Datas.TokenType.GUID:
                    return string.Format("g\"{0}\" ", lhs.lexeme);
                case Compiler.Datas.TokenType.DATETIME:
                    return string.Format("d\"{0}\" ", lhs.lexeme);
                case Compiler.Datas.TokenType.NULL:
                    return "null ";
                default:
                    return string.Format("{0} ", lhs.lexeme);
            }
        }
    }
}
