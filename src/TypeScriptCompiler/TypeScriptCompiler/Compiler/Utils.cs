﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (((string) jsResult["errors"]).Trim().Length > 0)
            {
                Debug.WriteLine(jsResult["errors"]);
                Debug.WriteLine("---------------------------------");

                throw new CompilerException((string) jsResult["errors"]);
            }

            return (string) jsResult["source"];
        }

        #endregion
    }
}