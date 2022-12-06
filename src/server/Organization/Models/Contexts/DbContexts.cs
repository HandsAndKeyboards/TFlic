using Microsoft.EntityFrameworkCore;

namespace Organization.Models.Contexts;

public static class DbContexts
{
    /// <summary>
    /// Создает и возвращает контекст базы данных
    /// </summary>
    /// <typeparam name="TContext">Тип контекста базы данных</typeparam>
    /// <returns>Созданный и сконфигурированный контекст базы данных</returns>
    /// <exception cref="NullReferenceException">Генерируется, если не удалось создать контекст данных</exception>
    public static TContext Get<TContext>() where TContext : DbContext
    {
        var context = (TContext?) Activator.CreateInstance(
            typeof(TContext),
            CreateOptions<TContext>(DbConnectionString).Options
        );
        if (context is null) { throw new NullReferenceException($"Не удалось создать экземпляр класса {nameof(TContext)}"); }
    
        return context;
    }

    public static string DbConnectionString { get; set; } = string.Empty;

    private static DbContextOptionsBuilder<T> CreateOptions<T>(string dbConnectionString) where T : DbContext => 
        new DbContextOptionsBuilder<T>().UseNpgsql(dbConnectionString);
}