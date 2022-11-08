using System.ComponentModel.DataAnnotations;
using Organization.Models.Organization.Accounts;
using Organization.Models.Organization.Project;

namespace Organization.Models.Organization;

public class Organization : IOrganization
{
    #region Public

    #region Methods
    public bool AddAccount(IAccount account)
    {
        if (Contains(account)) { return false; }
        
        _accounts.Add(account);
        return true;
    }
    
    public bool RemoveAccount(IAccount account)
    {
        return _accounts.Remove(account);
    }
    
    public IAccount? RemoveAccount(long id)
    {
        var toRemove = _accounts.Find(account => account.Id == id);
        if (toRemove is not null) { _accounts.Remove(toRemove); }
        
        return toRemove;
    }
    
    public bool Contains(IAccount account)
    {
        return _accounts.Contains(account);
    }
    
    public IAccount? Contains(long id)
    {
        return _accounts.Find(account => account.Id == id); 
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

    public IReadOnlyCollection<IAccount> Accounts
    {
        get => _accounts;
        init => _accounts = (List<IAccount>) value;
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
    
    private readonly List<IAccount> _accounts = new ();
    private readonly List<IUserGroup> _userGroups = new ();

    #endregion

    #endregion
}