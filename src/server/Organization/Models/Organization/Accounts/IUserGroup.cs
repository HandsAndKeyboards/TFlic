namespace Organization.Models.Organization.Accounts;

public interface IUserGroup : IUserAggregator
{
    /// <summary>
    /// Название группы пользователей
    /// </summary>
    string Name { get; set; }
    
    /// <summary>
    /// Участники группы пользователей
    /// </summary>
    ICollection<IAccount> Accounts { get; set; }
}