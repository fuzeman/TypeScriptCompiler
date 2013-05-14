using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TypeScript.Compiler;
using TypeScript.Compiler.Data;
using TypeScript.Compiler.IOHosts;

namespace Tests.Compiler
{
    [TestFixture]
    public class TypeScriptPlayExampleTests
    {
        [Test]
        public void PlayScriptCompiles()
        {
            var batchCompiler = new BatchCompiler(new MemoryIOHost(new Node
            {
                Children = new List<Node>
                {
                    new FileNode("test.ts", 
                        "class Greeter {" +
                        "	greeting: string;" +
                        "	constructor (message: string) {" +
                        "		this.greeting = message;" +
                        "	}" +
                        "	greet() {" +
                        "		return \"Hello, \" + this.greeting;" +
                        "	}" +
                        "}" +

                        "var greeter = new Greeter(\"world\");" +

                        "var button = document.createElement('button');" +
                        "button.innerText =\"Say Hello\";" +
                        "button.onclick = function() {" +
                        "	alert(greeter.greet());" +
                        "}" +

                        "document.body.appendChild(button);")
                }
            }));

            batchCompiler.CompilationEnvironment.Code.Add(new SourceUnit("test.ts"));

            batchCompiler.Run();

            batchCompiler.Result.Should().Be(
                "var Greeter = (function () {\r\n" +
                "    function Greeter(message) {\r\n" +
                "        this.greeting = message;\r\n" +
                "    }\r\n" +
                "    Greeter.prototype.greet = function () {\r\n" +
                "        return \"Hello, \" + this.greeting;\r\n" +
                "    };\r\n" +
                "    return Greeter;\r\n" +
                "})();\r\n" +
                "var greeter = new Greeter(\"world\");\r\n" +
                "var button = document.createElement('button');\r\n" +
                "button.innerText = \"Say Hello\";\r\n" +
                "button.onclick = function () {\r\n" +
                "    alert(greeter.greet());\r\n" +
                "};\r\n" +
                "document.body.appendChild(button);\r\n"
            );
        }
    }
}
