using System;
using System.Collections.Generic;
using TypeScript.Compiler.Data;

namespace TypeScript.Compiler
{
    public class BatchCompiler
    {
        public CompilationEnvironment CompilationEnvironment { get; set; }

        private Dictionary<string, string> _pathMap;

        public BatchCompiler(IIOHost fileSystemHost)
        {
            CompilationEnvironment = new CompilationEnvironment(fileSystemHost);

            _pathMap = new Dictionary<string, string>();
        }

        public void Resolve()
        {
            var resolver = new CodeResolver(CompilationEnvironment);

            // resolveCompilationEnvironment
            var preEnv = CompilationEnvironment;
            var resolvedEnv = new CompilationEnvironment();
            var nCode = preEnv.Code.Count;
            var path = "";

            for (var i = 0; i < nCode; i++)
            {
                path = Utils.SwitchToForwardSlashes(CompilationEnvironment.IOHost.ResolvePath(preEnv.Code[i].Path));
                _pathMap[preEnv.Code[i].Path] = path;

                resolver.ResolveCode(path, "", false, (resolvedPath, sourceUnit) =>
                {
                    Console.WriteLine("Resolved " + resolvedPath);
                }, (errorFile, line, col, message) =>
                {
                    Console.WriteLine(string.Format("{0} ({1},{2}) : {3}", errorFile, line, col, message));
                });
            }
        }

        public void Compile()
        {
            
        }
    }
}
