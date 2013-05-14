namespace TypeScript.Compiler.IOHosts
{
    public class DirectoryNode : Node
    {
        public override bool IsDirectory
        {
            get { return true; }
        }

        public DirectoryNode(string name)
        {
            Name = name;
        }
    }
}
