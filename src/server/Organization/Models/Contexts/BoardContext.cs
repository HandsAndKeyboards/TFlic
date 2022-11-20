using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Project;

namespace Organization.Models.Contexts;

public class BoardContext: DbContext
{
    public DbSet<Board> Boards { get; set; } = null!;
    
    public BoardContext(DbContextOptions<BoardContext> options) : base(options) { }

}