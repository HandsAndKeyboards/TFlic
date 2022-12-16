using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO;
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
    [HttpPost("/Authorize")]
    public ActionResult<AuthorizeResponseDto> Authorize(AuthorizeRequestDto authorizeRequest)
    {
        using var authInfoContext = DbContexts.Get<AuthInfoContext>();
        
        var authInfo = authInfoContext.Info.Where(
                info => info.Login == authorizeRequest.Login &&
                        info.PasswordHash == authorizeRequest.PasswordHash
            ).Include(info => info.Account)
            .SingleOrDefault();
        if (authInfo is null) { return NotFound(); }
        
        authInfo.Account.UserGroups = authInfo.Account.GetUserGroups();

        var (accessToken, refreshToken) = AuthenticationManager.GenerateTokens(authInfo.Account);
        var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var encodedRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);
        
        // сохраняем сгенерированный refreshToken в базу данных
        authInfo.RefreshToken = encodedRefreshToken; 
        authInfoContext.SaveChanges();
        
        return Ok(new AuthorizeResponseDto(
            new AccountDto(authInfo.Account),
            new TokenPairDto(encodedAccessToken, encodedRefreshToken)
        ));
    }

    /// <summary>
    /// Метод проверяет валидность токена доступа
    /// </summary>
    /// <param name="accessToken">Проверяемый токен доступа</param>
    /// <returns>true в случае, если токен валиден, иначе - false</returns>
    [HttpPost("/TryAuthorize")]
    public ActionResult<bool> TryAuthorize([FromBody] string accessToken)
    {
        return Ok(AuthenticationManager.IsTokenValid(accessToken, AuthenticationManager.TokenType.Access));
    }

    /// <summary>
    /// Обновление токена 
    /// </summary>
    [HttpPost("/Refresh")]
    public ActionResult<TokenPairDto> Refresh(RefreshTokenRequestDto request) 
    {
        using var accountContext = DbContexts.Get<AccountContext>();
        using var authInfoContext = DbContexts.Get<AuthInfoContext>();
        
        var authInfo = authInfoContext.Info
            .Where(info => info.Login == request.Login)
            .Include(info => info.Account)
            .SingleOrDefault();
        if (authInfo is null) { return NotFound(); }

        var refreshTokenValid = AuthenticationManager.IsTokenValid(
            request.RefreshToken,
            AuthenticationManager.TokenType.Refresh
        );
        if (!refreshTokenValid) { return Unauthorized(); }

        var (accessToken, refreshToken) = AuthenticationManager.GenerateTokens(authInfo.Account);
        var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
        var encodedRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);
        
        authInfo.RefreshToken = encodedRefreshToken;
        authInfoContext.SaveChanges();
        
        return Ok(new TokenPairDto(encodedAccessToken, encodedRefreshToken));
    }

    /// <summary>
    /// Регистрация пользователя в системе
    /// </summary>
    [HttpPost("/Register")]
    public ActionResult<AuthorizeResponseDto> Register(RegisterAccountRequestDto account)
    {
        var authInfoContext = DbContexts.Get<AuthInfoContext>();
        if (authInfoContext.Info.Any(info => info.Login == account.Login)) { return BadRequest("login already in use"); }
        
        var accountContext = DbContexts.Get<AccountContext>();
        
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
        
        return Ok(new AuthorizeResponseDto(
            new AccountDto(newAccount), 
            new TokenPairDto(encodedAccessToken, encodedRefreshToken)
        ));
    }
}