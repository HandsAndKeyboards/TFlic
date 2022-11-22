using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Project;

namespace Organization.Models.Contexts;

public class ProjectContext: DbContext
{
    public DbSet<Project> Projects { get; set; } = null!;

    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }
}