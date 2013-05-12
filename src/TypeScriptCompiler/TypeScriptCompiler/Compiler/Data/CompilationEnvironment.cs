using System.Collections.Generic;

namespace TypeScript.Compiler.Data
{
    public class CompilationEnvironment
    {
        public List<SourceUnit> Code { get; private set; }
        public IIOHost IOHost { get; private set; }

        public CompilationEnvironment()
        {
            Code = new List<SourceUnit>();
        }

        public CompilationEnvironment(IIOHost ioHost)
        {
            Code = new List<SourceUnit>();
            IOHost = ioHost;
        }
    }
}
