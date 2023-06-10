using Microsoft.Extensions.Logging;
using System;
using System.IO;

public class FileLogger : ILogger
{
    private readonly string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public IDisposable? BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true; // Vous pouvez ajuster ce comportement selon votre besoin
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        string logMessage = formatter(state, exception);
        string logEntry = $"{DateTime.UtcNow} [{logLevel}] {logMessage}{Environment.NewLine}";

        File.AppendAllText(_filePath, logEntry);
    }
}
