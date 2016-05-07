using System;
using System.IO;

namespace UnMagicalStrings
{
    public class Writer : IDisposable
    {
        private string tabs;
        private StreamWriter streamWriter;
        private readonly Options options;

        public Writer(Options options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.options = options;

            if (options.Target != string.Empty)
            {
                streamWriter = new StreamWriter(options.Target, false);
            }
            else
            {
                streamWriter = new StreamWriter(Console.OpenStandardOutput());
                streamWriter.AutoFlush = true;
            }
        }

        public void Tab()
        {
            tabs += options.Tab;
        }

        public void UnTab()
        {
            if (tabs.Length - options.Tab.Length >= 0)
            {
                tabs = tabs.Substring(0, tabs.Length - options.Tab.Length);
            }
        }

        public void Write(string value)
        {
            streamWriter.Write(tabs);
            streamWriter.Write(value);
        }

        public void WriteWithoutTabs(string value)
        {
            streamWriter.Write(value);
        }

        public void WriteLine(string value = null)
        {
            if (value != null)
            {
                streamWriter.Write(tabs);
                streamWriter.WriteLine(value);
            }
            else
            {
                streamWriter.WriteLine();
            }
        }

        public void Dispose()
        {
            if (streamWriter != null)
            {
                streamWriter.Dispose();
                streamWriter = null;
            }
        }
    }
}
