using System;

namespace TFlic.Model.Authentication.Exceptions;

public class RegistrationException : AuthenticationModelException
{
    public RegistrationException(AuthenticationExceptionState state, Exception? innerException = null) 
        : base(state, innerException) { }
}