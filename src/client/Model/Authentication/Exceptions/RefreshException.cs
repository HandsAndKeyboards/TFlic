using System;

namespace TFlic.Model.Authentication.Exceptions;

public class RefreshException : AuthenticationModelException
{
    public RefreshException(AuthenticationExceptionState state, Exception? innerException = null) 
        : base(state, innerException) { }
}