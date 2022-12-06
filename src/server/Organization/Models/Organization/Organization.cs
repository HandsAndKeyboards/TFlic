using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Organization.Models.Contexts;
using Organization.Models.Organization.Accounts;
using ModelProject = Organization.Models.Organization.Project.Project;

namespace Organization.Models.Organization;

[Table("organizations")]
public class Organization
{
    #region Public
    /// <summary>
    /// Основные группы пользоватлелей
    /// </summary>
    public enum PrimaryUserGroups { NoRole = 0, ProjectsMembers = 2, Admins = 4 }
    
    #region Methods
    /// <summary>
    /// Метод добавляет аккаунт в группу пользователей "Пользователи без роли" 
    /// </summary>
    /// <param name="account">Добавляемый аккаунт</param>
    /// <returns>true в случае успешного добавления, иначе - false</returns>
    /// <exception cref="OrganizationException">
    /// Исключение вазникает в случае отсутствия в организации группы пользователей "Пользователи без роли"
    /// </exception>
    public bool AddAccount(Account account)
    {
        if (Contains(account.Id) is not null) { return false; }
        
        var noRole = GetUserGroups()
            .FirstOrDefault(group => group.OrganizationId == Id && group.LocalId == (short) PrimaryUserGroups.NoRole);
        if (noRole is null) { throw new OrganizationException("Не найдена группа пользователей 'Пользователи без роли'"); }

        noRole.AddAccount(account);

        return true;
    }

    /// <summary>
    /// Метод добавляет аккаунт с указанным уникальным идентификатором в организацию
    /// </summary>
    /// <param name="accountId">Уникальный идентификатор добавляемого аккаунта</param>
    /// <returns>true в случае успешного добавления, иначе - false</returns>
    public bool AddAccount(ulong accountId)
    {
        if (Contains(accountId) is not null) { return false; }

        using var accountContext = DbContexts.Get<AccountContext>();
        var account = accountContext.Accounts.Single(acc => acc.Id == accountId);
        
        AddAccount(account);
        return true;
    }
    
    /// <summary>
    /// Метод удаляет аккаунт с уаазаннм уникальным идентификатором из всех групп пользователей организации
    /// </summary>
    /// <param name="id">Уникальный идентификатор удаляемого аккаунта</param>
    /// <returns>Ссылка на удаляемый объект в случае успешного удаления, иначе - null</returns>
    public Account? RemoveAccount(ulong id)
    {
        var toRemove = Contains(id);
        if (toRemove is not null)
        {
            foreach (var userGroup in GetUserGroups())
            {
                userGroup.RemoveAccount(toRemove.Id);
            }
        }
        
        return toRemove;
    }
    
    /// <summary>
    /// Метод проверяет, содержит ли любая из групп пользователей
    /// организации пользователя с указанным идентификатором
    /// </summary>
    /// <param name="id">Уникальный идентификатор проверяемого пользователя</param>
    /// <returns>ссылка на объект, если он присутствует, иначе - null </returns>
    public Account? Contains(ulong id)
    {
        Account? account = null;
        foreach (var userGroup in GetUserGroups())
        {
            account = userGroup.Contains(id);
            if (account is not null) { break; }
        }
        
        return account; 
    }

    /// <summary>
    /// <list type="bullet">
    ///     <listheader>Метод создает основные группы пользователей организации:</listheader>
    ///     <item>Пользователи без роли</item>
    ///     <item>Администраторы организации</item>
    ///     <item>Участники проектов</item>
    /// </list>
    /// </summary>
    public void CreateUserGroups()
    {
        using var userGroupContext = DbContexts.Get<UserGroupContext>();
        
        userGroupContext.Add(new UserGroup {LocalId = (short) PrimaryUserGroups.NoRole, OrganizationId = Id,  Name = "Пользователи без роли"});
        userGroupContext.Add(new UserGroup {LocalId = (short) PrimaryUserGroups.ProjectsMembers, OrganizationId = Id, Name = "Участники проектов"});
        userGroupContext.Add(new UserGroup {LocalId = (short) PrimaryUserGroups.Admins, OrganizationId = Id, Name = "Администраторы"});

        userGroupContext.SaveChanges();
    }
    
    /// <summary>
    /// Группы пользователй организации
    /// </summary>
    public ICollection<UserGroup> GetUserGroups() 
    {
        using var userGroupContext = DbContexts.Get<UserGroupContext>();
        var userGroups = userGroupContext.UserGroups
            .Where(ug => ug.OrganizationId == Id)
            .Include(ug => ug.Accounts)
            .ToList();

        return userGroups;
    }
    
    /// <summary>
    /// Метод добавляет указанного пользователя в группу пользователей
    /// с указанным идентификатором группы в организации
    /// </summary>
    /// <param name="account">Добавляемый аккаунт</param>
    /// <param name="groupLocalId">Идентификатор группы пользователей в организации</param>
    /// <exception cref="OrganizationException">
    /// <list type="bullet">
    ///     <listheader>Возникает в случаях:</listheader>
    ///     <item>Организация не содержит указанный аккаунт</item>
    ///     <item>Организация не содержит группу пользователей с указанным локальным Id</item>
    ///     <item>Организация не содержит группу пользователей "Пользователи без роли"</item>
    /// </list>
    /// </exception>
    public void AddAccountToGroup(Account account, short groupLocalId)
    {
        if (Contains(account.Id) is null) { throw new OrganizationException($"Организация не содержит аккаунт с Id = {account.Id}"); }
        
        var localUserGroup = GetUserGroups().FirstOrDefault(userGroup => userGroup.LocalId == groupLocalId);
        if (localUserGroup is null) { throw new OrganizationException($"Организация не содержит группу пользователей с локальным Id = {groupLocalId}"); }

        localUserGroup.AddAccount(account);
        
        /*
         * удаляем аккаунт из группы "Без роли" так как аккаунт может
         * одновременно содержаться в группе "Без роли" и в какой-либо другой
         */
        var noRoleUserGroup = GetUserGroups().First(userGroup => userGroup.LocalId == (int) PrimaryUserGroups.NoRole);
        if (noRoleUserGroup is null) { throw new OrganizationException("Не найдена группа пользователей 'Пользователи без роли'"); }
        noRoleUserGroup.RemoveAccount(account.Id);
    }
    
    /// <summary>
    /// Метод добавляет пользователя с указанным уникальным идентификатором
    /// в группу пользователей с указанным локальным идентификатором группы
    /// </summary>
    /// <param name="accountId">Уникальный идентификатор добавляемого аккаунта</param>
    /// <param name="groupLocalId">Идентификатор группы пользователей в организации</param>
    /// <exception cref="OrganizationException">
    /// Возникает в случае, если аккаунт с указанным accountId не содержится в организации
    /// </exception>
    public void AddAccountToGroup(ulong accountId, short groupLocalId)
    {
        var account = Contains(accountId);
        if (account is null) { throw new OrganizationException($"Организация не содержит аккаунт с Id = {accountId}. Сначала нужно добавить аккаунт в организацию"); }
        
        AddAccountToGroup(account, groupLocalId);
    }
    
    /// <summary>
    /// Метод удаляет аккаунт с указанным уникальным идентификатором из
    /// группы пользователей с указанным локальным идентификатором группы
    /// </summary>
    /// <param name="accountId">Уникальный идентификатор удаляемого аккаунта</param>
    /// <param name="groupLocalId">Идентификатор группы пользователей в организации</param>
    /// <exception cref="OrganizationException">
    /// Возникает в случае, если аккаунт с указанным accountId не содержится в организации
    /// </exception>
    public void RemoveAccountFromGroup(ulong accountId, short groupLocalId)
    {
        var account = Contains(accountId);
        if (account is null) { throw new OrganizationException($"Организация не содержит аккаунт с Id = {accountId}"); }
        
        var localUserGroup = GetUserGroups().First(userGroup => userGroup.LocalId == groupLocalId);
        if (localUserGroup is null) { throw new OrganizationException($"Организация не содержит группу пользователей с локальным Id = {groupLocalId}"); }

        localUserGroup.RemoveAccount(account.Id);
        
        /*
         * если после удаление не содержится ни в одной группе пользователей организации,
         * добавляем его в группу "Пользователи без роли"
         */
        var noRoleUserGroup = GetUserGroups().First(userGroup => userGroup.LocalId == (int) PrimaryUserGroups.NoRole);
        if (noRoleUserGroup is null) { throw new OrganizationException("Не найдена группа пользователей 'Пользователи без роли'"); }
        if (Contains(accountId) is null) { noRoleUserGroup.AddAccount(account); }
    }
    #endregion

    #region Properties
    /// <summary>
    /// Id рганизации в базе данных
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public ulong Id { get; init; }
    
    /// <summary>
    /// Название организации
    /// </summary>
    [Column("name"), MaxLength(50)]
    public required string Name { get; set; }

    /// <summary>
    /// Описание организации
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Проекты организации
    /// </summary>
    /// <remarks>
    /// Свойство должно быть использовано исключительно с EntityFramework
    /// </remarks>
    public IReadOnlyCollection<ModelProject> Projects { get; set; } = new List<ModelProject>();

    /// <summary>
    /// Группы пользователей организации
    /// </summary>
    /// <remarks>
    /// Свойство должно быть использовано исключительно с EntityFramework
    /// </remarks>
    public IReadOnlyCollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    #endregion
    #endregion
}

public class OrganizationException : Exception
{
    public OrganizationException(string message) : base(message) { }
    public OrganizationException() { }
}