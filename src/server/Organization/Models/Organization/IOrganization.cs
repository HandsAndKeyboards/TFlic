using Organization.Models.Organization.Accounts;
using Organization.Models.Organization.Project;

namespace Organization.Models.Organization;

public interface IOrganization : IUserAggregator
{
    /// <summary>
    /// Уникальный идентификатор организации
    /// </summary>
    long Id { get; init; }

    /// <summary>
    /// Название организации
    /// </summary>
    string Name { get; set; }
    
    /// <summary>
    /// Описание организации
    /// </summary>
    string Description { get; set; }
    
    /// <summary>
    /// Активные проекты организации
    /// </summary>
    ICollection<IProject> ActiveProjects { get; init; }
    
    /// <summary>
    /// Архивные проекты организации
    /// </summary>
    ICollection<IProject> ArchivedProjects { get; init; }
    
    /// <summary>
    /// Участники организации
    /// </summary>
    ICollection<IAccount> Accounts { get; init; }

    /// <summary>
    /// Группы пользователй организации
    /// </summary>
    ICollection<IUserGroup> UserGroups { get; init; }
}