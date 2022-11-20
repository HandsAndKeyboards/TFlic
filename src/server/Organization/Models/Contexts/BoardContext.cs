using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Accounts;
using Organization.Models.Organization.Project;

namespace Organization.Models.Contexts;

public class BoardContext: DbContext
{
    public DbSet<Board> Boards { get; set; }
    public BoardContext()
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=Test;Username=postgres;Password=au040403;");
    }
}