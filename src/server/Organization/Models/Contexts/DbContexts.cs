using Microsoft.EntityFrameworkCore;

namespace Organization.Models.Contexts;

public static class DbContexts
{
    public static string DbConnectionString { get; set; } = string.Empty;

    public static AccountContext AccountContext =>
        new (CreateOptions<AccountContext>(DbConnectionString).Options);
    public static UserGroupContext UserGroupContext =>
        new (CreateOptions<UserGroupContext>(DbConnectionString).Options);
    public static OrganizationContext OrganizationContext =>
        new (CreateOptions<OrganizationContext>(DbConnectionString).Options);
    public static ProjectContext ProjectContext =>
        new (CreateOptions<ProjectContext>(DbConnectionString).Options);
    public static BoardContext BoardContext =>
        new (CreateOptions<BoardContext>(DbConnectionString).Options);
    public static ColumnContext ColumnContext =>
        new (CreateOptions<ColumnContext>(DbConnectionString).Options);
    
    public static TaskContext TaskContext =>
        new (CreateOptions<TaskContext>(DbConnectionString).Options);
    
    public static ComponentContext ComponentContext =>
        new (CreateOptions<ComponentContext>(DbConnectionString).Options);
    
    private static DbContextOptionsBuilder<T> CreateOptions<T>(string dbConnectionString) where T : DbContext => 
        new DbContextOptionsBuilder<T>().UseNpgsql(dbConnectionString);
}