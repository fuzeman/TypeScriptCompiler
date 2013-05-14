using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TypeScript.Compiler.IOHosts;

namespace Tests.Compiler.IOHosts
{
    [TestFixture]
    public class MemoryIOHostTests
    {
        [Test]
        public void Simple()
        {
            var memoryHost = new MemoryIOHost(new Node
            {
                Children = new List<Node>
                {
                    new DirectoryNode("uno")
                    {
                        Children = new List<Node>
                        {
                            new FileNode("dos.ts", "dos"),
                            new FileNode("tres.ts", "tres")
                        }
                    },
                    new FileNode("cuatro.ts", "cuatro")
                }
            });

            // Resolve Path
            memoryHost.ResolvePath("cuatro.ts").Should().Be("/cuatro.ts");
            memoryHost.ResolvePath("/cuatro.ts").Should().Be("/cuatro.ts");
            memoryHost.ResolvePath("/uno/tres.ts").Should().Be("/uno/tres.ts");
            memoryHost.ResolvePath("uno/dos.ts").Should().Be("/uno/dos.ts");

            // Directory Name
            memoryHost.DirectoryName("cuatro.ts").Should().Be("/");
            memoryHost.DirectoryName("/cuatro.ts").Should().Be("/");
            memoryHost.DirectoryName("/uno/tres.ts").Should().Be("/uno");
            memoryHost.DirectoryName("uno/dos.ts").Should().Be("/uno");

            // Read File
            memoryHost.ReadFile("/uno/dos.ts").Should().Be("dos");
            memoryHost.ReadFile("uno/dos.ts").Should().Be("dos");
            memoryHost.ReadFile("/uno/tres.ts").Should().Be("tres");
            memoryHost.ReadFile("uno/tres.ts").Should().Be("tres");
            memoryHost.ReadFile("/cuatro.ts").Should().Be("cuatro");
            memoryHost.ReadFile("cuatro.ts").Should().Be("cuatro");

            // Relative Paths
            memoryHost.ResolvePath("/uno/../cuatro.ts").Should().Be("/cuatro.ts");
            memoryHost.ResolvePath("uno/../cuatro.ts").Should().Be("/cuatro.ts");
            memoryHost.ResolvePath("/uno/../uno/dos.ts").Should().Be("/uno/dos.ts");
        }
    }
}
