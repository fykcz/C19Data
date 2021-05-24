using System;
using System.Collections.Generic;
using System.Text;

namespace Fyk.C19.C19DataLoader
{
    public class ILogger : IC19Logger
    {
        public void LogInformation(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now}\tINFO\t{message}");
        }
        public void LogError(string message)
        {
            Console.Error.WriteLine($"{DateTime.Now}\tERROR\t{message}");
        }

        void IC19Logger.LogWarning(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now}\tWARN\t{message}");
        }

        void IC19Logger.LogDebug(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now}\tDEBUG\t{message}");
        }

        void IC19Logger.LogCritical(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now}\tCRITICAL\t{message}");
        }

        void IC19Logger.LogTrace(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now}\tTRACE\t{message}");
        }
    }
}
