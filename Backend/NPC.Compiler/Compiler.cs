using NPC.Compiler.AST;
using NPC.Compiler.Contract;
using NPC.Compiler.Implementation;

namespace NPC.Compiler
{
    public class Compiler
    {
        public static Policy Compile(string source)
        {
            ILexer lexer = new Lexer(source);
            IParser parser = new Parser(lexer);
            return parser.Parse();
        }
    }
}
