namespace Organization.Models.Organization.Accounts;

public interface IAccount
{
    /// <summary>
    /// Уникальный идентефикатор аккаунта
    /// </summary>
    byte[] Uid { get; set; }
    
    /// <summary>
    /// Имя аккаунта
    /// </summary>
    string Name { get; set; }
}