using System.ComponentModel.DataAnnotations;
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

    public bool AddAccount(IAccount account)
    {
        if (Contains(account)) { return false; }
        
        var noRole = _userGroups.Find(group => group.Id == (int) PrimaryUserGroups.NoRole);
        if (noRole is null) { throw new OrganizationException("Не найдена группа пользователей 'Пользователи без роли'"); }

        noRole.AddAccount(account);

        return true;
    }
    
    public bool RemoveAccount(IAccount account)
    {
        var toRemove = Contains(account.Id);
        return toRemove is not null && 
               _userGroups.Aggregate(false, (current, userGroup) => current || userGroup.RemoveAccount(toRemove));
    }
    
    public IAccount? RemoveAccount(long id)
    {
        var toRemove = Contains(id);
        if (toRemove is not null)
        {
            _userGroups.Aggregate(false, (current, userGroup) => current || userGroup.RemoveAccount(toRemove));
        }
        
        return toRemove;
    }
    
    public bool Contains(IAccount account)
    {
        return _userGroups.Any(userGroup => userGroup.Contains(account));
    }
    
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
        _userGroups.Add(new UserGroup {Id = (int) PrimaryUserGroups.NoRole, Name = "Пользователи без роли"});
        _userGroups.Add(new UserGroup {Id = (int) PrimaryUserGroups.Admins, Name = "Администраторы организации"});
        _userGroups.Add(new UserGroup {Id = (int) PrimaryUserGroups.ProjectsMembers, Name = "Участники проектов"});
    }
    #endregion

    #region Properties

    [Required]
    public long Id { get; init; }
    [Required]
    public string Name { get; set; }
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

    public IReadOnlyCollection<IUserGroup> UserGroups
    {
        get => _userGroups;
        init => _userGroups = (List<IUserGroup>) value;
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