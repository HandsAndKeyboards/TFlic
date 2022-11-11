using Microsoft.EntityFrameworkCore;

namespace Organization.Models.EntityContexts.Organization.Account;

public class UserGroupContext : DbContext
{
    public DbSet<Models.Organization.Accounts.UserGroup> UserGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Program.AppConfiguration?.GetConnectionString("DbConnectionString"));
    }
}