using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Contexts;

public class AccountContext: DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public AccountContext()
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=Test;Username=postgres;Password=au040403;");
    }
}