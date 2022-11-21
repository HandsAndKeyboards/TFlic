using Microsoft.EntityFrameworkCore;

namespace Organization.Models.Contexts;

public static class DbContexts
{
    public static TContext GetNotNull<TContext>() where TContext : DbContext
    {
        var context = (TContext?) Activator.CreateInstance(
            typeof(TContext),
            CreateOptions<TContext>(DbConnectionString).Options
        );
        if (context is null) { throw new NullReferenceException($"Не удалось создать экземпляр класса {nameof(TContext)}"); }
    
        return context;
    }
    
    public static TContext? Get<TContext>() where TContext : DbContext =>
        (TContext?) Activator.CreateInstance(typeof(TContext), CreateOptions<TContext>(DbConnectionString).Options);
    
    public static string DbConnectionString { get; set; } = string.Empty;

    [Obsolete("В скором времени свойство будет удалено")]
    public static AccountContext AccountContext =>
        new (CreateOptions<AccountContext>(DbConnectionString).Options);
    
    [Obsolete("В скором времени свойство будет удалено")]
    public static UserGroupContext UserGroupContext =>
        new (CreateOptions<UserGroupContext>(DbConnectionString).Options);
    
    [Obsolete("В скором времени свойство будет удалено")]
    public static OrganizationContext OrganizationContext =>
        new (CreateOptions<OrganizationContext>(DbConnectionString).Options);
    
    [Obsolete("В скором времени свойство будет удалено")]
    public static ProjectContext ProjectContext =>
        new (CreateOptions<ProjectContext>(DbConnectionString).Options);
    
    [Obsolete("В скором времени свойство будет удалено")]
    public static BoardContext BoardContext =>
        new (CreateOptions<BoardContext>(DbConnectionString).Options);
    
    [Obsolete("В скором времени свойство будет удалено")]
    public static ColumnContext ColumnContext =>
        new (CreateOptions<ColumnContext>(DbConnectionString).Options);
    
    [Obsolete("В скором времени свойство будет удалено")]
    public static TaskContext TaskContext =>
        new (CreateOptions<TaskContext>(DbConnectionString).Options);
    
    [Obsolete("В скором времени свойство будет удалено")]
    public static ComponentContext ComponentContext =>
        new (CreateOptions<ComponentContext>(DbConnectionString).Options);
    
    private static DbContextOptionsBuilder<T> CreateOptions<T>(string dbConnectionString) where T : DbContext => 
        new DbContextOptionsBuilder<T>().UseNpgsql(dbConnectionString);
}