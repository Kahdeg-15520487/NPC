using NPC.Compiler.AST;

using System;
using System.Collections.Generic;
using System.Text;

namespace NPC.Compiler.Contract
{
    interface IParser
    {
        Policy Parse();
    }
}
