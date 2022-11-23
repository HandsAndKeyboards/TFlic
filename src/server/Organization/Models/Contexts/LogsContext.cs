using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Project;

namespace Organization.Models.Contexts;

public class LogContext : DbContext
{
    public DbSet<Logs> Logs { get; set; } = null!;

    public LogContext(DbContextOptions<LogContext> options) : base(options) { }
}