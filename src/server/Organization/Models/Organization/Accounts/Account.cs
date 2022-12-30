using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Organization.Models.Authentication;
using Organization.Models.Contexts;

namespace Organization.Models.Organization.Accounts;

[Table("accounts")]
public class Account
{
    #region Methods
    /// <summary>
    /// Метод проверяет, состоит ли пользователь в организации с указанным уникальным идентификатором
    /// </summary>
    /// <param name="id">Уникальный идентификатор проверяемой организации</param>
    /// <returns>Ссылка на организацию с указанным Id, если состоит, иначе - false</returns>
    public Organization? IsMemberOf(ulong id)
    {
        using var orgContext = DbContexts.Get<OrganizationContext>();
        var organization = orgContext.Organizations.FirstOrDefault(org => org.Id == id);
        if (organization is null) { throw new OrganizationException($"Организация с Id = {id} не существует"); }
        
        return organization.Contains(Id) is not null ? organization : null;
    }

    public IEnumerable<Organization> GetOrganizations()
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        var account = accountContext.Accounts
            .Where(acc => acc.Id == Id)
            .Include(acc => acc.UserGroups)
            .ThenInclude(userGroup => userGroup.Accounts)
            .Single();

        using var orgContext = DbContexts.Get<OrganizationContext>(); 
        var organizations = new List<Organization>();
        foreach (var userGroup in account.UserGroups)
        {
            var org = orgContext.Organizations.Single(org => org.Id == userGroup.OrganizationId);
            if (!organizations.Contains(org)) { organizations.Add(org); }
        }

        return organizations;
    }
    
    public ICollection<UserGroup> GetUserGroups()
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        var account = accountContext.Accounts
            .Where(acc => acc.Id == Id)
            .Include(acc => acc.UserGroups)
            .Single();
        
        return account.UserGroups;
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
    /// Организации, в которых состоит пользователь
    /// </summary>
    [NotMapped]
    public ICollection<UserGroup> UserGroups { get; set; } = null!;

    /// <summary>
    /// Служебное поле, используется EF для настройки связи многие-ко-многим с сущностью Account
    /// </summary>
    [NotMapped]
    public List<UserGroupsAccounts> UserGroupsAccounts { get; set; } = null!;

    public AuthInfo AuthInfo { get; set; } = null!;
    #endregion
}