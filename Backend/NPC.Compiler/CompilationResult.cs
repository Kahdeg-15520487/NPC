using NPC.Compiler.AST;

using System;
using System.Collections.Generic;
using System.Text;

namespace NPC.Compiler
{
    public class CompilationResult
    {
        public Policy Policy;
        public Error Error;

        public CompilationResult(Policy policy = null, Error error = null)
        {
            Policy = policy;
            Error = error;
        }
    }
}
