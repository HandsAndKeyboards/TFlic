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

    #region Propertiew
    public required long GlobalId { get; init; }
    public required long LocalId { get; init; }
    public required string Name { get; set; }

    public IReadOnlyCollection<IAccount> Accounts
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