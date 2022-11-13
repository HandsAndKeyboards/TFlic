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
    IReadOnlyCollection<IProject> ActiveProjects { get; init; }
    
    /// <summary>
    /// Архивные проекты организации
    /// </summary>
    IReadOnlyCollection<IProject> ArchivedProjects { get; init; }

    /// <summary>
    /// Группы пользователй организации
    /// </summary>
    IReadOnlyCollection<IUserGroup> UserGroups { get; init; }

    /// <summary>
    /// Метод добавляет указанного пользователя в группу пользователей
    /// с указанным идентификатором группы в организации
    /// </summary>
    /// <param name="account">Добавляемый аккаунт</param>
    /// <param name="groupLocalId">Идентификатор группы пользователей в организации</param>
    void AddAccountToGroup(IAccount account, long groupLocalId);

    /// <summary>
    /// Метод добавляет пользователя с указанным уникальным идентификатором
    /// в группу пользователей с указанным локальным идентификатором группы
    /// </summary>
    /// <remarks></remarks>
    /// <param name="accountId">Уникальный идентификатор добавляемого аккаунта</param>
    /// <param name="groupLocalId">Идентификатор группы пользователей в организации</param>
    void AddAccountToGroup(long accountId, long groupLocalId);

    /// <summary>
    /// Метод удаляе указанный аккаунт из группы пользователей
    /// с указанным локальным идентификатором группы
    /// </summary>
    /// <param name="account">Удаляемый аккаунт</param>
    /// <param name="groupLocalId">Идентификатор группы пользователей в организации</param>
    void RemoveAccountFromGroup(IAccount account, long groupLocalId);

    /// <summary>
    /// Метод удаляет аккаунт с указанным уникальным идентификатором из
    /// группы пользователей с указанным локальным идентификатором группы
    /// </summary>
    /// <param name="accountId">Уникальный идентификатор удаляемого аккаунта</param>
    /// <param name="groupLocalId">Идентификатор группы пользователей в организации</param>
    void RemoveAccountFromGroup(long accountId, long groupLocalId);
}