using System;

namespace TFlic.Model.Configuration;

public class ConfigurationException : Exception
{
    public ConfigurationException(string message) : base(message) { }
    public ConfigurationException() { }
}