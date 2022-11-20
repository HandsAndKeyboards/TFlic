using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Organization.Models.Organization.Accounts;

public class Account
{
    #region Public

    #region Merhods

    /// <summary>
    /// Метод проверяет, состоит ли пользователь в указанной организации
    /// </summary>
    /// <param name="organization">Проверяемая организация</param>
    /// <returns>true, если состоит, иначе - false</returns>
    public bool IsMemberOf(Organization organization)
    {
        throw new NotImplementedException(); //organization.Contains(Id);
    }

    /// <summary>
    /// Метод проверяет, состоит ли пользователь в организации с указанным уникальным идентификатором
    /// </summary>
    /// <param name="id">Уникальный идентификатор проверяемой организации</param>
    /// <returns>Ссылка на организацию с указанным Id, если состоит, иначе - false</returns>
    public Organization? IsMemberOf(ulong id)
    {
        /*
         * 1. Найти организацию по Id
         * 2. Проверить, является ли пользователь участником указанной организации
         */
        throw new NotImplementedException();
    }

    #endregion
    
    #region Properties

    /// <summary>
    /// Уникальный идентификатор аккаунта
    /// </summary>
    public required ulong Id { get; init;  }
    
    /// <summary>
    /// Имя аккаунта
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Логин аккаунта
    /// </summary>
    [Column("login"), MaxLength(50)]
    public required string Login { get; init; }

    public required string PasswordHash { get; init; }

    /// <summary>
    /// Организации, в которых состоит пользователь
    /// </summary>
    public IReadOnlyCollection<UserGroup> Organizations
    {
        get => _userGroups; 
        init => _userGroups = (List<UserGroup>) value;
    }

    #endregion
    
    #endregion



    #region Private

    private readonly List<UserGroup> _userGroups = new();
    
    #endregion
}