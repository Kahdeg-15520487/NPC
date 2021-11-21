using NPC.Compiler.AST;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Newtonsoft.Json.Linq;

using Rosen.Data.Source.PCS;
using Rosen.EMS.Infrastructure.DynamicConditionQuery.Dto;

using static Rosen.EMS.Infrastructure.DynamicConditionQuery.Object.ConditionEnum;
using System.Text;
using Newtonsoft.Json;
using NPC.Compiler.Datas;

namespace NPC.Runtime.Runtime
{
    public class Translator
    {
        private List<ConditionStatementDto> conditionStatementDtos;
        public Translator()
        {
            conditionStatementDtos = new List<ConditionStatementDto>();
        }

        public ConditionStatementDto[] Translate(Policy policy)
        {
            conditionStatementDtos.Clear();

            foreach (IfStatement ifStmt in policy.Statements)
            {
                Translate(ifStmt);
            }

            conditionStatementDtos.First().Conjunction = ConjunctionEnum.None;
            return conditionStatementDtos.ToArray();
        }

        private void Translate(IfStatement ifStmt, bool isElif = false)
        {
            ConditionStatementDto stmt = new ConditionStatementDto();
            stmt.Conjunction = isElif ? ConjunctionEnum.Or : ConjunctionEnum.And;
            List<ConditionClauseDto> conditionClauseDtos = new List<ConditionClauseDto>();
            if (ifStmt.condition.Length == 0)
            {
                //else clause
                conditionClauseDtos.Add(new ConditionClauseDto()
                {
                    Conjunction = ConjunctionEnum.None,
                    LogicCondition = true,
                    DataType = DataTypeEnum.Bool,
                    Criteria = true,
                    Operator = OperatorTypeEnum.Equal,
                    Value = true
                });
            }
            else
            {
                foreach (Condition cond in ifStmt.condition)
                {
                    ConditionClauseDto condClause = new ConditionClauseDto();
                    switch (cond.Conjunction)
                    {
                        case Compiler.Datas.Conjunction.And:
                            condClause.Conjunction = ConjunctionEnum.And;
                            break;
                        case Compiler.Datas.Conjunction.Or:
                            condClause.Conjunction = ConjunctionEnum.Or;
                            break;
                        default:
                            condClause.Conjunction = ConjunctionEnum.None;
                            break;
                    }
                    condClause.LogicCondition = !cond.Negate;
                    switch (cond.Type)
                    {
                        case Compiler.Datas.ValType.Guid:
                            condClause.DataType = DataTypeEnum.Guid;
                            break;
                        case Compiler.Datas.ValType.Bool:
                            condClause.DataType = DataTypeEnum.Bool;
                            break;
                        case Compiler.Datas.ValType.Int:
                            condClause.DataType = DataTypeEnum.Int;
                            break;
                        case Compiler.Datas.ValType.String:
                            condClause.DataType = DataTypeEnum.String;
                            break;
                        case Compiler.Datas.ValType.DateTime:
                            condClause.DataType = DataTypeEnum.DateTime;
                            break;
                        default:
                            condClause.DataType = DataTypeEnum.String;
                            break;
                    }
                    condClause.Criteria = JToken.Parse(ConvertToJson(cond.LHS.tokens[0]));
                    switch (cond.Operator)
                    {
                        case Compiler.Datas.Operator.Equal:
                            condClause.Operator = OperatorTypeEnum.Equal;
                            break;
                        case Compiler.Datas.Operator.Contain:
                            condClause.Operator = OperatorTypeEnum.Contain;
                            break;
                        case Compiler.Datas.Operator.Greater:
                            condClause.Operator = OperatorTypeEnum.Greater;
                            break;
                        case Compiler.Datas.Operator.Less:
                            condClause.Operator = OperatorTypeEnum.Less;
                            break;
                        case Compiler.Datas.Operator.GreaterOrEqual:
                            condClause.Operator = OperatorTypeEnum.GreaterOrEqual;
                            break;
                        case Compiler.Datas.Operator.LessOrEqual:
                            condClause.Operator = OperatorTypeEnum.LessOrEqual;
                            break;
                        case Compiler.Datas.Operator.In:
                            condClause.Operator = OperatorTypeEnum.In;
                            break;
                        case Compiler.Datas.Operator.IsEmpty:
                            condClause.Operator = OperatorTypeEnum.IsEmpty;
                            break;
                        default:
                            condClause.Operator = OperatorTypeEnum.Equal;
                            break;
                    }

                    if (cond.Operator != Compiler.Datas.Operator.IsEmpty)
                    {
                        if (cond.RHS.IsArray)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("[");
                            sb.Append(string.Join(", ", cond.RHS.tokens.Select(t =>
                            {
                                return ConvertToJson(t);
                            })));
                            sb.Append("]");
                            condClause.Value = JToken.Parse(sb.ToString());
                        }
                        else
                        {
                            //todo handle null value and empty array
                            condClause.Value = JToken.Parse(ConvertToJson(cond.RHS.tokens[0]));
                        }
                    }
                    conditionClauseDtos.Add(condClause);
                }
            }
            stmt.If = conditionClauseDtos.ToArray();
            List<ResultClauseDto> resultClauseDtos = new List<ResultClauseDto>();
            foreach (var rtstmt in ifStmt.ifBody)
            {
                resultClauseDtos.Add(new ResultClauseDto()
                {
                    Criteria = rtstmt.Ident.lexeme,
                    Value = rtstmt.Results.Select(r => r.lexeme)
                });
            }
            stmt.Then = resultClauseDtos.Select(rcd => JToken.FromObject(rcd)).ToArray();

            conditionStatementDtos.Add(stmt);

            if (ifStmt.elseBody != null)
            {
                Translate(ifStmt.elseBody, true);
            }
        }

        private static string ConvertToJson(Token t)
        {
            if (t.type == TokenType.NULL)
            {
                return JsonConvert.SerializeObject(null);
            }
            switch (t.type.ToValType())
            {
                case Compiler.Datas.ValType.Bool:
                case Compiler.Datas.ValType.Int:
                case Compiler.Datas.ValType.Guid:
                case Compiler.Datas.ValType.String:
                case Compiler.Datas.ValType.DateTime:
                    return $"\"{t.lexeme}\"";
                default:
                    return (string)null;
            }
        }
    }
}
