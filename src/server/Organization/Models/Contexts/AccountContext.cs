using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Contexts;

public class AccountContext: DbContext
{
    public DbSet<Account> Accounts { get; set; } = null!;
    
    public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }

}