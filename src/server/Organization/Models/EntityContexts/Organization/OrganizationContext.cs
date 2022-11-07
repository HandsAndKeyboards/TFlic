using Microsoft.EntityFrameworkCore;

namespace Organization.Models.EntityContexts.Organization;

public class OrganizationContext : DbContext
{
    public DbSet<Models.Organization.Organization> Organizations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Program.AppConfiguration?.GetConnectionString("DbConnectionString"));
    }
}