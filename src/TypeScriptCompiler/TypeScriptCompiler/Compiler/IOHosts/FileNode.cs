namespace TypeScript.Compiler.IOHosts
{
    public class FileNode : Node
    {
        public override bool IsFile
        {
            get { return true; }
        }

        public string Content { get; private set; }

        public FileNode(string name)
        {
            Name = name;
        }

        public FileNode(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }
}
