using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Common;
using Organization.Models.Contexts;
using Organization.Models.Organization.Accounts;

namespace Organization.Controllers;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[ApiController]
[Route("Accounts")]
public class AccountController : ControllerBase
{
    #region Public
    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult GetAccounts()
    {
        // todo dispose для контекста
        var accounts = DbContexts.AccountContext.Accounts
            .Include(acc => acc.UserGroupsAccounts)
            .ThenInclude(uga => uga.UserGroup)
            .ToList();
        
        var dtoAccounts = accounts.Select(account => new DTO.Account(account)).ToList();

        return ResponseGenerator.Ok(value:dtoAccounts);
    }
    [HttpPost("")]
    public IActionResult PostAccount([FromBody] Account element)
    {
        // todo
        return ResponseGenerator.Ok();
    }

    // todo разобраться, как матчить ulong
    [HttpPatch("{AccountId:long}")]
    public IActionResult PatchAccount(ulong AccountId)
    {
        // todo
        return ResponseGenerator.Ok();
    }
    #endregion



    #region Private
    private readonly ILogger<AccountController> _logger;
    #endregion
}