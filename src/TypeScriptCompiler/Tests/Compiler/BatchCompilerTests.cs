using System.Collections.Generic;
using NUnit.Framework;
using TypeScript.Compiler;
using TypeScript.Compiler.Data;
using TypeScript.Compiler.IOHosts;

namespace Tests.Compiler
{
    [TestFixture]
    public class BatchCompilerTests
    {
        [Test]
        public void Single()
        {
            var batchCompiler = new BatchCompiler(new MemoryIOHost(new Node
            {
                Children = new List<Node>
                {
                    new FileNode("test.ts", "var a = 4364;")
                }
            }));

            batchCompiler.CompilationEnvironment.Code.Add(new SourceUnit("test.ts"));

            batchCompiler.Resolve();
        }

        [Test]
        public void Multi()
        {
            var batchCompiler = new BatchCompiler(new MemoryIOHost(new Node
            {
                Children = new List<Node>
                {
                    new DirectoryNode("tres")
                    {
                        Children = new List<Node>
                        {
                            new FileNode("dos.ts",
                                "///<reference path='missing.ts' />\n" +
                                "var c = 8653;")
                        }
                    },

                    new FileNode("test.ts",
                        "///<reference path='uno.ts' />\n" +
                        "var a = 4364;"),

                    new FileNode("uno.ts",
                        "// uno\n" +
                        "var g = 1235;\n" +
                        "///<reference path='tres/dos.ts' />\n" +
                        "var b = 6684;")
                }
            }));

            batchCompiler.CompilationEnvironment.Code.Add(new SourceUnit("test.ts"));

            batchCompiler.Resolve();
        }
    }
}
