using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TFlic.Model.Constants;
using TFlic.Model.DTO;
using TFlic.Model.Infrastructure;
using TFlic.Model.Service;

namespace TFlic.Model;

public class AuthenticationModel
{
    #region Public
    #region Methods
    public async Task<HttpResult> Authorize(string login, string password)
    {
        var passwordHash = HashPassword(password, SHA256.Create());

        var loginRequest = new LoginRequest(login, passwordHash);
        var content = JsonContent.Create(loginRequest);
        
        using var response = await _httpClient.PostAsync(Uris.LoginUri, content);
        var result = new HttpResult(response.StatusCode);
        if (response.StatusCode != HttpStatusCode.OK) { return result; }
        
        var responseValue = await response.Content.ReadFromJsonAsync<Response<TokenPair>>();
        if (responseValue is null) { throw new InvalidOperationException("Не удалось прочитать токены из Http ответа"); }

        var tokens = responseValue.Value;
        TokenService.SaveTokensToJsonFile(tokens!);

        result.Message = responseValue?.Message;
        return result;
    }

    public async Task<HttpResult> Register(string name, string login, string password)
    {
        var passwordHash = HashPassword(password, SHA256.Create());

        var registerRequest = new RegisterAccountRequest(name, login, passwordHash);
        var content = JsonContent.Create(registerRequest);

        using var response = await _httpClient.PostAsync(Uris.RegisterUri, content);
        var result = new HttpResult(response.StatusCode);
        if (response.StatusCode != HttpStatusCode.OK) { return result; }

        var responseValue = await response.Content.ReadFromJsonAsync<Response<AccountWithTokens>>();
        if (responseValue is null) { throw new InvalidOperationException("Не удалось прочитать Http ответ"); }

        var account = responseValue?.Value.Account; // todo поиспользовать аккаунт
        var tokens = responseValue?.Value.Tokens;
        TokenService.SaveTokensToJsonFile(tokens!);

        result.Message = responseValue?.Message;
        return new HttpResult(response.StatusCode);
    }

    public async Task<HttpResult> Refresh(string refreshToken, string login)
    {
        // todo протестировать
        var refreshRequest = new RefreshTokenRequest(refreshToken, login);
        var content = JsonContent.Create(refreshRequest);

        using var response = await _httpClient.PostAsync(Uris.RegisterUri, content);
        var result = new HttpResult(response.StatusCode);
        if (response.StatusCode != HttpStatusCode.OK) { return result; }
        
        var responseValue = await response.Content.ReadFromJsonAsync<Response<TokenPair>>();
        if (responseValue is null) { throw new InvalidOperationException("Не удалось прочитать токены из Http ответа"); }

        var tokens = responseValue.Value;
        TokenService.SaveTokensToJsonFile(tokens!);

        result.Message = responseValue?.Message;
        return new HttpResult(response.StatusCode);
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
    #endregion

    #region Fields
    private readonly HttpClient _httpClient = new();
    #endregion
    #endregion
}