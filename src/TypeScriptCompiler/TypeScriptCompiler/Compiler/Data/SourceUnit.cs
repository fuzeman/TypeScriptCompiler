using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace TypeScript.Compiler.Data
{
    public class SourceUnit
    {
        public string Path { get; set; }
        public string Content { get; set; }
        public List<FileReference> ReferencedFiles { get; set; }

        public SourceUnit(string path, string content)
        {
            Path = path;
            Content = content;
            ReferencedFiles = new List<FileReference>();
        }

        public SourceUnit(string path)
        {
            Path = path;
            Content = null;
            ReferencedFiles = new List<FileReference>();
        }

        public Dictionary<string, object> ToJavascriptObject()
        {
            return new Dictionary<string, object>
            {
                {"path", Path},
                {"content", Content},
            };
        }
    }
}
