using NPC.Compiler.Datas;

using System;

namespace NPC.Compiler
{
    public class Error
    {
        public int Line { get; set; }
        public int PositionInLine { get; set; }
        public int Length { get; set; }
        public string Context { get; set; }
        public string ErrorMessage { get; set; }
        public string Message { get; set; }

        public Error(SyntaxErrorException ex)
        {
            Line = ex.Line;
            PositionInLine = ex.PositionInLine;
            Length = ex.Length;
            Context = ex.Context;
            ErrorMessage = ex.ErrorMessage;
            Message = ex.Message;
        }

        public Error(int line, int positionInLine, int length, string context, string message)
        {
            Line = line;
            PositionInLine = positionInLine;
            Length = length;
            Context = context;
            ErrorMessage = message;
            Message = $"Syntax error : {message} at ({line}|{positionInLine}) : {Environment.NewLine}====={Environment.NewLine}{context}{Environment.NewLine}{new string(' ', positionInLine)}^{Environment.NewLine}=====";
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
