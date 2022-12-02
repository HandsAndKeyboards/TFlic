using Microsoft.EntityFrameworkCore;
using Organization.Models.Authentication;

namespace Organization.Models.Contexts;

public class AuthInfoContext: DbContext
{
    public DbSet<AuthInfo> Info { get; set; } = null!;
    
    public AuthInfoContext(DbContextOptions<AuthInfoContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthInfo>()
            .HasOne(info => info.Account)
            .WithOne(acc => acc.AuthInfo);
        
        base.OnModelCreating(modelBuilder);
    }
}