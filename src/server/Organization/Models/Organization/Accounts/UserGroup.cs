﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Organization.Models.Contexts;

namespace Organization.Models.Organization.Accounts;

[Table("user_groups")]
public class UserGroup
{
    #region Methods
    public bool AddAccount(Account account)
    {
        if (Contains(account.Id) is not null) { return false; }

        using var userGroupContext = DbContexts.Get<UserGroupContext>();
        var accounts = userGroupContext.UserGroups
            .Where(ug => ug.GlobalId == GlobalId)
            .Include(ug => ug.Accounts)
            .Single()
            .Accounts;
        
        accounts.Add(account);
        userGroupContext.SaveChanges();
        
        return true;
    }

    public Account? RemoveAccount(ulong id)
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        var toRemove = accountContext.Accounts.FirstOrDefault(account => account.Id == id);
        if (toRemove is null) { return null; }
        
        using var userGroupContext = DbContexts.Get<UserGroupContext>();
        var accounts = userGroupContext.UserGroups
            .Where(ug => ug.GlobalId == GlobalId)
            .Include(ug => ug.Accounts)
            .ToList()
            .Single()
            .Accounts;

        toRemove = accounts.FirstOrDefault(acc => acc.Id == id);
        if (toRemove is not null) accounts.Remove(toRemove);
        userGroupContext.SaveChanges();
        
        return toRemove;
    }

    public Account? Contains(ulong id)
    {
        using var userGroupContext = DbContexts.Get<UserGroupContext>();
        Account? account = null;

        try
        {
            account = userGroupContext.UserGroups
                .Where(ug => ug.GlobalId == GlobalId)
                .Include(ug => ug.Accounts)
                .Single()
                .Accounts
                .FirstOrDefault(acc => acc.Id == id);
        }
        catch (NullReferenceException)
        {
            // если аккаунт в базе данных не найден, возвращается Null, дополнительных действий не требуется
        }

        return account;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Уникальный идентификатор группы пользователей
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("global_id")]
    public ulong GlobalId { get; init; }
    
    /// <summary>
    /// Локальный идентификатор группы пользователей
    /// </summary>
    [Column("local_id")]
    public required short LocalId { get; init; }
    
    /// <summary>
    /// Уникальный идентификатор организации, которая содержит текущую группу пользователей
    /// </summary>
    [Column("organization_id")]
    public required ulong OrganizationId { get; set; }
    
    /// <summary> 
    /// Название группы пользователей
    /// </summary>
    [Column("name"), MaxLength(50)]
    public required string Name { get; set; }

    /// <summary>
    /// Участники группы пользователей
    /// </summary>
    public ICollection<Account> Accounts { get; set; } = null!;

    /// <summary>
    /// Служебное поле, используется EF для настройки связи многие-ко-многим с сущностью UserGroup
    /// </summary>
    public List<UserGroupsAccounts> UserGroupsAccounts { get; set; } = null!;
    #endregion
}