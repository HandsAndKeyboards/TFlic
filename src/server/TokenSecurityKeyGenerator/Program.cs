// See https://aka.ms/new-console-template for more information

/*
 * 
 * При запуске модуль генерирует пару RSA ключей и записывает их в файл в PKCS#8 формате:
 * 
 */

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using TokenSecurityKeyGenerator;
using TokenSecurityKeyGenerator.Config;

var securityKey = new RsaSecurityKey(RSA.Create());
var privateKeyPkcs8 = securityKey.Rsa.ExportPkcs8PrivateKey();

var keys = new RsaKeysDto(privateKeyPkcs8);
try
{
    SaveJsonToFile(JsonSerializer.Serialize(keys));
}
catch (Exception err)
{
    Console.Error.WriteLine($"Во время записи ключа в файл возникло исключение:\n{err.Message}");
    Console.WriteLine("Press enter key to continue");
    Console.ReadLine();
}
Console.WriteLine("Done!\nPress enter key to continue");
Console.ReadLine();



#region LocalFuncs
void SaveJsonToFile(string json, string? path = null)
{
    path ??= ConfigurationUtils.FromConfiguration["key_path"];
    if (path is null) { throw new Exception($"Не удалось открыть файл конфигурации. Возможно, он отсутствует по пути {path}"); }

    using var file = new FileStream(path, FileMode.Create);
    var writeBuffer = Encoding.Default.GetBytes(json);
    file.Write(writeBuffer, 0, writeBuffer.Length);
}
#endregion