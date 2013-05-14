using System.Collections.Generic;
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

        public delegate bool FileExtensionMethodDelegate(string filename);

        public static List<FileExtensionMethodDelegate> FileExtensionMethods = new List<FileExtensionMethodDelegate>
        {
            Utils.IsFileDSTR, Utils.IsFileSTR,
            Utils.IsFileDTS, Utils.IsFileTS,
            Utils.IsFileJS,
        };

        public static Dictionary<string, FileExtensionMethodDelegate[]> FileExtensionSubjects =
            new Dictionary<string, FileExtensionMethodDelegate[]>
        {
            {"basic.d.str",     new FileExtensionMethodDelegate[]{Utils.IsFileDSTR, Utils.IsFileSTR}},
            {"basic.d.ts",      new FileExtensionMethodDelegate[]{Utils.IsFileDTS, Utils.IsFileTS}},
            {"basic.js",        new FileExtensionMethodDelegate[]{Utils.IsFileJS}},
            {"basic.str",       new FileExtensionMethodDelegate[]{Utils.IsFileSTR}},
            {"basic.ts",        new FileExtensionMethodDelegate[]{Utils.IsFileTS}},
        };

        [Test]
        public void IsFileExtension()
        {
            foreach (var subject in FileExtensionSubjects)
            {
                var trueMethods = subject.Value;
                var falseMethods = FileExtensionMethods.Except(trueMethods).ToArray();

                foreach (var m in trueMethods)
                {
                    Assert.IsTrue(m(subject.Key));
                }

                foreach (var m in falseMethods)
                {
                    Assert.IsFalse(m(subject.Key));
                }
            }
        }
    }
}
