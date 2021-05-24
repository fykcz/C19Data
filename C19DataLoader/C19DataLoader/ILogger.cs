using System;
using System.Collections.Generic;
using System.Text;

namespace Fyk.C19.C19DataLoader
{
    public class ILogger : IC19Logger
    {
        public void LogInformation(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\tINFO\t{message}");
        }
        public void LogError(string message)
        {
            Console.Error.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\tERROR\t{message}");
        }

        void IC19Logger.LogWarning(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\tWARN\t{message}");
        }

        void IC19Logger.LogDebug(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\tDEBUG\t{message}");
        }

        void IC19Logger.LogCritical(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\tCRITICAL\t{message}");
        }

        void IC19Logger.LogTrace(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\tTRACE\t{message}");
        }
    }
}
