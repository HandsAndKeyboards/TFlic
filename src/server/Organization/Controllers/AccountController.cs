using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO;
using Organization.Models.Authentication;
using Organization.Models.Contexts;
using Organization.Models.Organization.Accounts;

namespace Organization.Controllers;

#if AUTH
using Microsoft.AspNetCore.Authorization;
[Authorize]
#endif
[SuppressMessage("ReSharper", "InconsistentNaming")]
[ApiController]
[Route("Accounts/")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// Получение аккаунта с указанным Login
    /// </summary>
    [HttpGet("/{AccountLogin}")]
    public ActionResult<AccountDto> GetAccountByLogin(string AccountLogin)
    {
        using var authInfoContext = DbContexts.Get<AuthInfoContext>();
        var account = authInfoContext.Info
            .Where(info => info.Login == AccountLogin)
            .Include(info => info.Account)
            .Select(info => info.Account)
            .SingleOrDefault();
        if (account is null) { return NotFound(); }

        var foundAccount = new Account
        {
            Id = account.Id,
            Name = account.Name,
            UserGroups = account.GetUserGroups(),
            AuthInfo = new AuthInfo
            {
                Login = AccountLogin,
                PasswordHash = string.Empty
            }
        };
        return Ok(new AccountDto(foundAccount));
    }
    
    /// <summary>
    /// Получение аккаунта с указанным Id
    /// </summary>
    [HttpGet("/{AccountId:long}")]
    public ActionResult<AccountDto> GetAccountById(long AccountId)
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        
        var account = accountContext.Accounts
            .Where(acc => acc.Id == (ulong) AccountId)
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
    [HttpPatch("/{AccountId}")]
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
    [HttpGet("/{AccountId}/Organizations")]
    public ActionResult<IEnumerable<ulong>> GetAccountsOrganizations(ulong AccountId)
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        
        var account = accountContext.Accounts.SingleOrDefault(acc => acc.Id == AccountId);
        if (account is null) { return NotFound(); }

        var organizationIds = account.GetOrganizations().Select(org => org.Id);
        return Ok(organizationIds);
    }
}
