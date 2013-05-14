using Noesis.Javascript;
using TypeScript.Compiler;
using TypeScript.Properties;

namespace TypeScript
{
    public class TypeScriptCompiler
    {
        public string Compile(string jsCode)
        {
            var jsCompiler = Resources.compiler_js;
            var typeScript = Resources.typescript_js;
            string tsCode;
            string error;
            using (var context = new JavascriptContext())
            {
                context.SetParameter("jsCode", jsCode);
                context.Run(typeScript);
                context.Run(jsCompiler);
                tsCode = (string)context.GetParameter("tsCode");
                error = (string)context.GetParameter("error");
            }
            if (!string.IsNullOrEmpty(error.Trim()))
            {
                throw new CompilerException(error);
            }
            return tsCode.Trim();
        }
    }
}
