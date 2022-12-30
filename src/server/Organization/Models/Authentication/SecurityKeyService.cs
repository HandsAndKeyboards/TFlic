using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Organization.Models.Config;
using Organization.Models.Constants;

namespace Organization.Models.Authentication;

public static class SecurityKeyService
{
    /// <summary>
    /// Чтение пары RSA public/private ключей из Json файла
    /// </summary>
    /// <param name="filepath">Путь к файлу с ключами. Если не указан, путь извлекается из файла конфигурации</param>
    /// <returns>Объект RSA с установленными прочитанными ключами</returns>
    /// <exception cref="ConfigurationException">Возникает в случае неудачной попытки открыть файл с конфигурацией</exception>
    /// <remarks>Ключи должны быть представлены в фармате PKCS#8</remarks>
    public static RSA ReadKeysFromJson(string? filepath = null)
    {
        filepath ??= FilesystemPaths.KeysFullPath; 
        if (filepath is null) { throw new ConfigurationException($"Не удалось открыть файл конфигурации. Возможно, он отсутствует по пути {string.Join(FilesystemPaths.ConfigDir, FilesystemPaths.ConfigName)}"); }
        
        
        RsaKeysDto rsaKeys;
        try { rsaKeys = ReadKeysFromJsonFile(filepath); }
        catch (FileNotFoundException) { throw; /* todo организовать логгирование */ }
        
        var rsa = RSA.Create();
        try { rsa.ImportPkcs8PrivateKey(rsaKeys.PrivateKeyPkcs8, out _); }
        catch (CryptographicException) { throw; /* todo организовать логгирование */ }

        return rsa;
    }

    private static RsaKeysDto ReadKeysFromJsonFile(string filepath)
    {
        if (!File.Exists(filepath)) { throw new FileNotFoundException($"Файл {filepath} не найден. Возможно, его не существует"); }
        
        using var file = new FileStream(filepath, FileMode.Open);

        var readBuffer = new byte[file.Length];
        _ = file.Read(readBuffer, 0, readBuffer.Length);
        var keys = JsonSerializer.Deserialize<RsaKeysDto>(Encoding.Default.GetString(readBuffer));

        return keys ?? throw new JsonException($"Не удалось десериализовать содержимое файла\n{filepath}");
    }
}