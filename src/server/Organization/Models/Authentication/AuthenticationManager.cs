using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Authentication;

public static class AuthenticationManager
{
    #region Public
    #region Properties
    /// <summary>
    /// Пареметры проверки Access токена
    /// </summary>
    public static TokenValidationParameters AccessTokenValidationParameters { get; }

    /// <summary>
    /// Пареметры проверки Refresh токена
    /// </summary>
    public static TokenValidationParameters RefreshTokenValidationParameters { get; }
    #endregion

    #region Methods
    static AuthenticationManager()
    {
        /*
         * todo при переносе в claims запихнуть Id и Роль пользователя,
         * todo в параметры валидации добавить проверку этих данных (нужен слой взаимодействия с базой данных)
         */

        AccessTokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidAlgorithms = new [] {"RS256"},
            IssuerSigningKey = SecurityKey,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
        };

        RefreshTokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidAlgorithms = new [] {"RS256"},
            IssuerSigningKey = SecurityKey,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
        };
    }
    
    /// <summary>
    /// Генерация Access и Refresh токеов
    /// </summary>
    public static (JwtSecurityToken access, JwtSecurityToken refresh) GenerateTokens(Account account, string remoteIp)
    {
        var accessToken = GenerateAccessToken(account, remoteIp);
        var refreshToken = GenerateRefreshToken(account, remoteIp);
        
        return (accessToken, refreshToken);
    }

    public static bool IsAudienceIpValid(string requesterIp, string encodedToken)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(encodedToken);
        var audienceIp = token.Payload.Aud[0];
        if (audienceIp is null) { throw new AuthenticationException("audience ip not found"); }
        
        return requesterIp == audienceIp;
    }

    public static bool IsTokenValid(string token, TokenValidationParameters validationParameters)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var validationResult = jwtHandler.ValidateTokenAsync (token, validationParameters);

        return validationResult.Result.IsValid;
    }
    #endregion
    #endregion



    #region Private
    #region Methods
    /// <summary>
    /// Генерация Access Token на основе данных о пользователе и IP адреса, с которого пришел запрос
    /// </summary>
    /// <param name="account">Данные о затребовавшем токен аккаунте</param>
    /// <param name="remoteIp">IP адрес, затребовавший Access Token</param>
    /// <returns>Сгенерированный Access Token</returns>
    /// <exception cref="NullReferenceException">Генерируется</exception>
    private static JwtSecurityToken GenerateAccessToken(Account account, string remoteIp)
    {
        if (account is null) { throw new NullReferenceException("Account cannot be null"); }
        
        /*
         * todo
         * в claims записать роли пользователя в организациях:
         * {
         *     "org1" : [1, 2],
         *     "org2": [0]...
         * }
         * в массивах записаны локальные id групп пользователей
         */
        var claims = new List<Claim>
        {
            // new ("id", account.Id.ToString(), ClaimValueTypes.UInteger64),
            // new (ClaimTypes.Role, account.Role.ToString(), ClaimValueTypes.UInteger64)
        }; 
        
        var accessToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: remoteIp,
            claims: claims, 
            // notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.Add(AccessTokenLifetime), 
            signingCredentials: new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha256)
        );
        
        return accessToken;
    }

    /// <summary>
    /// Генерация Access Token на основе данных о пользователе и IP адреса, с которого пришел запрос
    /// </summary>
    /// <param name="account">Данные о затребовавшем токен аккаунте</param>
    /// <param name="remoteIp">IP адрес, затребовавший Access Token</param>
    /// <returns>Сгенерированный Access Token</returns>
    /// <exception cref="NullReferenceException">Генерируется</exception>
    private static JwtSecurityToken GenerateRefreshToken(Account account, string remoteIp)
    {
        if (account is null) { throw new NullReferenceException("account cannot be null"); }
                
        /*
         * todo
         * в claims записать роли пользователя в организациях:
         * {
         *     "org1" : [1, 2],
         *     "org2": [0]...
         * }
         * в массивах записаны локальные id групп пользователей
         */
        var claims = new List<Claim> // todo
        {
            // new (ClaimTypes.Name, account.Email)
        }; 
        var refreshToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: remoteIp,
            claims: claims,
            // notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.Add(RefreshTokenLifetime),
            signingCredentials: new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha256)
        );

        return refreshToken;
    }
    #endregion
    
    #region Fields
    private static readonly AsymmetricSecurityKey SecurityKey = new RsaSecurityKey(RSA.Create());
    private static readonly string Issuer = "localhost";
    private static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromSeconds(15); // в минутах
    #endregion
    #endregion
}