using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Organization.Controllers.Common;

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
        // todo
        return ResponseGenerator.Ok(value:new List<string>());
    }

    [HttpPost("")]
    public IActionResult PostAccount()
    {
        // todo
        return ResponseGenerator.Ok();
    }

    [HttpPatch("{AccountId:long}")]
    public IActionResult PatchAccount(long AccountId)
    {
        // todo
        return ResponseGenerator.Ok();
    }

    #endregion



    #region Private

    private readonly ILogger<AccountController> _logger;

    #endregion
}