namespace TypeScript.Compiler
{
    public interface IIOHost
    {
        string ResolvePath(string path);
        string DirectoryName(string path);
        bool IsRelative(string path);
        bool IsAbsolute(string path);

        bool IsDirectory(string path);
        bool IsFile(string path);

        string ReadFile(string path);
    }
}
