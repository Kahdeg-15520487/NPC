using NPC.Compiler.Datas;

namespace NPC.Compiler.Contract
{
    interface ILexer
    {
        int CurrentLine { get; }
        string CurrentLineSource { get; }
        int CurrentPosInLine { get; }
        string CurrentLineContext { get; }

        void Error();
        Token GetNextToken();
        Token PeekNextToken();
    }
}