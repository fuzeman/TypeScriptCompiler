using System.Collections.Generic;
using System.Text;

namespace TypeScript.Compiler.IOHosts
{
    public class Node
    {
        public string Name { get; set; }
        public List<Node> Children { get; set; }
        public Node Parent { get; set; }

        public virtual bool IsDirectory { get { return false; } }
        public virtual bool IsFile { get { return false; } }
        public bool IsRoot { get { return !IsDirectory && !IsFile; } }

        public string GetFullName()
        {
            if (IsRoot)
                return "/";

            var stringBuilder = new StringBuilder(Name);

            var current = this;

            while (current != null)
            {
                current = current.Parent;

                if (current != null)
                {
                    if (current.IsRoot)
                        stringBuilder.Insert(0, "/");
                    else
                        stringBuilder.Insert(0, current.Name + "/");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
