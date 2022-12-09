using System.Net.Http;
using TFlic.Model.Constants;

namespace TFlic.Model;

public static class WebClient
{
    public static Client Get { get; } = new (Uris.BaseUrl, new HttpClient());
}