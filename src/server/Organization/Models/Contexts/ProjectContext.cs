using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Accounts;
using Organization.Models.Organization.Project;

namespace Organization.Models.Contexts;

public class ProjectContext: DbContext
{
    public DbSet<Project> Projects { get; set; }
    public ProjectContext()
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=Test;Username=postgres;Password=au040403;");
    }
}