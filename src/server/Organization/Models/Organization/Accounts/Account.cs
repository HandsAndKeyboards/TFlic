using System.ComponentModel.DataAnnotations;

namespace Organization.Models.Organization.Accounts;

public class Account : IAccount 
{
    #region Public

    #region Merhods

    public bool IsMemberOf(IOrganization organization)
    {
        return organization.Contains(this);
    }

    public IOrganization? IsMemberOf(long id)
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
    public required long Id { get; init;  }
    
    /// <summary>
    /// Имя аккаунта
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Организации, в которых состоит пользователь
    /// </summary>
    public IReadOnlyCollection<IOrganization> Organizations
    {
        get => _organizations; 
        init => _organizations = (List<IOrganization>) value;
    }

    #endregion
    
    #endregion



    #region Private

    private List<IOrganization> _organizations = new();
    
    #endregion
}