using System.Collections.Generic;
using Noesis.Javascript;
using TypeScript.Properties;

namespace TypeScript.Compiler
{
    public class JavascriptCompilerHost
    {
        private static readonly JavascriptContext Context;
        
        static JavascriptCompilerHost()
        {
            Context = new JavascriptContext();
            Context.Run(Resources.typescript_js);
            Context.Run(Resources.compiler_js);
        }

        #region Run(string code)

        public static object Run(string code)
        {
            return Run(code, "result");
        }

        public static T Run<T>(string code)
        {
            return Run<T>(code, "result");
        }

        #endregion

        #region Run(string code, string resultParameter)

        public static object Run(string code, string resultParameter)
        {
            return Run(code, resultParameter, null);
        }

        public static T Run<T>(string code, string resultParameter)
        {
            return Run<T>(code, resultParameter, null);
        }

        #endregion

        #region Run(string code, params KeyValuePair<string, object>[] arguments)

        public static object Run(string code, params KeyValuePair<string, object>[] arguments)
        {
            return Run(code, "result", arguments);
        }

        public static T Run<T>(string code, params KeyValuePair<string, object>[] arguments)
        {
            return Run<T>(code, "result", arguments);
        }

        #endregion

        #region Run(string code, string resultParameter, params KeyValuePair<string, object>[] arguments)

        public static object Run(string code, string resultParameter, params KeyValuePair<string, object>[] arguments)
        {
            if (arguments != null)
            {
                foreach (var pair in arguments)
                {
                    Context.SetParameter(pair.Key, pair.Value);
                }
            }

            Context.Run(code);

            return Context.GetParameter(resultParameter);
        }

        public static T Run<T>(string code, string resultParameter, params KeyValuePair<string, object>[] arguments)
        {
            return (T) Run(code, resultParameter, arguments);
        }

        #endregion
    }
}
