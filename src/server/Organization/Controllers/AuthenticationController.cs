using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Organization.Controllers.Service;
using Organization.Models.Authentication;
using Organization.Models.Contexts;

namespace Organization.Controllers;

using ModelAccount = Models.Organization.Accounts.Account;

[ApiController]
public class AuthenticationController : ControllerBase
{
    /// <summary>
    /// Аутентификация и авторизация пользователя
    /// </summary>
    [HttpPost("/login")]
    public IActionResult Login(DTO.AuthenticationAccount loginAccount)
    {
        // todo проверка аккаунта
        var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }
        
        ModelAccount? account;
        try
        {
            account = accountContext.Accounts.Single(
                acc =>
                    acc.Login == loginAccount.Login &&
                    acc.PasswordHash == loginAccount.PasswordHash
            );
        }
        catch (InvalidOperationException) { return ResponseGenerator.Unauthorized(message: "Login or password is incorrect"); }
        catch (Exception err) { return Handlers.HandleException(err); }

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

    /// <summary>
    /// Обновление токена 
    /// </summary>
    [HttpPost("/refresh")]
    public IActionResult Refresh(DTO.RefreshTokenRequest request) 
    {
        // todo проверка аккаунта
        using var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }
        
        // var account = accountContext.Accounts.FirstOrDefault(acc => acc.Login == request.Login);
        // if(account is null) return ResponseGenerator.Unauthorized();
        
        var (error, account) = DbValueRetriever.RetrieveFromDb(accountContext.Accounts, nameof(ModelAccount.Login), request.Login);
        if (error is not null)
            return error.GetType() == typeof(InvalidOperationException)
                ? ResponseGenerator.Unauthorized()
                : Handlers.HandleException(error);

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

    /// <summary>
    /// Регистрация пользователя в системе
    /// </summary>
    [HttpPost("/register")]
    public IActionResult Register(DTO.RegistrationAccount account)
    {
        // todo возвращение нового аккаунта и пары токенов
        var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }
        
        var newAccount = new ModelAccount
        {
            Name = account.Name, 
            Login = account.Login, 
            PasswordHash = account.PasswordHash
        };
        accountContext.Add(newAccount);

        newAccount = accountContext.Accounts.First(acc => acc.Login == account.Login);

        var responce = new
        {
            account = new DTO.Account(newAccount),
            // access_token = encodedAccessToken,
            // refresh_token = encodedRefreshToken,
        };
        return ResponseGenerator.Ok(value: responce);
    }
}