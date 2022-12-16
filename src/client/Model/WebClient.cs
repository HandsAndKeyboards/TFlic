using System;
using System.Net.Http;
using System.Net.Http.Headers;
using TFlic.Model.Config;
using TFlic.Model.ModelExceptions;
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
        var accessToken = AccountService.ReadAccountFromJsonFile().Tokens.AccessToken;
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return new Client(HttpClient);
    }

    private static readonly HttpClient HttpClient = new ();
}