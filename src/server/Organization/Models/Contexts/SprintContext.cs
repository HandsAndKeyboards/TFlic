using Microsoft.EntityFrameworkCore;
using Organization.Models.Organization.Project;

namespace Organization.Models.Contexts
{
    public class SprintContext : DbContext
    {
        public DbSet<Sprint> Sprints { get; set; } = null!;

        public SprintContext(DbContextOptions<SprintContext> options) : base(options) { }
    }
}
