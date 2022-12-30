using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Project.Component;

namespace Organization.Models.Contexts;

public class ComponentContext : DbContext
{
    public DbSet<ComponentDto> Components { get; set; } = null!;
    
    public ComponentContext(DbContextOptions<ComponentContext> options) : base(options) { }
}