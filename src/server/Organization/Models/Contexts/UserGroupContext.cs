using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Contexts;

public class UserGroupContext : DbContext
{
    public DbSet<UserGroup> UserGroups { get; set; } = null!;
    
    public UserGroupContext(DbContextOptions<UserGroupContext> options) : base(options) { }
    
    // связь многие-ко-многим UserGroups - Accounts настраивается в AccountContext
}