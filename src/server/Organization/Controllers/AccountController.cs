using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        var context = new AccountContext();
        // todo
        return ResponseGenerator.Ok(value:context.Accounts.ToList());
    }
    [HttpPost("")]
    public IActionResult PostAccount([FromBody] Account element)
    {
        // todo
        //var req = HttpContext.Request;
        //var body = req.Body;
        
        //var str = new StreamReader(body).ReadToEndAsync().Result;
        //var reader = new JsonTextReader(new StringReader(str));
        AccountContext context = new AccountContext();
        context.Accounts.Add(element);
        context.SaveChanges();
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