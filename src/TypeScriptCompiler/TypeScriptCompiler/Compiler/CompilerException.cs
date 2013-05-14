using System;

namespace TypeScript.Compiler
{
    public class CompilerException : ApplicationException
    {
        public CompilerException(string message)
            : base(message)
        {
        }
    }
}