using System;
using TFlic.Model.Exceptions;

namespace TFlic.Model.Authentication.Exceptions;

public class AuthenticationModelException : ModelException
{
    public AuthenticationModelException(AuthenticationExceptionState state, Exception? innerException = null) 
        : base(message: null, innerException: innerException)
    {
        State = state;
    }

    public AuthenticationExceptionState State { get; }
}

public enum AuthenticationExceptionState
{
    IncorrectCredentials,
    AuthenticationExpired,
    UnexpectedError,
}