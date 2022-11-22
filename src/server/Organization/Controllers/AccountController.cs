using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization.Accounts;

namespace Organization.Controllers;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[ApiController]
[Route("Accounts/{AccountId}")]
public class AccountController : ControllerBase
{
    #region Public
    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult GetAccount(ulong AccountId)
    {
        List<Account> accounts;
        try { accounts = SelectAccounts(AccountId); }
        catch { return Handlers.HandleNullDbContext(typeof(AccountContext)); }

        return accounts.Count switch
        {
            < 1 => Handlers.HandleElementNotFound(nameof(Account), AccountId),
            > 1 => Handlers.HandleFoundMultipleElementsWithSameId(nameof(Account), AccountId),
            _ => ResponseGenerator.Ok(value: new DTO.Account(accounts[0]))
        };
    }

    [HttpPatch("")]
    public IActionResult PatchAccount(ulong AccountId, [FromBody] JsonPatchDocument<Account> patch)
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }

        var account = accountContext.Accounts.FirstOrDefault(acc => acc.Id == AccountId);
        if (account is null) { return Handlers.HandleElementNotFound(nameof(Account), AccountId); }
        
        patch.ApplyTo(account);
        accountContext.SaveChanges();
        
        return ResponseGenerator.Ok(value: new DTO.Account(account));
    }

    [HttpGet("Organizations")]
    public IActionResult GetAccountsOrganizations(ulong AccountId)
    {
        List<Account> accounts;
        try { accounts = SelectAccounts(AccountId); }
        catch { return Handlers.HandleNullDbContext(typeof(AccountContext)); }

        if (accounts.Count < 1) { return Handlers.HandleElementNotFound(nameof(Account), AccountId); }
        if (accounts.Count > 1) { return Handlers.HandleFoundMultipleElementsWithSameId(nameof(Account), AccountId); }
        
        var organizationIds = new HashSet<ulong>();
        foreach (var userGroup in accounts[0].UserGroups) { organizationIds.Add(userGroup.OrganizationId); }
       
        return ResponseGenerator.Ok(value: organizationIds);
    }
    #endregion



    #region Private
    #region Methods
    private static List<Account> SelectAccounts(ulong accountId)
    {
        using var accountContext = DbContexts.GetNotNull<AccountContext>();
        var accounts = accountContext.Accounts
            .Where(acc => acc.Id == accountId)
            .Include(acc => acc.UserGroups)
            .ToList();

        return accounts;
    }
    #endregion
    
    #region Fields
    private readonly ILogger<AccountController> _logger;
    #endregion
    #endregion
}