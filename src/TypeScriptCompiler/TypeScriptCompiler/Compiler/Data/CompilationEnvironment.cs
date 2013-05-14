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

        public Dictionary<string, object> ToJavascriptObject()
        {
            var result = new Dictionary<string, object>();

            result["code"] = new object[Code.Count];

            for (var i = 0; i < Code.Count; i++)
            {
                ((object[]) result["code"])[i] = Code[i].ToJavascriptObject();
            }

            return result;
        }
    }
}
