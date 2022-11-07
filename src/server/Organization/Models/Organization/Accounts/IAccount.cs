namespace Organization.Models.Organization.Accounts;

public interface IAccount
{
    /// <summary>
    /// Метод проверяет, состоит ли пользователь в указанной организации
    /// </summary>
    /// <param name="organization">Проверяемая организация</param>
    /// <returns>true, если состоит, иначе - false</returns>
    bool IsMemberOf(IOrganization organization);

    /// <summary>
    /// Метод проверяет, состоит ли пользователь в организации с указанным уникальным идентификатором
    /// </summary>
    /// <param name="id">Уникальный идентификатор проверяемой организации</param>
    /// <returns>Ссылка на организацию с указанным Id, если состоит, иначе - false</returns>
    IOrganization? IsMemberOf(long id);
    
    /// <summary>
    /// Уникальный идентефикатор аккаунта
    /// </summary>
    long Id { get; init; }
    
    /// <summary>
    /// Имя аккаунта
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Организации, в которых состоит пользователь
    /// </summary>
    ICollection<IOrganization> Organizations { get; init; }
}