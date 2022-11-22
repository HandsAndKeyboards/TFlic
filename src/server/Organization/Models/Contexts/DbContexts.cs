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

    private static DbContextOptionsBuilder<T> CreateOptions<T>(string dbConnectionString) where T : DbContext => 
        new DbContextOptionsBuilder<T>().UseNpgsql(dbConnectionString);
}