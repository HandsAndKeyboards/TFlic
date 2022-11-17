namespace Organization.Models.Organization.Accounts;

public interface IUserGroup : IUserAggregator
{
    /// <summary>
    /// Уникальный идентификатор группы пользователей
    /// </summary>
    long LocalId { get; init; }
    
    /// <summary>
    /// Название группы пользователей
    /// </summary>
    string Name { get; set; }
    
    /// <summary>
    /// Участники группы пользователей
    /// </summary>
    IReadOnlyCollection<IAccount> Accounts { get; init; }
}