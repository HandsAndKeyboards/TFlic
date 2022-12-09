using System;

namespace TFlic.Model.ModelExceptions;

public class RefreshTokenException : Exception
{
    public RefreshTokenException(string message) : base(message) { }
}