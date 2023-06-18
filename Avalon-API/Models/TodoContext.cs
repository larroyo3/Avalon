using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Avalon_API.Models;

public class TodoContext : DbContext
{

    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}