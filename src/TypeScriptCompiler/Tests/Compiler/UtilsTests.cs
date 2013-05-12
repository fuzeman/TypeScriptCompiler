using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TypeScript.Compiler;
using TypeScript.Compiler.Data;

namespace Tests.Compiler
{
    [TestFixture]
    public class UtilsTests
    {
        [Test]
        public void PreProcessFile()
        {
            Utils.PreProcessFile(new SourceUnit("somefile.ts",
                "///<reference path='anotherfile.ts' />\n" +
                "var a = 253;"
            )).ReferencedFiles[0].Path.Should().Be("anotherfile.ts");

            Utils.PreProcessFile(new SourceUnit("somefile.ts",
                "///<reference path='file-a.ts' />\n" +
                "///<reference path='file-b.ts' />\n" +
                "///<reference path='somedirectory/file-c.ts' />\n" +
                "var a = 253;"
            )).ReferencedFiles.Select(file => file.Path)
              .Should().Contain(new[] {"file-a.ts", "file-b.ts", "somedirectory/file-c.ts"});
        }
    }
}
