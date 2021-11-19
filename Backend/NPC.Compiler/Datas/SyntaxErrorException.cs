using System;
using System.Collections.Generic;
using System.Text;

namespace NPC.Compiler.Datas
{

    [Serializable]
    public class SyntaxErrorException : Exception
    {
        public int Line;
        public int PositionInLine;
        public int Length;
        public string Context;
        public string ErrorMessage;

        public SyntaxErrorException() { }
        public SyntaxErrorException(string message) : base(message) { }
        public SyntaxErrorException(string message, Exception inner) : base(message, inner) { }

        public SyntaxErrorException(int line, int positionInLine, int length, string context, string message) : base($"Syntax error : {message} at ({line}|{positionInLine}) : {Environment.NewLine}====={Environment.NewLine}{context}{Environment.NewLine}{new string(' ', positionInLine)}^{Environment.NewLine}=====")
        {
            Line = line;
            PositionInLine = positionInLine;
            Length = length;
            Context = context;
            ErrorMessage = message;
        }

        protected SyntaxErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
