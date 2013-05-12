using System.Collections.Generic;
using Noesis.Javascript;
using TypeScript.Properties;

namespace TypeScript
{
    public class JavascriptCompilerHost
    {
        private static readonly JavascriptContext _javascriptContext;
        
        static JavascriptCompilerHost()
        {
            _javascriptContext = new JavascriptContext();
            _javascriptContext.Run(Resources.typescript_js);
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
                    _javascriptContext.SetParameter(pair.Key, pair.Value);
                }
            }

            _javascriptContext.Run(code);

            return _javascriptContext.GetParameter(resultParameter);
        }

        public static T Run<T>(string code, string resultParameter, params KeyValuePair<string, object>[] arguments)
        {
            return (T) Run(code, resultParameter, arguments);
        }

        #endregion
    }
}
