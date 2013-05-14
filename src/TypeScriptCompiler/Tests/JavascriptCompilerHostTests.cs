using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TypeScript;
using TypeScript.Compiler;

namespace Tests
{
    [TestFixture]
    public class JavascriptCompilerHostTests
    {
        [Test]
        public void Simple()
        {
            JavascriptCompilerHost.Run<int>("var result = 1534;").Should().Be(1534);

            JavascriptCompilerHost.Run<string>("var result = \"hello\" + \" world\";").Should().Be("hello world");

            JavascriptCompilerHost.Run<int>("var result = 1534 - 533;").Should().Be(1001);

            JavascriptCompilerHost.Run<int>("var result = input;", new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("input", 52978)
            }).Should().Be(52978);

            JavascriptCompilerHost.Run<int>("var result = input;",
                new KeyValuePair<string, object>("input", 52978)
            ).Should().Be(52978);

            JavascriptCompilerHost.Run<int>("var result = a + b;",
                new KeyValuePair<string, object>("a", 123),
                new KeyValuePair<string, object>("b", 321)
            ).Should().Be(444);

            JavascriptCompilerHost.Run<Dictionary<string, object>>(
                "var result = new TypeScript.SourceUnit(\"somefile.ts\", null);")
                ["path"].Should().Be("somefile.ts");

            ((Dictionary<string, object>) ((object[]) JavascriptCompilerHost.Run<Dictionary<string, object>>(
                "var result = TypeScript.preProcessFile(new TypeScript.SourceUnit(\"somefile.ts\", \"" +
                    "///<reference path='anotherfile.ts' />" +
                    "var a = 89273;" +
                "\"));")["referencedFiles"])[0])
                ["path"].Should().Be("anotherfile.ts");
        }
    }
}
