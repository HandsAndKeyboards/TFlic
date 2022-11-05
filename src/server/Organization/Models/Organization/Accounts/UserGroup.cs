namespace Organization.Models.Organization.Accounts;

public class UserGroup : IUserGroup
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
    
    public bool RemoveAccount(long id)
    {
        var toRemove = _accounts.Find(account => account.Id == id);
        return toRemove is not null && _accounts.Remove(toRemove);
    }
    
    public bool Contains(IAccount account)
    {
        return _accounts.Contains(account);
    }
    
    public bool Contains(long id)
    {
        return _accounts.Any(account => account.Id == id);
    }
    
    #endregion

    #region Propertiew

    public required long Id { get; init; }
    public required string Name { get; set; }

    public ICollection<IAccount> Accounts
    {
        get => _accounts; 
        init => _accounts = (List<IAccount>) value;
    }

    #endregion
    #endregion



    #region Private
    #region Fields

    private readonly List<IAccount> _accounts = new();

    #endregion
    #endregion
}