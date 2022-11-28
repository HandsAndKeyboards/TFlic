using System.IO;
using System.Text;
using System.Text.Json;
using TFlic.Model.Infrastructure;

namespace TFlic.Model.Service;

public static class TokenService
{
    public enum TokenType { AccessToken, RefreshToken }
    
    public static string ReadTokenFromJson(TokenType tokenType, string filePath = Constants.FilesystemPaths.TokensFilePath)
    {
        using var file = new FileStream(filePath, FileMode.Open);
        
        var readBuffer = new byte[file.Length];
        _ = file.Read(readBuffer, 0, readBuffer.Length);
        var tokens = JsonSerializer.Deserialize<TokenPair>(Encoding.Default.GetString(readBuffer));

        if (tokens is null) { throw new JsonException($"Не удалось десериализовать соержимое файла\n{filePath}"); }

        return tokenType == TokenType.AccessToken
            ? tokens.AccessToken
            : tokens.RefreshToken;
    }
    
    /// <summary>
    /// Метод сохраняет токены аутентификации в файл
    /// </summary>
    /// <param name="tokens">Пара токенов (Access token, Refresh token)</param>
    /// <param name="filePath">Путь к файлу, в котором будут сохранены токены</param>
    public static void SaveTokensToJsonFile(TokenPair tokens, string filePath = Constants.FilesystemPaths.TokensFilePath)
    {
        var jsonTokens = JsonSerializer.Serialize(tokens);
        
        using var file = new FileStream(filePath, FileMode.Create);
        var writeBuffer = Encoding.Default.GetBytes(jsonTokens);
        file.Write(writeBuffer, 0, writeBuffer.Length);
    }
}