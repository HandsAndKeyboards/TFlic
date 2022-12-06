using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO;
using Organization.Models.Contexts;
using Organization.Models.Organization.Accounts;

namespace Organization.Controllers;

#if AUTH
using Microsoft.AspNetCore.Authorization;
[Authorize]
#endif
[SuppressMessage("ReSharper", "InconsistentNaming")]
[ApiController]
[Route("Accounts/{AccountId}")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// Получение аккаунта с указанным Id
    /// </summary>
    [HttpGet]
    public ActionResult<AccountDto> GetAccount(ulong AccountId)
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        
        var account = accountContext.Accounts
            .Where(acc => acc.Id == AccountId)
            .Include(acc => acc.UserGroups)
            .Include(acc => acc.AuthInfo)
            .SingleOrDefault();

        return account is not null
            ? Ok(new AccountDto(account))
            : NotFound();
    }

    /// <summary>
    /// Изменение данных аккаунта с указанным Id
    /// </summary>
    [HttpPatch]
    public ActionResult<AccountDto> PatchAccount(ulong AccountId, [FromBody] JsonPatchDocument<Account> patch)
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        var account = accountContext.Accounts
            .Where(acc => acc.Id == AccountId)
            .Include(acc => acc.UserGroups)
            .Include(acc => acc.AuthInfo)
            .SingleOrDefault();
        if (account is null) { return NotFound(); }
        
        
        patch.ApplyTo(account);
        accountContext.SaveChanges(); 

        return Ok(new AccountDto(account)) ;
    }

    /// <summary>
    /// Получение списка организаций, в которых состоит указанный аккаунт
    /// </summary>
    [HttpGet("Organizations")]
    public ActionResult<IEnumerable<ulong>> GetAccountsOrganizations(ulong AccountId)
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        
        var account = accountContext.Accounts.SingleOrDefault(acc => acc.Id == AccountId);
        if (account is null) { return NotFound(); }

        var organizationIds = account.GetOrganizations().Select(org => org.Id);
        return Ok(organizationIds);
    }
}
