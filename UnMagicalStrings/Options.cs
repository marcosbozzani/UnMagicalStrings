using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnMagicalStrings
{
    public class Options
    {
        public string Source { get; private set; }
        public string Target { get; private set; }
        public string NameSpace { get; private set; }
        public string[] Include { get; private set; }
        public string[] Exclude { get; private set; }
        public string Tab { get; private set; }

        public Options()
        {
            Source = Environment.CurrentDirectory;
            Target = string.Empty;
            NameSpace = string.Empty;
            Include = new string[0];
            Exclude = new string[0];
            Tab = new string(' ', 4);
        }

        public static Options Parse(string commandLine)
        {
            if (commandLine == null)
            {
                throw new NullReferenceException("commandLine");
            }

            var args = SplitArguments(commandLine);
            var option = string.Empty;
            var options = new Options();
            var include = new List<string>();
            var exclude = new List<string>();

            foreach (var arg in args.Skip(1))
            {
                if (arg.StartsWith("-"))
                {
                    option = arg;
                }
                else
                {
                    if (option == "-t" || option == "-target" || option == string.Empty)
                    {
                        options.Target = arg;
                    }
                    else if (option == "-s" || option == "-source")
                    {
                        options.Source = arg;
                    }
                    else if (option == "-n" || option == "-namespace")
                    {
                        options.NameSpace = arg;
                    }
                    else if (option == "-i" || option == "-include")
                    {
                        include.Add(arg);
                    }
                    else if (option == "-e" || option == "-exclude")
                    {
                        exclude.Add(arg);
                    }
                }
            }

            options.Source = Path.GetFullPath(options.Source).PathAddBackslash();

            if (!Directory.Exists(options.Source))
            {
                throw new DirectoryNotFoundException(options.Source);
            }

            options.Include = include.Count == 0 ? new string[] { "*" } : include.ToArray();
            options.Exclude = exclude.ToArray();

            return options;
        }

        // http://stackoverflow.com/questions/298830/2132004#2132004
        private static string[] SplitArguments(string commandLine)
        {
            var parmChars = commandLine.ToCharArray();
            var inSingleQuote = false;
            var inDoubleQuote = false;

            for (var index = 0; index < parmChars.Length; index++)
            {
                if (parmChars[index] == '"' && !inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                    parmChars[index] = '\n';
                }

                if (parmChars[index] == '\'' && !inDoubleQuote)
                {
                    inSingleQuote = !inSingleQuote;
                    parmChars[index] = '\n';
                }

                if (!inSingleQuote && !inDoubleQuote && parmChars[index] == ' ')
                {
                    parmChars[index] = '\n';
                }
            }

            return new string(parmChars).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public class OptionsParseException : Exception
    {
        public OptionsParseException(string message) : base(message) { } 
    }
}
