using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TFlic.Model.Infrastructure;
using TFlic.Model.Service;

namespace TFlic.Model;

public class AuthenticationModel
{
    #region Public
    #region Methods
    public static async Task Authorize(string login, string password)
    {
        var passwordHash = HashPassword(password, SHA256.Create());
        var loginRequest = new AuthorizeRequestDto{Login = login, PasswordHash = passwordHash};

        AuthorizeResponseDto? response = null;
        try { response = await WebClient.Get.AuthorizeAsync(loginRequest); }
        catch (ApiException err) { ThrowHelper.ThrowAuthenticationException(err); }

        var storedAccount = ConvertAccountDto(response!.AccountDto, response.Tokens);
        AccountService.SaveAccountToJsonFile(storedAccount);
    }

    public static async Task Register(string name, string login, string password)
    {
        var passwordHash = HashPassword(password, SHA256.Create());
        var registerRequest = new RegisterAccountRequestDto{Name = name, Login = login, PasswordHash = passwordHash};

        AuthorizeResponseDto? response = null;
        try { response = await WebClient.Get.RegisterAsync(registerRequest); }
        catch (ApiException err) { ThrowHelper.ThrowRegistrationException(err); }
        
        var storedAccount = ConvertAccountDto(response!.AccountDto, response.Tokens);
        AccountService.SaveAccountToJsonFile(storedAccount);
    }

    public static async Task Refresh(string refreshToken, string login)
    {
        var refreshRequest = new RefreshTokenRequestDto{RefreshToken = refreshToken, Login = login};

        TokenPairDto? response = null;
        try { response = await WebClient.Get.RefreshAsync(refreshRequest); }
        catch (ApiException err) { ThrowHelper.ThrowRefreshTokenException(err); }

        var tokenPair = new TokenPair {AccessToken = response!.AccessToken, RefreshToken = response.RefreshToken};
        AccountService.UpdateTokensInJson(tokenPair);
    }
    #endregion

    #region Properties
    #endregion
    #endregion



    #region Private
    #region Methods
    /// <summary>
    /// Метод вычисляет хеш пароля заданнмы алгоритмом и возвращает закодированный в base64 хеш
    /// </summary>
    private static string HashPassword(string password, HashAlgorithm hashAlgorithm)
    {
        var passwordHash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
        var encodedPasswordHash = Convert.ToBase64String(passwordHash);

        return encodedPasswordHash;
    }

    private static StoredAccount ConvertAccountDto(AccountDto accountDto, TokenPairDto tokens)
    {
        var storedAccount = new StoredAccount
        {
            Id = (uint) accountDto.Id,
            Login = accountDto.Login,
            Name = accountDto.Name,

            Tokens = new TokenPair {AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken},
        };

        return storedAccount;
    }
    #endregion
    #endregion
}