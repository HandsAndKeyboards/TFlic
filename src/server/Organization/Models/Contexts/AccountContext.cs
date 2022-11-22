using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Contexts;

public class AccountContext: DbContext
{
    public DbSet<Account> Accounts { get; set; } = null!;
    
    public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasMany(acc => acc.UserGroups)
            .WithMany(ug => ug.Accounts)
            .UsingEntity<UserGroupsAccounts>(
                intermediate => intermediate
                    .HasOne(uga => uga.UserGroup)
                    .WithMany(ug => ug.UserGroupsAccounts)
                    .HasForeignKey(uga => uga.UserGroupId),
                intermediate => intermediate
                    .HasOne(uga => uga.Account)
                    .WithMany(acc => acc.UserGroupsAccounts)
                    .HasForeignKey(uga => uga.AccountId),
                intermediate =>
                    intermediate.HasAlternateKey(uga => new { uga.AccountId, uga.UserGroupId })
            );
        base.OnModelCreating(modelBuilder);
    }
}