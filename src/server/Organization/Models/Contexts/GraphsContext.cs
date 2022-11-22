using Microsoft.EntityFrameworkCore;

namespace Organization.Models.Contexts;

public class GraphsContext : DbContext
{
    public DbSet<Graph> Graphs { get; set; } = null!;

    public GraphsContext(DbContextOptions<GraphsContext> options) : base(options) { }
}