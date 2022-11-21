using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Organization.Models.Contexts;

namespace Organization.Models.Organization.Accounts;

[Table("user_groups")]
public class UserGroup : IUserAggregator
{
    #region Public
    #region Methods

    public bool AddAccount(Account account)
    {
        if (Contains(account.Id) is not null) { return false; }

        using var accountContext = DbContexts.GetNotNull<AccountContext>();
        accountContext.Accounts.Add(account);
        accountContext.SaveChanges();
        
        return true;
    }

    public Account? RemoveAccount(ulong id)
    {
        using var accountContext = DbContexts.GetNotNull<AccountContext>();

        var toRemove = accountContext.Accounts.FirstOrDefault(account => account.Id == id);
        if (toRemove is null) { return null; }
        
        accountContext.Accounts.Remove(toRemove);
        accountContext.SaveChanges();
        
        return toRemove;
    }

    public Account? Contains(ulong id)
    {
        using var accountContext = DbContexts.GetNotNull<AccountContext>();
        return accountContext.Accounts.FirstOrDefault(account => account.Id == id); 
    }
    
    /// <summary>
    /// Метод проверяет, содержитсся ли в хранилище пользователь с указанным логином
    /// </summary>
    /// <param name="login">Логин проверяемого аккаунта</param>
    /// <returns>Ссылка на объект, если он присутствует, иначе - null</returns>
    public Account? Contains(string login)
    {
        using var accountContext = DbContexts.GetNotNull<AccountContext>();
        return accountContext.Accounts.FirstOrDefault(account => account.Login == login);
    }
    
    #endregion

    #region Propertiew
    /// <summary>
    /// Уникальный идентификатор группы пользователей
    /// </summary>
    [Key, Column("global_id")]
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
    public IReadOnlyCollection<Account> Accounts { get; set; }
    
    /// <summary>
    /// Служебное поле, используется EF для настройки связи многие-ко-многим с сущностью UserGroup
    /// </summary>
    public List<UserGroupsAccounts> UserGroupsAccounts { get; set; }
    #endregion
    #endregion



    #region Private
    #region Fields
    #endregion
    #endregion
}