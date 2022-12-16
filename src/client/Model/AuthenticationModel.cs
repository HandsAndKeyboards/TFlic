using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using TFlic.Model.Infrastructure;
using TFlic.Model.ModelExceptions;
using TFlic.Model.Service;

namespace TFlic.Model;

public class AuthenticationModel
{
    #region Public
    #region Methods
    public static void Authorize(string login, string password)
    {
        var passwordHash = HashPassword(password, SHA256.Create());
        var loginRequest = new AuthorizeRequestDto{Login = login, PasswordHash = passwordHash};

        AuthorizeResponseDto? response = null;
        try
        {
            var responseTask = WebClient.Get.AuthorizeAsync(loginRequest);
            responseTask.Wait();
            response = responseTask.Result;
        }
        catch (AggregateException err)
        {
            ThrowHelper.ThrowAuthenticationException((ApiException) err.InnerException!);
        }

        var storedAccount = ConvertAccountDto(response!.AccountDto, response.Tokens);
        AccountService.SaveAccountToJsonFile(storedAccount);
    }

    // todo приватный? 
    public static bool TryAuthorize(string accessToken)
    {
        
        var response = false;
        try
        {
            var responseTask = WebClient.Get.TryAuthorizeAsync(accessToken);
            responseTask.Wait();
            response = responseTask.Result;
        }
        catch (AggregateException err)
        {
            ThrowHelper.ThrowAuthenticationException((ApiException) err.InnerException!);
        }

        return response;
    }
    
    public static bool TryValidateCredentials()
    {
        StoredAccount? storedAccount = null;
        try { storedAccount = AccountService.ReadAccountFromJsonFile(); }
        catch (JsonException) { return false; }
        catch (ConfigurationException) { return false; }
        
        if (TryAuthorize(storedAccount.Tokens.AccessToken)) { return true; }
            
        try { Refresh(storedAccount.Tokens.RefreshToken, storedAccount.Login); return true; }
        catch (RefreshTokenException) { /* ничего делать не нужно */ }
        
        return false;
    }

    public static void Register(string name, string login, string password)
    {
        var passwordHash = HashPassword(password, SHA256.Create());
        var registerRequest = new RegisterAccountRequestDto{Name = name, Login = login, PasswordHash = passwordHash};

        AuthorizeResponseDto? response = null;
        try
        {
            var responseTask = WebClient.Get.RegisterAsync(registerRequest);
            responseTask.Wait();
            response = responseTask.Result;
        }
        catch (AggregateException err)
        {
            ThrowHelper.ThrowRegistrationException((ApiException) err.InnerException!);
        }
        
        var storedAccount = ConvertAccountDto(response!.AccountDto, response.Tokens);
        AccountService.SaveAccountToJsonFile(storedAccount);
    }

    public static void Refresh(string refreshToken, string login)
    {
        var refreshRequest = new RefreshTokenRequestDto{RefreshToken = refreshToken, Login = login};

        TokenPairDto? response = null;
        try
        {
            var responseTask = WebClient.Get.RefreshAsync(refreshRequest);
            responseTask.Wait();
            response = responseTask.Result;
        }
        catch (AggregateException err)
        {
            ThrowHelper.ThrowRefreshTokenException((ApiException) err.InnerException!);
        }

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