using NPC.Compiler.AST;
using NPC.Compiler.Contract;
using NPC.Compiler.Datas;
using NPC.Compiler.Implementation;

using System;

namespace NPC.Compiler
{
    public class Compiler
    {
        public static (Policy policy, Error error) Compile(string source)
        {
            ILexer lexer = new Lexer(source);
            IParser parser = new Parser(lexer);
            try
            {
                return (parser.Parse(), null);
            }
            catch (SyntaxErrorException ex)
            {
                return (null, new Error(ex));
            }
        }
    }
}
