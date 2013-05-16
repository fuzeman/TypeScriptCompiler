using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TypeScript.Compiler.Data;

namespace TypeScript.Compiler
{
    public class BatchCompiler
    {
        public CompilationEnvironment CompilationEnvironment { get; set; }
        public CompilationEnvironment ResolvedEnvironment { get; private set; }
        public string Result { get; private set; }

        private readonly Dictionary<string, string> _pathMap;
        private readonly Dictionary<string, bool> _resolvedPaths;

        public BatchCompiler(IIOHost fileSystemHost)
        {
            CompilationEnvironment = new CompilationEnvironment(fileSystemHost);

            _pathMap = new Dictionary<string, string>();
            _resolvedPaths = new Dictionary<string, bool>();
        }

        public void Run()
        {
            Result = null;

            ResolvedEnvironment = Resolve(); // TODO: compilationSettings.resolve
            Result = Compile();
        }

        public CompilationEnvironment Resolve()
        {
            var resolver = new CodeResolver(CompilationEnvironment);

            // resolveCompilationEnvironment
            var preEnv = CompilationEnvironment;
            var resolvedEnv = new CompilationEnvironment(CompilationEnvironment.IOHost);
            var nCode = preEnv.Code.Count;
            var path = "";

            // Resolve Code
            for (var i = 0; i < nCode; i++)
            {
                path = CompilationEnvironment.IOHost.ResolvePath(preEnv.Code[i].Path);
                _pathMap[preEnv.Code[i].Path] = path;

                resolver.ResolveCode(path, "", false, (resolvedPath, code) =>
                {
                    var pathId = Utils.GetPathIdentifier(resolvedPath);
                    if (!_resolvedPaths.ContainsKey(pathId) || !_resolvedPaths[pathId])
                    {
                        resolvedEnv.Code.Add(code);
                        _resolvedPaths[pathId] = true;
                    }

                }, (errorFile, line, col, message) =>
                    Debug.WriteLine((line != null && col != null) ?
                        string.Format("{0} ({1},{2}) : {3}", errorFile, line, col, message) :
                        string.Format("{0} : {3}", errorFile, line, col, message)));
            }

            // Check if preEnv code was resolved
            foreach (var resolvedPath in from code in preEnv.Code
                                         where !IsResolved(code.Path)
                                         select code.Path)
            {
                if (!Utils.IsFileSTR(resolvedPath) && !Utils.IsFileDSTR(resolvedPath) &&
                    !Utils.IsFileTS(resolvedPath) && !Utils.IsFileDTS(resolvedPath))
                {
                    Debug.WriteLine("Unknown extension for file: \"" + resolvedPath + "\". Only .ts and .d.ts extensions are allowed.");
                }
                else
                {
                    Debug.WriteLine("Error reading file \"" + resolvedPath + "\": File not found");
                }
            }

            return resolvedEnv;
        }

        public bool IsResolved(string path)
        {
            return _resolvedPaths.ContainsKey(Utils.GetPathIdentifier(_pathMap[path]));
        }

        public string Compile()
        {
            try
            {
                return Utils.Compile(ResolvedEnvironment);
            }
            catch (CompilerException ex)
            {
                // Add environment details and rethrow
                ex.CompilationFiles = CompilationEnvironment.Code.Select(c => c.Path).ToArray();
                ex.ResolvedFiles = ResolvedEnvironment.Code.Select(c => c.Path).ToArray();

                throw ex;
            }
        }
    }
}
