using Microsoft.EntityFrameworkCore;
using Task = Organization.Models.Organization.Project.Task;

namespace Organization.Models.Contexts;

public class TaskContext: DbContext
{
    public DbSet<Task> Tasks { get; set; } = null!;
    
    public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

}