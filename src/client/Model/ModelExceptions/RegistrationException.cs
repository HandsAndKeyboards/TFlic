using System;

namespace TFlic.Model.ModelExceptions;

public class RegistrationException : Exception
{
    public RegistrationException(string message) : base(message) { }
}