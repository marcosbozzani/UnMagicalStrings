using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnMagicalStrings
{
    public enum NodeType
    { 
        File, Folder
    }

    public class Node
    {
        private readonly string name;
        private readonly NodeType type;
        private readonly Node parent;
        private readonly Dictionary<string, Node> lookup;

        public string Name { get { return name; } }
        public NodeType Type { get { return type; } }
        public Node Parent { get { return parent; } }
        public IEnumerable<Node> Children { get { return lookup.Values; } }

        public Node(string name, NodeType type)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
            this.type = type;
            this.parent = this;
            this.lookup = new Dictionary<string, Node>();
        }

        public Node Add(string name, NodeType type)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            var node = Get(name);
         
            if (node == null)
            {
                node = new Node(name, type);
                lookup[name] = node;
            }

            return node;
        }

        public Node Get(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Node node;

            if (lookup.TryGetValue(name, out node))
            {
                return node;
            }

            return null;
        }

        public bool Remove(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            return lookup.Remove(name);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
