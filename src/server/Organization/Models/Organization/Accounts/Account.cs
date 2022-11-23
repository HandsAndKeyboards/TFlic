using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Organization.Models.Contexts;

namespace Organization.Models.Organization.Accounts;

[Table("accounts")]
public class Account
{
    #region Public
    #region Methods
    /// <summary>
    /// Метод проверяет, состоит ли пользователь в организации с указанным уникальным идентификатором
    /// </summary>
    /// <param name="id">Уникальный идентификатор проверяемой организации</param>
    /// <returns>Ссылка на организацию с указанным Id, если состоит, иначе - false</returns>
    public Organization? IsMemberOf(ulong id)
    {
        using var orgContext = DbContexts.GetNotNull<OrganizationContext>();
        var organization = orgContext.Organizations.FirstOrDefault(org => org.Id == id);
        if (organization is null) { throw new OrganizationException($"Организация с Id = {id} не существует"); }
        
        return organization.Contains(Id) is not null ? organization : null;
    }
    #endregion
    
    #region Properties
    /// <summary>
    /// Уникальный идентификатор аккаунта
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    [Column("id")]
    public ulong Id { get; init;  }
    
    /// <summary>
    /// Имя аккаунта
    /// </summary>
    [Column("name"), MaxLength(50)]
    public required string Name { get; set; }
    
    /// <summary>
    /// Логин аккаунта
    /// </summary>
    [Column("login"), MaxLength(50)]
    public required string Login { get; init; }

    /// <summary>
    /// Хеш пароля аккаунта
    /// </summary>
    [Column("password_hash")]
    public required string PasswordHash { get; init; }

    /// <summary>
    /// Организации, в которых состоит пользователь
    /// </summary>
    public ICollection<UserGroup> UserGroups { get; set; } = null!;

    /// <summary>
    /// Служебное поле, используется EF для настройки связи многие-ко-многим с сущностью Account
    /// </summary>
    public List<UserGroupsAccounts> UserGroupsAccounts { get; set; } = null!;
    #endregion
    #endregion
}