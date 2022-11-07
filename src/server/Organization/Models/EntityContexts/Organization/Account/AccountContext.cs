using Microsoft.EntityFrameworkCore;

namespace Organization.Models.EntityContexts.Organization.Account;

public class AccountContext : DbContext
{
    public DbSet<Models.Organization.Accounts.Account> Accounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Program.AppConfiguration?.GetConnectionString("DbConnectionString"));
    }
}