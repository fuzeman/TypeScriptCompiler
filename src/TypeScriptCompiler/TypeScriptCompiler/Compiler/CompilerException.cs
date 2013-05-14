using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;

namespace TypeScript.Compiler
{
    public class CompilerException : ApplicationException
    {
        public string[] CompilationFiles { get; set; }
        public string[] ResolvedFiles { get; set; }

        private readonly object[] _compilerErrors;
        private string _extendedMessage;

        public CompilerException(object[] compilerErrors)
            : base("TypeScript CompilerException")
        {
            _compilerErrors = compilerErrors;
        }

        public override string Message
        {
            get
            {
                if (ResolvedFiles != null && _extendedMessage == null && _compilerErrors.Length > 0)
                    _extendedMessage = BuildExtendedMessage();

                return _extendedMessage ?? base.Message;
            }
        }

        private string BuildExtendedMessage()
        {
            var m = "";

            foreach (var error in _compilerErrors)
            {
                var errorData = (Dictionary<string, object>) error;

                if ((string) errorData["type"] == "compiler")
                {
                    var path = "unknown";
                    if (ResolvedFiles.Length > (int) errorData["block"] - 1)
                        path = CleanFilePath(ResolvedFiles[(int) errorData["block"] - 1]);

                    m += string.Format("{0}[{1}, {2}]: {3}",
                        path, errorData["start"], errorData["len"], errorData["message"]) + "\r\n";
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            return m;
        }

        private static readonly string BaseAppPath = Directory.GetCurrentDirectory().Replace('/', '\\') + "\\";

        private static string CleanFilePath(string path)
        {
            if (path.StartsWith("internal:"))
                path = path.Substring(11);

            path = path.Replace('/', '\\');

            return path.StartsWith(BaseAppPath) ?
                path.Substring(BaseAppPath.Length) : path;
        }
    }
}