using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Graphs;

namespace Organization.Models.Contexts;

public class GraphContext : DbContext
{
    public DbSet<GraphAggregator> Graphs { get; set; } = null!;

    public GraphContext(DbContextOptions<GraphContext> options) : base(options) { }
}