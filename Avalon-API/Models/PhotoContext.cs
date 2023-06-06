using Microsoft.EntityFrameworkCore;

namespace Avalon_API.Models;

public class PhotoContext : DbContext
{
    public PhotoContext(DbContextOptions<PhotoContext> options)
        : base(options)
    {
    }

    public DbSet<PhotoItem> PhotoItems { get; set; } = null!;
}