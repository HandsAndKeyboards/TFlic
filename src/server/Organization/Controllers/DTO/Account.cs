namespace Organization.Controllers.DTO;

using ModelAccount = Models.Organization.Accounts.Account;

/// <summary>
/// Класс используется для обмена данными между клиентом и сервером
/// </summary>
public record Account
{
    public Account(ModelAccount account)
    {
        Id = account.Id;
        Name = account.Name;
        Login = account.AuthInfo.Login;

        var userGroups = account.UserGroups;
        if (userGroups is null) { return; }
        
        foreach (var userGroup in userGroups) { UserGroups.Add(userGroup.GlobalId); }
    }
    
    public ulong Id { get; }
    public string Login { get; }
    public string Name { get; }
    public List<ulong> UserGroups { get; } = new();
}