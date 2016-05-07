using System;

namespace UnMagicalStrings
{
    class Program
    {
        static int Main()
        {
            var exitCode = 0;
            
            try
            { 
                var options = Options.Parse(Environment.CommandLine);

                exitCode = new Resources(options).Create();
#if DEBUG
                if (options.Target == string.Empty)
                {
                    Console.ReadKey();
                }
#endif
            }
            catch (OptionsParseException ex)
            {
                Console.WriteLine(ex.Message);
                exitCode = -1;
            }

            return exitCode;
        }
    }
}
