using Organization.Models.Organization.Accounts;
using Organization.Models.Organization.Project;

namespace Organization.Models.Organization;

public interface IOrganization : IUserAggregator
{
    /// <summary>
    /// Активные проекты организации
    /// </summary>
    ICollection<IProject> ActiveProjects { get; set; }
    
    /// <summary>
    /// Архивные проекты организации
    /// </summary>
    ICollection<IProject> ArchivedProjects { get; set; }

    /// <summary>
    /// Группы пользователй организации
    /// </summary>
    ICollection<IUserGroup> UserGroups { get; set; }
}