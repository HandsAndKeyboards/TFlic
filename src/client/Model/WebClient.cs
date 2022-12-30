using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using TFlic.Model.Configuration;
using TFlic.Model.Service;

namespace TFlic.Model;

public static class WebClient
{
    public static Client Get => CreateClient();

    static WebClient()
    {
        var baseUri = ConfigurationUtils.FromConfiguration["base_uri"];
        if (baseUri is null) { throw new ConfigurationException($"Не удалось открыть файл конфигурации. Возможно, он отсутствует по пути {string.Join(Constants.FilesystemPaths.ConfigDir, Constants.FilesystemPaths.ConfigName)}"); }
        
        HttpClient.BaseAddress = new Uri(baseUri);
    }

    private static Client CreateClient()
    {
        try
        {
            var accessToken = AccountService.ReadAccountFromJsonFile().Tokens.AccessToken;
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        catch(FileNotFoundException) { }

        return new Client(HttpClient);
    }

    private static readonly HttpClient HttpClient = new ();
}