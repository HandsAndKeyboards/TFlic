using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Service;
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
    public IActionResult GetAccount(ulong AccountId)
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }
        
        Account? account;
        try
        {
            account = accountContext.Accounts
                .Where(acc => acc.Id == AccountId)
                .Include(acc => acc.UserGroups)
                .Include(acc => acc.AuthInfo)
                .Single();
        }
        catch (ArgumentNullException) { return Handlers.HandleElementNotFound(nameof(Account), AccountId); }
        catch (InvalidOperationException) { return Handlers.HandleFoundMultipleElementsWithSameId(nameof(Account), AccountId); }

        return ResponseGenerator.Ok(value: new DTO.Account(account));
    }

    /// <summary>
    /// Изменение данных аккаунта с указанным Id
    /// </summary>
    [HttpPatch]
    public IActionResult PatchAccount(ulong AccountId, [FromBody] JsonPatchDocument<Account> patch)
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }

        Account? account;
        try
        {
            account = accountContext.Accounts
                .Where(acc => acc.Id == AccountId)
                .Include(acc => acc.UserGroups)
                .Include(acc => acc.AuthInfo)
                .Single();
        }
        catch (InvalidOperationException err) { return Handlers.HandleElementNotFound(nameof(Account), AccountId); }
        catch (Exception err) { return Handlers.HandleException(err); }
        
        patch.ApplyTo(account);

        try { accountContext.SaveChanges(); }
        catch (DbUpdateException) { return Handlers.HandleException("Updation failure"); }
        catch (Exception err) { return Handlers.HandleException(err); }
        
        return ResponseGenerator.Ok(value: new DTO.Account(account));
    }

    /// <summary>
    /// Получение списка организаций, в которых состоит указанный аккаунт
    /// </summary>
    [HttpGet("Organizations")]
    public IActionResult GetAccountsOrganizations(ulong AccountId)
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }
        
        Account? account;
        try { account = accountContext.Accounts.Single(acc => acc.Id == AccountId); }
        catch (ArgumentNullException) { return Handlers.HandleElementNotFound(nameof(Account), AccountId); }
        catch (InvalidOperationException) { return Handlers.HandleFoundMultipleElementsWithSameId(nameof(Account), AccountId); }
        
        var organizationIds = account.GetOrganizations().Select(org => org.Id);
        return ResponseGenerator.Ok(value: organizationIds);
    }
}
