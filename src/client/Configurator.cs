using System.IO;
using Microsoft.Extensions.Configuration;

namespace TFlic;

public class Configurator
{
    private IConfigurationRoot _config;
    
    public Configurator()
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Constants.configDir)
                .AddJsonFile(Constants.configName);
        _config = builder.Build();
    }
    
    public string? this[string key] => _config[key];
}