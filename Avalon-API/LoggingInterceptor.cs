using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class LoggingInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        Console.WriteLine($"EF query executed: {command.CommandText}");
        return base.ReaderExecuting(command, eventData, result);
    }
}