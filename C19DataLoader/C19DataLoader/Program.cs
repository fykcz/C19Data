using System;
using System.Diagnostics;
using System.IO;

using Fyk.Utils;

namespace Fyk.C19.C19DataLoader
{
    internal static class Program
    {
        public static readonly string Executable = new FileInfo(Process.GetCurrentProcess().MainModule.FileName).Name;

        private static void Main(string[] args)
        {
            var arguments = new Arguments(args);
            if ((arguments["help"] != null) || (arguments["?"] != null))
                HelpAndExit();
            if (arguments["config"] == null)
                HelpAndExit();

            var cfg = arguments["config"];

            if (Path.GetDirectoryName(cfg).Length == 0)
                cfg = Path.Combine(AppContext.BaseDirectory, cfg);
            if (!File.Exists(cfg))
            {
                Console.Error.WriteLine($"Config file {cfg} not found");
                Environment.Exit(1);
            }

            var x = new C19LoadData();
            var log = new ILogger();
            x.TransferData(new System.IO.StreamReader(cfg), log);

#if DEBUG
            if (!Console.IsOutputRedirected)
            {
                Console.Out.Write("Press Enter...");
                Console.ReadLine();
            }
#endif
        }
        private static void HelpAndExit()
        {
            Console.WriteLine($"{Executable} <options>");
            Console.WriteLine("\t--config\tPath to config file");
            Console.WriteLine("\t--help\t\tPrint this help");
            Environment.Exit(1);
        }

    }
}
