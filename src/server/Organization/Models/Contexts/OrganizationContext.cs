using Microsoft.EntityFrameworkCore;

namespace Organization.Models.Contexts;

public class OrganizationContext : DbContext
{
    public DbSet<Organization.Organization> Organizations { get; set; } = null!;
    
    public OrganizationContext(DbContextOptions<OrganizationContext> options) : base(options) { }
}