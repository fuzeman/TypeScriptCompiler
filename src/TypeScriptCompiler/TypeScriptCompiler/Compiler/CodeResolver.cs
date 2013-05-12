using System.Collections.Generic;
using System.Diagnostics;
using TypeScript.Compiler.Data;

namespace TypeScript.Compiler
{
    public class CodeResolver
    {
        public delegate void ResolutionDelegate(string path, SourceUnit sourceUnit);
        public delegate void ResolutionErrorDelegate(string path, int line, int col, string message);

        private CompilationEnvironment _environment;
        private IIOHost _ioHost;
        private Dictionary<string, bool> _visted;

        public CodeResolver(CompilationEnvironment environment)
        {
            _environment = environment;
            _ioHost = environment.IOHost;

            _visted = new Dictionary<string, bool>();
        }

        public bool ResolveCode(string referencePath, string parentPath, bool performSearch,
            ResolutionDelegate resolutionCallback, ResolutionErrorDelegate resolutionErrorCallback)
        {
            var resolvedFile = new SourceUnit(referencePath);

            // normalize resolvedFile.Path
            var isRelativePath = _ioHost.IsRelative(resolvedFile.Path);
            var isAbsolutePath = !isRelativePath && _ioHost.IsAbsolute(resolvedFile.Path);

            resolvedFile.Path = isRelativePath
                ? _ioHost.ResolvePath(parentPath + "/" + referencePath)
                : (isAbsolutePath || parentPath == null || performSearch)
                    ? referencePath : parentPath + "/" + referencePath;

            var absoluteModuleId = resolvedFile.Path.ToUpper(); // TODO: Case sensitive resolution

            if (!_visted.ContainsKey(absoluteModuleId) || !_visted[absoluteModuleId])
            {
                if (_ioHost.IsRelative(resolvedFile.Path) || _ioHost.IsAbsolute(resolvedFile.Path) || !performSearch)
                {
                    Debug.WriteLine("Reading code from " + resolvedFile.Path);

                    resolvedFile.Content = _environment.IOHost.ReadFile(resolvedFile.Path);

                    Debug.WriteLine("Found code at " + resolvedFile.Path);
                    _visted[absoluteModuleId] = true;
                }
                else
                {
                    // TODO - findFile
                }

                // Process File
                if (resolvedFile.Content != null)
                {
                    var rootDir = _environment.IOHost.DirectoryName(resolvedFile.Path);

                    // Find references in file
                    var preProcessedFileInfo = Utils.PreProcessFile(resolvedFile);
                    resolvedFile.ReferencedFiles = preProcessedFileInfo.ReferencedFiles;

                    foreach (var fileReference in preProcessedFileInfo.ReferencedFiles)
                    {
                        // Normalize path (ensure absolute)
                        var normalizedPath = _ioHost.IsAbsolute(fileReference.Path)
                            ? fileReference.Path
                            : rootDir + "/" + fileReference.Path;
                        normalizedPath = _ioHost.ResolvePath(normalizedPath);

                        if (resolvedFile.Path == normalizedPath)
                        {
                            // TODO: post error - file is referencing itself
                            continue;
                        }

                        // Resolve Reference File
                        var resolutionResult = ResolveCode(fileReference.Path, rootDir, false,
                            resolutionCallback, resolutionErrorCallback);

                        if (!resolutionResult)
                        {
                            // TODO: post error - incorrect reference
                        }
                    }

                    // TODO: foreach fileImport in importedFiles
                }
            }

            return true;
        }
    }
}
