namespace Organization.Controllers.DTO;

/// <summary>
/// Класс используется для обмена данными между клиентом и сервером
/// </summary>
public record Account
{
    public Account(Models.Organization.Accounts.Account account)
    {
        Id = account.Id;
        Login = account.Login;
        Name = account.Name;

        var userGroups = account.UserGroups;
        if (userGroups is null) { return; }
        
        foreach (var userGroup in userGroups) { UserGroups.Add(userGroup.GlobalId); }
    }
    
    public ulong Id { get; }
    public string Login { get; }
    public string Name { get; }
    public List<ulong> UserGroups { get; } = new();
}