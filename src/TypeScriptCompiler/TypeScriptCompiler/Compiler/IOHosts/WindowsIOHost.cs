using System.IO;
using System.Linq;

namespace TypeScript.Compiler.IOHosts
{
    public class WindowsIOHost : IIOHost
    {
        public string BasePath { get; private set; }

        public WindowsIOHost(string basePath)
        {
            BasePath = basePath;
        }

        public string ResolvePath(string path)
        {
            if (IsAbsolute(path))
                return path;

            path = Path.Combine(BasePath, path);
            return Path.GetFullPath(path);
        }

        public string DirectoryName(string path)
        {
            return Path.GetDirectoryName(ResolvePath(path));
        }

        public bool IsRelative(string path)
        {
            return !IsAbsolute(path);
        }

        public bool IsAbsolute(string path)
        {
            return Path.IsPathRooted(path) &&
                path.Contains(Path.VolumeSeparatorChar.ToString() + Path.DirectorySeparatorChar) &&
                !path.Split(Path.DirectorySeparatorChar).Contains("..");
        }

        public bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public bool IsFile(string path)
        {
            return File.Exists(path);
        }

        public string ReadFile(string path)
        {
            return File.ReadAllText(ResolvePath(path));
        }
    }
}
