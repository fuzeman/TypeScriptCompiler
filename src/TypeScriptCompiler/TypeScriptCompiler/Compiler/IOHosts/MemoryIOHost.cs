using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TypeScript.Compiler.IOHosts
{
    public class MemoryIOHost : IIOHost
    {
        public Node Root { get; private set; }
        public Node Current { get; set; }

        #region Constructor

        public MemoryIOHost()
        {
            Root = new Node();
            Current = Root;
        }

        public MemoryIOHost(Node root)
        {
            Root = root;
            Current = root;

            FinishTree();
        }

        public MemoryIOHost(Node root, Node current)
        {
            Root = root;
            Current = current;

            FinishTree();
        }

        private void FinishTree()
        {
            FinishTree(Current, null);
        }

        private void FinishTree(Node current, Node parent)
        {
            current.Parent = parent;

            if (current.Children == null) return;

            foreach (var child in current.Children)
            {
                FinishTree(child, current);
            }
        }

        #endregion

        #region FindNode

        private Node FindNode(string path)
        {
            return FindNode(path.Split('/'));
        }

        private Node FindNode(IEnumerable<string> names)
        {
            var nameQueue = new Queue<string>(names);
            var current = Current;

            while (nameQueue.Count > 0)
            {
                var name = nameQueue.Dequeue();
                if (name != "")
                {
                    current = name == ".." ? current.Parent :
                        current.Children.FirstOrDefault(n => n.Name == name);

                    if (current == null)
                        break;
                }
            }

            return current;
        }

        #endregion FindNode

        public bool IsRelative(string path)
        {
            return path[0] != '/';
        }

        public bool IsAbsolute(string path)
        {
            return !IsRelative(path);
        }

        public string ResolvePath(string path)
        {
            var node = FindNode(path);
            if (node == null)
                throw new FileNotFoundException();

            return node.GetFullName();
        }

        public string DirectoryName(string path)
        {
            var node = FindNode(path);
            if(node == null)
                throw new FileNotFoundException(string.Format("\"{0}\" is an invalid path", path));

            if (node.IsDirectory)
                return node.GetFullName();

            if (node.Parent.IsFile)
                throw new Exception();

            return node.Parent.GetFullName();
        }

        public string ReadFile(string path)
        {
            var node = FindNode(path);
            if(node == null || !node.IsFile)
                throw new FileNotFoundException();

            return ((FileNode) node).Content;
        }
    }
}
