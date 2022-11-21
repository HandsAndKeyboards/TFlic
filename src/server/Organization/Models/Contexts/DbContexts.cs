﻿using Microsoft.EntityFrameworkCore;

namespace Organization.Models.Contexts;

public static class DbContexts
{
    public static string DbConnectionString { get; set; } = string.Empty;
    
    public static TContext? Get<TContext>() where TContext : DbContext => 
        (TContext?) Activator.CreateInstance(typeof(TContext), CreateOptions<TContext>(DbConnectionString).Options);

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
    
    private static DbContextOptionsBuilder<T> CreateOptions<T>(string dbConnectionString) where T : DbContext => 
        new DbContextOptionsBuilder<T>().UseNpgsql(dbConnectionString);
}