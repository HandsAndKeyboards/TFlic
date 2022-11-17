﻿using System.ComponentModel.DataAnnotations;
using Organization.Models.Organization.Accounts;
using Organization.Models.Organization.Project;

namespace Organization.Models.Organization;

public class Organization : IOrganization
{
    public class OrganizationException : Exception
    {
        public OrganizationException(string message) : base(message) { }
        public OrganizationException() { }
    }
    
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
    public bool AddAccount(IAccount account)
    {
        if (Contains(account)) { return false; }
        
        var noRole = _userGroups.Find(group => group.GlobalId == (int) PrimaryUserGroups.NoRole);
        if (noRole is null) { throw new OrganizationException("Не найдена группа пользователей 'Пользователи без роли'"); }

        noRole.AddAccount(account);

        return true;
    }
    
    /// <summary>
    /// Метод удаляет указанный аккаунт из всех групп пользователей организации
    /// </summary>
    /// <returns>true в случае успешного удаления, иначе - false</returns>
    public bool RemoveAccount(IAccount account)
    {
        var toRemove = Contains(account.Id);
        return toRemove is not null && 
               _userGroups.Aggregate(false, (current, userGroup) => current || userGroup.RemoveAccount(toRemove));
    }
    
    /// <summary>
    /// Метод удаляет аккаунт с уаазаннм уникальным идентификатором из всех групп пользователей организации
    /// </summary>
    /// <param name="id">Уникальный идентификатор удаляемого аккаунта</param>
    /// <returns>Ссылка на удаляемый объект в случае успешного удаления, иначе - null</returns>
    public IAccount? RemoveAccount(long id)
    {
        var toRemove = Contains(id);
        if (toRemove is not null)
        {
            _userGroups.Aggregate(false, (current, userGroup) => current || userGroup.RemoveAccount(toRemove));
        }
        
        return toRemove;
    }
    
    /// <summary>
    /// Метод проверяет, содержит ли любая из групп пользователей
    /// организации пользователя с указанным идентификатором
    /// </summary>
    /// /// <returns>true в случае успушного удаление, иначе - false</returns>
    public bool Contains(IAccount account)
    {
        return _userGroups.Any(userGroup => userGroup.Contains(account));
    }
    
    /// <summary>
    /// Метод проверяет, содержит ли любая из групп пользователей
    /// организации пользователя с указанным идентификатором
    /// </summary>
    /// <param name="id">Уникальный идентификатор проверяемого пользователя</param>
    /// <returns>ссылка на объект, если он присутствует, иначе - null </returns>
    public IAccount? Contains(long id)
    {
        IAccount? account = null;
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
        // todo выдача уникальных глобальных id
        _userGroups.Add(new UserGroup {GlobalId = 0, LocalId = (int) PrimaryUserGroups.NoRole, Name = "Пользователи без роли"});
        _userGroups.Add(new UserGroup {GlobalId = 1, LocalId = (int) PrimaryUserGroups.Admins, Name = "Администраторы организации"});
        _userGroups.Add(new UserGroup {GlobalId = 2, LocalId = (int) PrimaryUserGroups.ProjectsMembers, Name = "Участники проектов"});
    }
    #endregion

    #region Properties
    
    public required long Id { get; init; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;

    public IReadOnlyCollection<IProject> ActiveProjects
    {
        get => _activeProjects;
        init => _activeProjects = (List<IProject>) value;
    }

    public IReadOnlyCollection<IProject> ArchivedProjects
    {
        get => _archivedProjects;
        init => _archivedProjects = (List <IProject>) value;
    }

    /// <summary>
    /// Группы пользователй организации
    /// </summary>
    /// <remarks>
    /// Свойство не должно использоваться для перемещения аккаунтов между группами пользователей
    /// </remarks>
    public IReadOnlyCollection<IUserGroup> UserGroups
    {
        get => _userGroups;
        init => _userGroups = (List<IUserGroup>) value;
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
    public void AddAccountToGroup(IAccount account, long groupLocalId)
    {
        if (!Contains(account)) { throw new OrganizationException($"Организация не содержит аккаунт с Id = {account.Id}"); }
        
        var localUserGroup = _userGroups.Find(userGroup => userGroup.LocalId == groupLocalId);
        if (localUserGroup is null) { throw new OrganizationException($"Организация не содержит группу пользователей с локальным Id = {groupLocalId}"); }

        localUserGroup.AddAccount(account);
        
        /*
         * удаляем аккаунт из группы "Без роли" так как аккаунт может
         * одновременно содержаться в группе "Без роли" и в какой-либо другой
         */
        var noRoleUserGroup = _userGroups.Find(userGroup => userGroup.LocalId == (int) PrimaryUserGroups.NoRole);
        if (noRoleUserGroup is null) { throw new OrganizationException("Не найдена группа пользователей 'Пользователи без роли'"); }
        noRoleUserGroup.RemoveAccount(account);
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
    public void AddAccountToGroup(long accountId, long groupLocalId)
    {
        var account = Contains(accountId);
        if (account is null) { throw new OrganizationException($"Организация не содержит аккаунт с Id = {accountId}"); }
        
        AddAccountToGroup(account, groupLocalId);
    }
    
    /// <summary>
    /// Метод удаляе указанный аккаунт из группы пользователей
    /// с указанным локальным идентификатором группы
    /// </summary>
    /// <param name="account">Удаляемый аккаунт</param>
    /// <param name="groupLocalId">Идентификатор группы пользователей в организации</param>
    /// <exception cref="OrganizationException">
    /// <list type="bullet">
    ///     <listheader>Возникает в случаях:</listheader>
    ///     <item>Организация не содержит указанный аккаунт</item>
    ///     <item>Организация не содержит группу пользователей с указанным локальным Id</item>
    ///     <item>Организация не содержит группу пользователей "Пользователи без роли"</item>
    /// </list>
    /// </exception>
    public void RemoveAccountFromGroup(IAccount account, long groupLocalId)
    {
        if (!Contains(account)) { throw new OrganizationException($"Организация не содержит аккаунт с Id = {account.Id}"); }
        
        var localUserGroup = _userGroups.Find(userGroup => userGroup.LocalId == groupLocalId);
        if (localUserGroup is null) { throw new OrganizationException($"Организация не содержит группу пользователей с локальным Id = {groupLocalId}"); }

        localUserGroup.RemoveAccount(account);
        
        /*
         * если после удаление не содержится ни в одной группе пользователей организации,
         * добавляем его в группу "Пользователи без роли"
         */
        var noRoleUserGroup = _userGroups.Find(userGroup => userGroup.LocalId == (int) PrimaryUserGroups.NoRole);
        if (noRoleUserGroup is null) { throw new OrganizationException("Не найдена группа пользователей 'Пользователи без роли'"); }
        if (!Contains(account)) noRoleUserGroup.AddAccount(account);
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
    public void RemoveAccountFromGroup(long accountId, long groupLocalId)
    {
        var account = Contains(accountId);
        if (account is null) { throw new OrganizationException($"Организация не содержит аккаунт с Id = {accountId}"); }
        
        RemoveAccountFromGroup(account, groupLocalId);
    }
    
    #endregion
    #endregion



    #region Private
    #region Methods
    #endregion

    #region Fields

    private readonly List<IProject> _activeProjects = new ();
    private readonly List<IProject> _archivedProjects = new ();
    private readonly List<IUserGroup> _userGroups = new ();

    #endregion

    #endregion
}