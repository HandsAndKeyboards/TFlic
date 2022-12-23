using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using TFlic.Model.Authentication.Exceptions;
using TFlic.Model.Configuration;
using TFlic.Model.Exceptions;
using TFlic.Model.Service;

namespace TFlic.Model.Authentication;

public static class AuthenticationManager
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
            if (err.InnerException!.GetType() == typeof(ApiException))
                ThrowHelper.ThrowAuthenticationException((ApiException) err.InnerException!);
            else if (err.InnerException!.GetType() == typeof(HttpRequestException))
                ThrowHelper.ThrowTimeoutException((HttpRequestException) err.InnerException!);
        }

        var storedAccount = ConvertAccountDto(response!.AccountDto, response.Tokens);
        AccountService.SaveAccountToJsonFile(storedAccount);
    }

    public static bool TryValidateCredentials()
    {
        StoredAccount? storedAccount = null;
        try { storedAccount = AccountService.ReadAccountFromJsonFile(); }
        catch (JsonException) { return false; }
        catch (ConfigurationException) { return false; }
        catch (FileNotFoundException) { return false; }
        
        if (TryAuthorize(storedAccount.Tokens.AccessToken)) { return true; }
            
        try { Refresh(storedAccount.Tokens.RefreshToken, storedAccount.Login); return true; }
        catch (RefreshException) { /* ничего делать не нужно */ }
        catch (TimeoutException) { /* ничего делать не нужно */ }
        // catch (Exception err) { throw new Exception(); }
        
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
            if (err.InnerException!.GetType() == typeof(ApiException))
                ThrowHelper.ThrowRegistrationException((ApiException) err.InnerException!);
            else if (err.InnerException!.GetType() == typeof(HttpRequestException))
                ThrowHelper.ThrowTimeoutException((HttpRequestException) err.InnerException!);
            
            // ThrowHelper.ThrowRegistrationException((ApiException) err.InnerException!);
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
            if (err.InnerException!.GetType() == typeof(ApiException))
                ThrowHelper.ThrowRefreshTokenException((ApiException) err.InnerException!);
            else if (err.InnerException!.GetType() == typeof(HttpRequestException))
                ThrowHelper.ThrowTimeoutException((HttpRequestException) err.InnerException!);
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
    
    private static bool TryAuthorize(string accessToken)
    {
        try
        {
            var responseTask = WebClient.Get.TryAuthorizeAsync(accessToken);
            responseTask.Wait();
            return responseTask.Result;
        }
        catch (AggregateException) { /* ничего делать не нужно */ }

        return false;
    }
    #endregion
    #endregion
}