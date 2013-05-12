using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TypeScript.Compiler.Data;

namespace TypeScript.Compiler
{
    public class Utils
    {
        #region Paths

        public static string SwitchToForwardSlashes(string path)
        {
            return path.Replace("\\", "/");
        }

        #endregion

        public static PreProcessedFileInfo PreProcessFile(SourceUnit sourceUnit)
        {
            var jsResult = JavascriptCompilerHost.Run<Dictionary<string, object>>(
                "var result = TypeScript.preProcessFile(new TypeScript.SourceUnit(path, content));",
                new KeyValuePair<string, object>("path", sourceUnit.Path),
                new KeyValuePair<string, object>("content", sourceUnit.Content));

            return PreProcessedFileInfo.FromJavascriptResult(jsResult);
        }
    }
}
