using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Project;

namespace Organization.Models.Contexts;

public class ColumnContext: DbContext
{
    public DbSet<Column> Columns { get; set; } = null!;
    
    public ColumnContext(DbContextOptions<ColumnContext> options) : base(options) { }

}