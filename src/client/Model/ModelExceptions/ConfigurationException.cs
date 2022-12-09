using System;

namespace TFlic.Model.ModelExceptions;

public class ConfigurationException : Exception
{
    public ConfigurationException(string message) : base(message) { }
    public ConfigurationException() { }
}