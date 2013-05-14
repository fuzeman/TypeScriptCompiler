using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TypeScript.Compiler;
using TypeScript.Compiler.Data;
using TypeScript.Compiler.IOHosts;

namespace Tests.Compiler
{
    [TestFixture]
    public class BatchCompilerTests
    {
        private static readonly Regex VariableNameRegex = new Regex("var\\s(\\w+)\\s=\\s\\d+;", RegexOptions.IgnoreCase);

        [Test]
        public void Success_Single()
        {
            var batchCompiler = new BatchCompiler(new MemoryIOHost(new Node
            {
                Children = new List<Node>
                {
                    new FileNode("test.ts", "var a = 4364;")
                }
            }));

            batchCompiler.CompilationEnvironment.Code.Add(new SourceUnit("test.ts"));

            batchCompiler.Run();

            VariableNameRegex.Matches(batchCompiler.Result)
                             .Cast<Match>().Select(m => m.Groups[1].Value)
                             .Should().Contain(new[] { "a" });
        }

        [Test]
        public void Success_Multi()
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

            batchCompiler.Run();

            VariableNameRegex.Matches(batchCompiler.Result)
                             .Cast<Match>().Select(m => m.Groups[1].Value)
                             .Should().Contain(new[] {"c", "g", "b", "a"});
        }

        [Test]
        public void Fail_Single()
        {
            var batchCompiler = new BatchCompiler(new MemoryIOHost(new Node
            {
                Children = new List<Node>
                {
                    new FileNode("test.ts", "var a = 4364-;")
                }
            }));

            batchCompiler.CompilationEnvironment.Code.Add(new SourceUnit("test.ts"));

            try
            {
                batchCompiler.Run();
                Assert.Fail("Compilation did not fail.");
            }
            catch (CompilerException exception)
            {
                exception.Message.Should().Be(
                    "Compilation error: " +
                    "Check format of expression term, " +
                    "Code block: 1, Start position: 13, Length: 1\r\n"
                );
            }
        }

        [Test]
        public void SimpleClassDeclaration()
        {
            var batchCompiler = new BatchCompiler(new MemoryIOHost(new Node
            {
                Children = new List<Node>
                {
                    new FileNode("test.ts", "class Person{}")
                }
            }));

            batchCompiler.CompilationEnvironment.Code.Add(new SourceUnit("test.ts"));

            batchCompiler.Run();

            batchCompiler.Result.Should().Be(
                "var Person = (function () {\r\n" +
                "    function Person() { }\r\n" +
                "    return Person;\r\n" +
                "})();\r\n");
        }
    }
}
