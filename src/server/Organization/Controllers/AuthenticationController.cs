using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Organization.Controllers.Common;
using Organization.Controllers.DTO;
using Organization.Models.Authentication;
using Organization.Models.Contexts;
using Account = Organization.Models.Organization.Accounts.Account;

namespace Organization.Controllers;

// todo

[ApiController]
public class AuthenticationController : ControllerBase
{
    #region Public
    public AuthenticationController(ILogger<AuthenticationController> logger)
    {
        _logger = logger;
    }

    [HttpPost("/login")]
    public IActionResult Login(AuthenticationAccount loginAccount)
    {
        // todo проверка аккаунта
        var account = DbContexts.AccountContext.Accounts.FirstOrDefault(acc => acc.Login == loginAccount.Login);
        if(account is null) return ResponseGenerator.Unauthorized();

        var remoteIp = HttpContext.Connection.RemoteIpAddress!.ToString();
        var tokens = AuthenticationManager.GenerateTokens(account, remoteIp);
        
        var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(tokens.access);
        var encodedRefreshToken = new JwtSecurityTokenHandler().WriteToken(tokens.refresh);
        
        var response = new
        {
            access_token = encodedAccessToken,
            refresh_token = encodedRefreshToken,
        };

        return Ok(response);
    }

    [HttpPost("/refresh")]
    public IActionResult Refresh(RefreshTokenRequest request) 
    {
        // todo проверка аккаунта
        var account = DbContexts.AccountContext.Accounts.FirstOrDefault(acc => acc.Login == request.Login);
        if(account is null) return ResponseGenerator.Unauthorized();
        
        var remoteIp = HttpContext.Connection.RemoteIpAddress!.ToString();
        
        var tokenValid = 
            AuthenticationManager.IsTokenValid(request.RefreshToken, AuthenticationManager.RefreshTokenValidationParameters) && 
            AuthenticationManager.IsAudienceIpValid(remoteIp, request.RefreshToken); 
        if (!tokenValid) { return Unauthorized("refresh token is invalid"); }

        var tokens = AuthenticationManager.GenerateTokens(account, remoteIp);
        
        var response = new
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(tokens.access),
            refresh_token = new JwtSecurityTokenHandler().WriteToken(tokens.refresh) 
        };

        return Ok(response);
    }

    [HttpPost("/register")]
    public IActionResult Register(RegistrationAccount account)
    {
        var newAccount = new Account
        {
            Name = account.Name, 
            Login = account.Login, 
            PasswordHash = account.PasswordHash
        };
        DbContexts.AccountContext.Add(newAccount);

        newAccount = DbContexts.AccountContext.Accounts.First(acc => acc.Login == account.Login);
        return ResponseGenerator.Ok(value: newAccount);
    }
    #endregion


    #region Private
    #region Methods
    #endregion

    #region Fields

    private readonly ILogger<AuthenticationController> _logger;
    #endregion
    #endregion
}