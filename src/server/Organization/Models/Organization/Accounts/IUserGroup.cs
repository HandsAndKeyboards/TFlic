namespace Organization.Models.Organization.Accounts;

public interface IUserGroup : IUserAggregator
{
    /// <summary>
    /// Уникальный идентификатор группы пользователей
    /// </summary>
    long Id { get; init; }
    
    /// <summary>
    /// Название группы пользователей
    /// </summary>
    string Name { get; set; }
    
    /// <summary>
    /// Участники группы пользователей
    /// </summary>
    ICollection<IAccount> Accounts { get; init; }
}