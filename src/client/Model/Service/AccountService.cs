using System.IO;
using System.Text;
using System.Text.Json;
using TFlic.Model.Config;
using TFlic.Model.Infrastructure;
using TFlic.Model.ModelExceptions;

namespace TFlic.Model.Service;

public static class AccountService
{
    /// <summary>
    /// Метод читает информацию об аккаунте из json файла
    /// </summary>
    /// <param name="filePath">Путь к файлу, в котором будет сохранен аккаунт</param>
    /// <returns>Прочитанный из json файла аккаунт</returns>
    /// <exception cref="ConfigurationException">Генерируется при неудачной попытке открыть файл конфигурации</exception>
    /// <exception cref="JsonException">Не удалось десериализовать сождержимое файла</exception>
    public static StoredAccount ReadAccountFromJsonFile(string? filePath = null)
    {
        filePath ??= ConfigurationUtils.FromConfiguration["account_file_path"];
        if (filePath is null) { throw new ConfigurationException($"Не удалось открыть файл конфигурации. Возможно, он отсутствует по пути {filePath}"); }

        if (!Path.Exists(Path.GetFullPath(filePath))) { throw new FileNotFoundException($"Не удалось открыть файл {filePath}. Возможно, его не существует"); }
        
        using var file = new FileStream(filePath, FileMode.Open);

        var readBuffer = new byte[file.Length];
        _ = file.Read(readBuffer, 0, readBuffer.Length);
        var account = JsonSerializer.Deserialize<StoredAccount>(Encoding.Default.GetString(readBuffer));

        return account ?? throw new JsonException($"Не удалось десериализовать содержимое файла\n{filePath}");
    }
    
    /// <summary>
    /// Метод сохраняет аккаунт в json файл
    /// </summary>
    /// <param name="account">Аккаунт, который нужно сохранить</param>
    /// <param name="filePath">Путь к файлу, в котором будет сохранен аккаунт</param>
    /// <exception cref="ConfigurationException">Генерируется при неудачной попытке открыть файл конфигурации</exception>
    public static void SaveAccountToJsonFile(StoredAccount account, string? filePath = null)
    {
        filePath ??= ConfigurationUtils.FromConfiguration["account_file_path"];
        if (filePath is null) { throw new ConfigurationException($"Не удалось открыть файл конфигурации. Возможно, он отсутствует по пути {filePath}"); } 

        var jsonAccount = JsonSerializer.Serialize(account);
        
        using var file = new FileStream(filePath, FileMode.Create);
        var writeBuffer = Encoding.Default.GetBytes(jsonAccount);
        file.Write(writeBuffer, 0, writeBuffer.Length);
    }

    /// <summary>
    /// Метод обновляет сохраненные в json файле токены
    /// </summary>
    /// <param name="newTokens">Новая пара токенов</param>
    /// <param name="filePath">Путь к файлу, в котором будет сохранен аккаунт</param>
    /// <exception cref="ConfigurationException">Генерируется при неудачной попытке открыть файл конфигурации</exception>
    public static void UpdateTokensInJson(TokenPair newTokens, string? filePath = null)
    {
        filePath ??= ConfigurationUtils.FromConfiguration["account_file_path"];
        if (filePath is null) { throw new ConfigurationException($"Не удалось открыть файл конфигурации. Возможно, он отсутствует по пути {filePath}"); }

        var account = ReadAccountFromJsonFile(filePath);
        account.Tokens = newTokens;
        SaveAccountToJsonFile(account, filePath);
    }
}