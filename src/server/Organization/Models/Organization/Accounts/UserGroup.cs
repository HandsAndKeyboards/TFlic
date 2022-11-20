using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organization.Models.Organization.Accounts;

public class UserGroup : IUserAggregator
{
    #region Public
    #region Methods

    public bool AddAccount(Account account)
    {
        if (Contains(account)) { return false; }
        
        _accounts.Add(account);
        return true;
    }
    
    public bool RemoveAccount(Account account)
    {
        return _accounts.Remove(account);
    }
    
    public Account? RemoveAccount(ulong id)
    {
        var toRemove = _accounts.Find(account => account.Id == id);
        if (toRemove is not null) { _accounts.Remove(toRemove); }
        
        return toRemove;
    }
    
    public bool Contains(Account account)
    {
        return _accounts.Contains(account);
    }
    
    public Account? Contains(ulong id)
    {
        return _accounts.Find(account => account.Id == id); 
    }
    
    #endregion

    #region Propertiew
    /// <summary>
    /// Уникальный идентификатор группы пользователей
    /// </summary>
    [Column("global_id"), Key]
    public ulong GlobalId { get; init; }
    
    /// <summary>
    /// Локальный идентификатор группы пользователей
    /// </summary>
    [Column("local_id")]
    public required short LocalId { get; init; }
    
    /// <summary>
    /// Название группы пользователей
    /// </summary>
    [Column("organization_id")]
    public required string Name { get; set; }

    /// <summary>
    /// Участники группы пользователей
    /// </summary>
    public IReadOnlyCollection<Account> Accounts
    {
        get => _accounts; 
        init => _accounts = (List<Account>) value;
    }

    #endregion
    #endregion



    #region Private
    #region Fields

    private readonly List<Account> _accounts = new();

    #endregion
    #endregion
}