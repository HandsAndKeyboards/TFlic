using System.Net.Http;
using TFlic.Model.Config;

namespace TFlic.Model;

public static class WebClient
{
    public static Client Get { get; } = new (ConfiguratorUtils.FromConfiguration["base_uri"], new HttpClient());
}