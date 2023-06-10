using Microsoft.Build.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;

    public FileLoggerProvider(string filePath)
    {
        _filePath = filePath;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(_filePath);
    }

    public void Dispose()
    {
    }
}
