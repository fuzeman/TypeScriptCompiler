using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TypeScript.Compiler.Data;

namespace TypeScript.Compiler
{
    public class Utils
    {
        #region Paths

        public static bool IsFileExtension(string filename, string extension)
        {
            var uFilename = filename.ToUpper();
            var uExt = extension.ToUpper();

            return uFilename.Length > uExt.Length &&
                uFilename.Substring(uFilename.Length - uExt.Length, uExt.Length) == uExt;
        }

        public static bool IsFileJS(string filename)
        {
            return IsFileExtension(filename, ".js");
        }

        public static bool IsFileSTR(string filename)
        {
            return IsFileExtension(filename, ".str");
        }

        public static bool IsFileTS(string filename)
        {
            return IsFileExtension(filename, ".ts");
        }

        public static bool IsFileDSTR(string filename)
        {
            return IsFileExtension(filename, ".d.str");
        }

        public static bool IsFileDTS(string filename)
        {
            return IsFileExtension(filename, ".d.ts");
        }

        public static string GetPathIdentifier(string path)
        {
            return path.ToUpper();
        }

        #endregion

        #region JavascriptCompilerHost calls

        public static PreProcessedFileInfo PreProcessFile(SourceUnit sourceUnit)
        {
            var jsResult = JavascriptCompilerHost.Run<Dictionary<string, object>>(
                "var result = TypeScript.preProcessFile(new TypeScript.SourceUnit(path, content));",
                new KeyValuePair<string, object>("path", sourceUnit.Path),
                new KeyValuePair<string, object>("content", sourceUnit.Content));

            return PreProcessedFileInfo.FromJavascriptResult(jsResult);
        }

        public static string Compile(CompilationEnvironment resolvedEnvironment)
        {
            var jsEnv = resolvedEnvironment.ToJavascriptObject();

            var jsResult = JavascriptCompilerHost.Run<Dictionary<string, object>>(
                "reset(); var result = compile(buildEnvironment());",
                new KeyValuePair<string, object>("environment", jsEnv));

            Debug.WriteLine("---------------------------------");
            Debug.WriteLine(jsResult["log"]);
            Debug.WriteLine("---------------------------------");

            // Throw on errors
            var errors = (object[]) jsResult["errors"];

            if (errors.Length > 0)
            {
                foreach (var error in errors)
                {
                    var errorData = (Dictionary<string, object>) error;

                    if ((string) errorData["type"] == "compiler")
                    {
                        Debug.WriteLine("Block {0} [{1}, {2}] {3}", errorData["block"],
                            errorData["start"], errorData["len"], errorData["message"]);
                    }
                    else if ((string) errorData["type"] == "exception")
                    {
                        Debug.WriteLine(errorData["exception"]);
                    }
                }
                Debug.WriteLine("---------------------------------");

                throw new CompilerException((object[]) jsResult["errors"]);
            }

            return (string) jsResult["source"];
        }

        #endregion
    }
}
