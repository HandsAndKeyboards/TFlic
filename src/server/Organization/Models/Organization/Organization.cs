using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Organization;

[Table("organizations")]
public class Organization : IUserAggregator
{
    /// <summary>
    /// Основные группы пользоватлелей
    /// </summary>
    private enum PrimaryUserGroups { NoRole, Admins, ProjectsMembers }

    #region Public

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
        
        var noRole = _userGroups.Find(group => group.GlobalId == (ulong) PrimaryUserGroups.NoRole);
        if (noRole is null) { throw new OrganizationException("Не найдена группа пользователей 'Пользователи без роли'"); }

        noRole.AddAccount(account);

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
            _userGroups.Aggregate(false, (current, userGroup) => current || userGroup.RemoveAccount(toRemove.Id) is not null);
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
        foreach (var userGroup in _userGroups)
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
        _userGroups.Add(new UserGroup {LocalId = (int) PrimaryUserGroups.NoRole, OrganizationId = Id,  Name = "Пользователи без роли"});
        _userGroups.Add(new UserGroup {LocalId = (int) PrimaryUserGroups.Admins, OrganizationId = Id, Name = "Администраторы организации"});
        _userGroups.Add(new UserGroup {LocalId = (int) PrimaryUserGroups.ProjectsMembers, OrganizationId = Id, Name = "Участники проектов"});
    }
    #endregion

    #region Properties
    
    [Key, Column("id")]
    public ulong Id { get; init; }
    
    [Column("name"), MaxLength(50)]
    public required string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    /*
     * todo при добавлении булевого флага "IsActive" в проект нужно заменить два 
     * todo массива "ActiveProjects" "ArchivedProjects" на один "Projects"
     */
    public IReadOnlyCollection<Project.Project> Projects
    {
        get => _activeProjects;
        init => _activeProjects = (List<Project.Project>) value;
    }


    /// <summary>
    /// Группы пользователй организации
    /// </summary>
    /// <remarks>
    /// Свойство не должно использоваться для перемещения аккаунтов между группами пользователей
    /// </remarks>
    public ICollection<UserGroup> UserGroups
    {
        get => _userGroups;
        init => _userGroups = (List<UserGroup>) value;
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
        
        var localUserGroup = _userGroups.Find(userGroup => userGroup.LocalId == groupLocalId);
        if (localUserGroup is null) { throw new OrganizationException($"Организация не содержит группу пользователей с локальным Id = {groupLocalId}"); }

        localUserGroup.AddAccount(account);
        
        /*
         * удаляем аккаунт из группы "Без роли" так как аккаунт может
         * одновременно содержаться в группе "Без роли" и в какой-либо другой
         */
        var noRoleUserGroup = _userGroups.Find(userGroup => userGroup.LocalId == (int) PrimaryUserGroups.NoRole);
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
        if (account is null) { throw new OrganizationException($"Организация не содержит аккаунт с Id = {accountId}"); }
        
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
        
        var localUserGroup = _userGroups.Find(userGroup => userGroup.LocalId == groupLocalId);
        if (localUserGroup is null) { throw new OrganizationException($"Организация не содержит группу пользователей с локальным Id = {groupLocalId}"); }

        localUserGroup.RemoveAccount(account.Id);
        
        /*
         * если после удаление не содержится ни в одной группе пользователей организации,
         * добавляем его в группу "Пользователи без роли"
         */
        var noRoleUserGroup = _userGroups.Find(userGroup => userGroup.LocalId == (int) PrimaryUserGroups.NoRole);
        if (noRoleUserGroup is null) { throw new OrganizationException("Не найдена группа пользователей 'Пользователи без роли'"); }
        if (Contains(accountId) is null) noRoleUserGroup.AddAccount(account);
    }
    
    #endregion
    #endregion



    #region Private
    #region Methods
    #endregion

    #region Fields

    private readonly List<Project.Project> _activeProjects = new ();
    private readonly List<Project.Project> _archivedProjects = new ();
    private readonly List<UserGroup> _userGroups = new ();

    #endregion

    #endregion
}

public class OrganizationException : Exception
{
    public OrganizationException(string message) : base(message) { }
    public OrganizationException() { }
}