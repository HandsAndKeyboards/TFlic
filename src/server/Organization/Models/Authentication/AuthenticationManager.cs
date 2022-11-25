using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Organization.Models.Contexts;
using Organization.Models.Organization.Accounts;

namespace Organization.Models.Authentication;

public static class AuthenticationManager
{
    #region Public
    public enum TokenType { Access, Refresh }
    
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
    /// Аутентификация пользователя на основе токена и указанной политики 
    /// </summary>
    /// <param name="encodedToken">Закодированыый в base64 Jwt токен</param>
    /// <param name="allowedOrganization">Организация, участникам которой разрешен доступ к методу</param>
    /// <param name="adminRequired">ture, если для выполнения операции требются права администратора, иначе false</param>
    /// <param name="allowNoRole">true, если операцию может выполнить даже пользователь без роли, иначе false</param>
    /// <returns>true, если пользователю предоставляется доступ, иначе - false</returns>
    public static bool Authorize(string encodedToken, ulong allowedOrganization, bool adminRequired = false, bool allowNoRole = false)
    {
        if (encodedToken.IsNullOrEmpty()) { throw new ArgumentNullException(encodedToken); }
        
        // декожируем JWT токен
        JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(encodedToken);
        
        // организации и их группы пользователей, в которых состоит пользователь
        var rolesClaim = token.Claims.Single(claim => claim.Type == "roles").Value;
        var orgUserGroups = JsonSerializer.Deserialize<AccountsUserGroups>(rolesClaim);
        if (orgUserGroups is null) { throw new InvalidOperationException("Не удалось десериализовать организации и группы пользователей"); }

        // если пользователь не состоит в разрешенной организации, возвращаем false
        if (!orgUserGroups.ContainsOrganization(allowedOrganization)) { return false; }

        // если дозволен доступ пользователю без роли, возвращаем true без проверки на участника проекта или админа
        if (allowNoRole) { return true; }

        // декодируем группы пользователей в массив
        var userGroups = AccountsUserGroups.Decode(orgUserGroups.UserGroups[allowedOrganization]);

        // если пользователь является админом разрешенной группы - возвращаем true
        if (adminRequired && userGroups.Contains((short) Organization.Organization.PrimaryUserGroups.Admins)) { return true; }

        // если дозволен доступ участникам проекта и пользователь является участником
        if (userGroups.Contains((short) Organization.Organization.PrimaryUserGroups.ProjectsMembers)) { return true; }

        // если пользователь не является ни админом, ни участником, то ему доступ запрещен
        return false;
    }
    
    /// <summary>
    /// Генерация Access и Refresh токеов
    /// </summary>
    public static (JwtSecurityToken access, JwtSecurityToken refresh) GenerateTokens(Account account)
    {
        var accessToken = GenerateAccessToken(account);
        var refreshToken = GenerateRefreshToken(account);
        
        return (accessToken, refreshToken);
    }

    /// <summary>
    /// Метод определяет валидность токена
    /// </summary>
    /// <param name="token">закодированный в base64 Jwt токен</param>
    /// <param name="tokenType">Тип токена</param>
    /// <returns>true, если токен валиден, иначе false</returns>
    /// <exception cref="ArgumentNullException">Возникает, если токен null или пустой</exception>
    public static bool IsTokenValid(string token, TokenType tokenType)
    {
        if (token.IsNullOrEmpty()) { throw new ArgumentNullException(token); }
        
        var tokenValidationParameters = tokenType is TokenType.Access
            ? AccessTokenValidationParameters
            : RefreshTokenValidationParameters;
        
        var jwtHandler = new JwtSecurityTokenHandler();
        var validationResult = jwtHandler.ValidateTokenAsync(token, tokenValidationParameters);

        bool isValid = validationResult.Result.IsValid;
        // refresh токен невалиден, если не записан в базе данных 
        if (tokenType is TokenType.Refresh && isValid) { isValid = isValid && IsRefreshTokenInDatabase(token); }

        return isValid;
    }
    #endregion
    #endregion



    #region Private
    #region Methods

    /// <summary>
    /// Генерация Access Token на основе данных о пользователе
    /// </summary>
    /// <param name="account">Данные о затребовавшем токен аккаунте</param>
    /// <returns>Сгенерированный Access Token</returns>
    /// <exception cref="ArgumentNullException">Генерируется в случае, если account == null</exception>
    private static JwtSecurityToken GenerateAccessToken(Account account)
    {
        if (account is null) { throw new ArgumentNullException(nameof(account)); }

        // var roles = new List<Dictionary<ulong, byte>>();
        var claims = new List<Claim>
        {
            new ("roles", JsonSerializer.Serialize(new AccountsUserGroups(account)))
        }; 
        
        var accessToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: account.Id.ToString(),
            claims: claims, 
            expires: DateTime.UtcNow.Add(AccessTokenLifetime), 
            signingCredentials: new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha256)
        );
        
        return accessToken;
    }

    /// <summary>
    /// Генерация Access Token на основе данных о пользователе
    /// </summary>
    /// <param name="account">Данные о затребовавшем токен аккаунте</param>
    /// <returns>Сгенерированный Access Token</returns>
    /// <exception cref="ArgumentNullException">Генерируется в случае, если account == null</exception>
    private static JwtSecurityToken GenerateRefreshToken(Account account)
    {
        if (account is null) { throw new ArgumentNullException(nameof(account)); }
        
        var refreshToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: account.Id.ToString(),
            expires: DateTime.UtcNow.Add(RefreshTokenLifetime),
            signingCredentials: new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha256)
        );

        return refreshToken;
    }

    /// <summary>
    /// Проверка нахождения refresh токена в базе данных
    /// </summary>
    /// <param name="encodedRefreshToken">Закодированный в base64 Jwt refresh токен</param>
    /// <returns>true, если токен находится в базе данных, иначе false</returns>
    /// <exception cref="InvalidCastException">Возникает, если не удается прочитать записанный в токене id получателя</exception>
    private static bool IsRefreshTokenInDatabase(string encodedRefreshToken)
    {
        var refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(encodedRefreshToken);
        var audience = refreshToken.Audiences.ElementAt(0);
        if (!ulong.TryParse(audience, out var audienceId)) { throw new InvalidCastException($"Не удается преобразовать Id к ulong: {audience}"); }
        
        using var authInfoContext = DbContexts.GetNotNull<AuthInfoContext>();
        
        return authInfoContext.Info
            .Single(info => info.AccountId == audienceId)
            .RefreshToken == encodedRefreshToken;
    }
    #endregion
    
    #region Fields
    private static readonly AsymmetricSecurityKey SecurityKey = new RsaSecurityKey(RSA.Create());
    private static readonly string Issuer = "localhost"; // todo записать что-то нормальное
    private static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromSeconds(300);
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromSeconds(500); 
    #endregion
    #endregion
}