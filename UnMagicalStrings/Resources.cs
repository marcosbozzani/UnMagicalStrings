using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.IO;
using System.Linq;

namespace UnMagicalStrings
{
    public class Resources
    {
        private readonly Options options;
        private readonly string className;
        
        public Resources(Options options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.options = options;

            className = options.Target == string.Empty ? "Resources" : Path.GetFileNameWithoutExtension(options.Target);
        }

        public int Create()
        {
            var tree = MakeTree(options.Source);

            using (var writer = new Writer(options))
            {
                if (!string.IsNullOrEmpty(options.NameSpace))
                {
                    writer.WriteLine("namespace " + options.NameSpace);
                    writer.WriteLine("{");
                    writer.Tab();
                }

                WalkTree(writer, tree, string.Empty, 0);

                if (!string.IsNullOrEmpty(options.NameSpace))
                {
                    writer.UnTab();
                    writer.WriteLine("}");
                }
            }

            return 0;
        }

        private Node MakeTree(string path)
        {
            var root = new Node(className, NodeType.Folder);

            foreach (var entry in Directory.EnumerateFileSystemEntries(path, "*.*", SearchOption.AllDirectories))
            {
                var relativePath = GetRelativePath(entry);

                if (Matches(relativePath, options.Include) && !Matches(relativePath, options.Exclude))
                {
                    var node = root;
                    var segments = relativePath.Split(Path.DirectorySeparatorChar);
                    var isFolder = File.GetAttributes(entry).HasFlag(FileAttributes.Directory);

                    for (int i = 0; i < segments.Length; i++ )
                    {
                        var type = NodeType.Folder;

                        if (i == segments.Length - 1 && !isFolder)
                        {
                            type = NodeType.File;
                        }

                        node = node.Add(segments[i], type);
                    }
                }
            }

            return root;
        }

        private void WalkTree(Writer writer, Node node, string path, int depth)
        {
            var identifier = CreateIdentifier(node.Name);

            if (node.Type == NodeType.Folder)
            {
                if (path != string.Empty)
                {
                    writer.WriteLine("public const string " + identifier + "Folder = @\"" + path + "\";");
                }

                writer.WriteLine("public partial class " + identifier);
                writer.WriteLine("{");
                writer.Tab();
            }
            else
            {
                writer.WriteLine("public const string " + identifier + " = @\"" + path + "\";");
            }

            foreach (var child in node.Children)
            {
                var childPath = path + child.Name;

                if (child.Type == NodeType.Folder)
                {
                    childPath += Path.DirectorySeparatorChar;
                }

                WalkTree(writer, child, childPath, depth + 1);
            }

            if (node.Type == NodeType.Folder)
            {
                writer.UnTab();
                writer.WriteLine("}");
            }
        }

        private bool Matches(string relativePath, string[] patterns)
        {
            foreach (var pattern in patterns)
            {
                // For Glob implementation see: https://github.com/SLaks/Minimatch
                if (Operators.LikeString(relativePath, pattern, CompareMethod.Binary))
                {
                    return true;
                }
            }

            return false;
        }

        private string GetRelativePath(string path)
        {
            return path.Substring(options.Source.Length);
        }

        private string CreateIdentifier(string name)
        {
            if (char.IsDigit(name[0]))
            {
                name = "_" + name;
            }

            return name.Replace(".", "_").Replace("-", "_");
        }
    }
}
