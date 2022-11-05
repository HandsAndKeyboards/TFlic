namespace Organization.Models.Organization.Accounts;

public class Account : IAccount 
{
    public Account () {  }
    
    public Account(string name)
    {
        Name = name;
    }
    
    /// <summary>
    /// Уникальный идентификатор аккаунта
    /// </summary>
    public required long Id { get; init;  }
    
    /// <summary>
    /// Имя аккаунта
    /// </summary>
    public required string Name { get; set; }
}