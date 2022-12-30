using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Contexts;

public class UserGroupContext : DbContext
{
    public DbSet<UserGroup> UserGroups { get; set; } = null!;
    
    public UserGroupContext(DbContextOptions<UserGroupContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserGroup>()
            .HasMany(ug => ug.Accounts)
            .WithMany(acc => acc.UserGroups)
            .UsingEntity<UserGroupsAccounts>(
                intermediate => intermediate
                    .HasOne(uga => uga.Account)
                    .WithMany(acc => acc.UserGroupsAccounts)
                    .HasForeignKey(uga => uga.AccountId),
                intermediate => intermediate
                    .HasOne(uga => uga.UserGroup)
                    .WithMany(ug => ug.UserGroupsAccounts)
                    .HasForeignKey(uga => uga.UserGroupId),
                intermediate =>
                    intermediate.HasAlternateKey(uga => new { uga.AccountId, uga.UserGroupId })
            );
        base.OnModelCreating(modelBuilder);
    }
}