namespace Fyk.C19.C19DataLoader
{
    interface IC19Logger
    {
        public void LogInformation(string message);
        public void LogWarning(string message);
        public void LogError(string message);
        public void LogDebug(string message);
        public void LogCritical(string message);
        public void LogTrace(string message);
    }
}
