namespace Organization.Models.Organization.Accounts;

public interface IAccount
{
    /// <summary>
    /// Уникальный идентефикатор аккаунта
    /// </summary>
    long Id { get; init; }
    
    /// <summary>
    /// Имя аккаунта
    /// </summary>
    string Name { get; set; }
}