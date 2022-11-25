using System.IdentityModel.Tokens.Jwt;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.Service;
using Organization.Models.Authentication;
using Organization.Models.Contexts;
using ControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;

namespace Organization.Controllers;

using ModelAccount = Models.Organization.Accounts.Account;

[ApiController]
public class AuthenticationController : ControllerBase
{
    /// <summary>
    /// Аутентификация и авторизация пользователя
    /// </summary>
    [Microsoft.AspNetCore.Mvc.HttpPost("/login")]
    public IActionResult Login(DTO.AuthenticationAccount authAccount)
    {
        using var authInfoContext = DbContexts.Get<AuthInfoContext>();
        if (authInfoContext is null) { return Handlers.HandleNullDbContext(typeof(AuthInfoContext)); }
        
        AuthInfo? authInfo;
        try
        {
            authInfo = authInfoContext.Info.Where(
                    info => info.Login == authAccount.Login &&
                            info.PasswordHash == authAccount.PasswordHash
                ).Include(info => info.Account)
                .Single();
        }
        catch (InvalidOperationException) { return ResponseGenerator.Unauthorized(message: "Login or password is incorrect"); }
        catch (Exception err) { return Handlers.HandleException(err); }

        var (accessToken, refreshToken) = AuthenticationManager.GenerateTokens(authInfo.Account);
        var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var encodedRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);

        authInfo.RefreshToken = encodedRefreshToken;
        authInfoContext.SaveChanges();

        return ResponseGenerator.Ok(value: new DTO.TokenPair(encodedAccessToken, encodedRefreshToken));
    }

    /// <summary>
    /// Обновление токена 
    /// </summary>
    [Microsoft.AspNetCore.Mvc.HttpPost("/refresh")]
    public IActionResult Refresh(DTO.RefreshTokenRequest request) 
    {
        // todo проверка аккаунта
        using var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }
        
        using var authInfoContext = DbContexts.Get<AuthInfoContext>();
        if (authInfoContext is null) { return Handlers.HandleNullDbContext(typeof(AuthInfoContext)); }

        AuthInfo? authInfo;
        try
        {
            authInfo = authInfoContext.Info
                .Where(info => info.Login == request.Login)
                .Include(info => info.Account)
                .Single();
        }
        catch (ArgumentNullException) { return Handlers.HandleElementNotFound(nameof(ModelAccount), request.Login); }
        catch (InvalidOperationException) { return ResponseGenerator.Unauthorized(); }

        var refreshTokenValid = AuthenticationManager.IsTokenValid(
            request.RefreshToken,
            AuthenticationManager.TokenType.Refresh
        );
        if (!refreshTokenValid) { return Unauthorized("refresh token is invalid"); }

        var (accessToken, refreshToken) = AuthenticationManager.GenerateTokens(authInfo!.Account!);
        var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var encodedRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);
        
        authInfo.RefreshToken = encodedRefreshToken;
        authInfoContext.SaveChanges();
        
        return ResponseGenerator.Ok(value: new DTO.TokenPair(encodedAccessToken, encodedRefreshToken));
    }

    /// <summary>
    /// Регистрация пользователя в системе
    /// </summary>
    [Microsoft.AspNetCore.Mvc.HttpPost("/register")]
    public IActionResult Register(DTO.RegistrationAccount account)
    {
        var accountContext = DbContexts.Get<AccountContext>();
        if (accountContext is null) { return Handlers.HandleNullDbContext(typeof(AccountContext)); }
        
        var newAccount = new ModelAccount
        {
            Name = account.Name,
            AuthInfo = new AuthInfo
            {
                Login = account.Login, 
                PasswordHash = account.PasswordHash
            }
        };
        newAccount = accountContext.Add(newAccount).Entity;
        accountContext.SaveChanges();
        
        var (accessToken, refreshToken) = AuthenticationManager.GenerateTokens(newAccount);
        var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var encodedRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);

        newAccount.AuthInfo.RefreshToken = encodedRefreshToken;
        accountContext.SaveChanges();
        
        var response = new DTO.RegisterAccountResponse(
            new DTO.Account(newAccount), 
            new DTO.TokenPair(encodedAccessToken, encodedRefreshToken)
        );
        return ResponseGenerator.Ok(value: response);
    }
}