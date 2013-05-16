using System;
using System.Diagnostics;
using System.IO;
using FluentAssertions;
using TypeScript.Compiler;
using TypeScript.Compiler.Data;
using TypeScript.Compiler.IOHosts;
using NUnit.Framework;

namespace Tests.Compiler.IOHosts
{
    [TestFixture]
    public class WindowsIOHostTests
    {
        private string _tempPath;

        #region Core

        [SetUp]
        public void Setup()
        {
            _tempPath = Path.Combine(Path.GetTempPath(), "TypeScriptCompiler",
                "Tests", Guid.NewGuid().ToString("N"), "WindowsIOHostTests");

            Directory.CreateDirectory(_tempPath);

            SimpleSetup();
            CompileWithDuplicateReferencesSetup();
        }

        [TearDown]
        public void TearDown()
        {
            SimpleTearDown();
            CompileWithDuplicateReferencesTearDown();

            try
            {
                Directory.Delete(_tempPath);
                Directory.Delete(Path.Combine(_tempPath, ".."));
            }
            catch (IOException ex)
            {
                Debug.WriteLine("Exception while trying to delete temporary test directory at {0}, Exception: {1}",
                    _tempPath, ex.Message);
            }
        }

        #endregion

        #region Simple Test

        private const string Simple_DirectoryName = "Simple";
        private string Simple_DirectoryPath;

        private void SimpleSetup()
        {
            // Create test directory
            Simple_DirectoryPath = Path.Combine(_tempPath, Simple_DirectoryName);
            Directory.CreateDirectory(Simple_DirectoryPath);
            var testPath = Simple_DirectoryPath;

            // Create Test Files
            File.WriteAllText(Path.Combine(testPath, "cuatro.ts"), "cuatro");

            Directory.CreateDirectory(Path.Combine(testPath, "uno"));
            File.WriteAllText(Path.Combine(testPath, "uno", "tres.ts"), "tres");
            File.WriteAllText(Path.Combine(testPath, "uno", "dos.ts"), "dos");
        }

        [Test]
        public void Simple()
        {
            var testPath = Simple_DirectoryPath;
            var windowsHost = new WindowsIOHost(testPath);

            // Resolve Path
            windowsHost.ResolvePath("cuatro.ts")
                .Should().Be(Path.Combine(testPath, "cuatro.ts"));

            windowsHost.ResolvePath(Path.Combine(testPath, "cuatro.ts"))
                .Should().Be(Path.Combine(testPath, "cuatro.ts"));

            windowsHost.ResolvePath(Path.Combine(testPath, "uno", "tres.ts"))
                .Should().Be(Path.Combine(testPath, "uno", "tres.ts"));

            windowsHost.ResolvePath("uno/dos.ts")
                .Should().Be(Path.Combine(testPath, "uno", "dos.ts"));

            // Directory Name
            windowsHost.DirectoryName("cuatro.ts").Should().Be(testPath);
            windowsHost.DirectoryName(Path.Combine(testPath, "cuatro.ts")).Should().Be(testPath);
            windowsHost.DirectoryName(Path.Combine(testPath, "uno", "tres.ts")).Should().Be(Path.Combine(testPath, "uno"));
            windowsHost.DirectoryName("uno/dos.ts").Should().Be(Path.Combine(testPath, "uno"));

            // Read File
            windowsHost.ReadFile(Path.Combine(testPath, "uno", "dos.ts")).Should().Be("dos");
            windowsHost.ReadFile("uno/dos.ts").Should().Be("dos");
            windowsHost.ReadFile(Path.Combine(testPath, "uno", "tres.ts")).Should().Be("tres");
            windowsHost.ReadFile("uno/tres.ts").Should().Be("tres");
            windowsHost.ReadFile(Path.Combine(testPath, "cuatro.ts")).Should().Be("cuatro");
            windowsHost.ReadFile("cuatro.ts").Should().Be("cuatro");

            // Relative Paths
            windowsHost.ResolvePath(Path.Combine(testPath, "uno", "..", "cuatro.ts")).Should().Be(Path.Combine(testPath, "cuatro.ts"));
            windowsHost.ResolvePath(Path.Combine("uno", "..", "cuatro.ts")).Should().Be(Path.Combine(testPath, "cuatro.ts"));
            windowsHost.ResolvePath(Path.Combine(testPath, "uno", "..", "uno", "dos.ts")).Should().Be(Path.Combine(testPath, "uno", "dos.ts"));
        }

        private void SimpleTearDown()
        {
            var testPath = Simple_DirectoryPath;

            // Delete Files
            if (File.Exists(Path.Combine(testPath, "cuatro.ts")))
                File.Delete(Path.Combine(testPath, "cuatro.ts"));

            if (File.Exists(Path.Combine(testPath, "uno", "tres.ts")))
                File.Delete(Path.Combine(testPath, "uno", "tres.ts"));

            if (File.Exists(Path.Combine(testPath, "uno", "dos.ts")))
                File.Delete(Path.Combine(testPath, "uno", "dos.ts"));

            // Delete "uno" directory
            if (Directory.Exists(Path.Combine(testPath, "uno")))
                Directory.Delete(Path.Combine(testPath, "uno"));

            // Delete "Simple" test directory
            if (Directory.Exists(testPath))
                Directory.Delete(testPath);
        }

        #endregion

        #region CompileWithDuplicateReferences

        private const string CompileWithDuplicateReferences_DirectoryName = "CompileWithDuplicateReferences";
        private string CompileWithDuplicateReferences_DirectoryPath;

        private void CompileWithDuplicateReferencesSetup()
        {
            // Create test directory
            CompileWithDuplicateReferences_DirectoryPath = Path.Combine(_tempPath, CompileWithDuplicateReferences_DirectoryName);
            var testPath = CompileWithDuplicateReferences_DirectoryPath;
            Directory.CreateDirectory(testPath);

            // Create Test Files
            File.WriteAllText(Path.Combine(testPath, "utils.ts"), 
                "var utils = 1252;");

            File.WriteAllText(Path.Combine(testPath, "two.ts"),
                "///<reference path='utils.ts' />\n" +
                "///<reference path='things/one.ts' />\n" +
                "var two = 3126;");

            Directory.CreateDirectory(Path.Combine(testPath, "things"));
            File.WriteAllText(Path.Combine(testPath, "things", "one.ts"),
                "///<reference path='../utils.ts' />\n" +
                "var one = 1244;");
        }

        [Test]
        public void CompileWithDuplicateReferences()
        {
            var windowsHost = new WindowsIOHost(CompileWithDuplicateReferences_DirectoryPath);
            var batchCompiler = new BatchCompiler(windowsHost);

            batchCompiler.CompilationEnvironment.Code.Add(new SourceUnit("two.ts"));

            batchCompiler.Run();
        }

        private void CompileWithDuplicateReferencesTearDown()
        {
            var testPath = CompileWithDuplicateReferences_DirectoryPath;

            if (File.Exists(Path.Combine(testPath, "utils.ts")))
                File.Delete(Path.Combine(testPath, "utils.ts"));

            if (File.Exists(Path.Combine(testPath, "two.ts")))
                File.Delete(Path.Combine(testPath, "two.ts"));

            if (File.Exists(Path.Combine(testPath, "things", "one.ts")))
                File.Delete(Path.Combine(testPath, "things", "one.ts"));

            if(Directory.Exists(Path.Combine(testPath, "things")))
                Directory.Delete(Path.Combine(testPath, "things"));

            if (Directory.Exists(testPath))
                Directory.Delete(testPath);
        }

        #endregion
    }
}
