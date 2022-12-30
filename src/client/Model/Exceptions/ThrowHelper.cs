using System;
using System.Net.Http;
using TFlic.Model.Authentication.Exceptions;

namespace TFlic.Model.Exceptions;

public static class ThrowHelper
{
    public static void ThrowAuthenticationException(ApiException err)
    {
        var state = err.StatusCode switch
        {
            404 => AuthenticationExceptionState.IncorrectCredentials,
            _ => AuthenticationExceptionState.UnexpectedError
        };
        
        throw new AuthenticationModelException(state, err);
    }
    
    public static void ThrowTimeoutException(HttpRequestException err)
    {
        throw new TimeoutException(err.Message);
    }
    
    public static void ThrowRegistrationException(ApiException err)
    {
        throw new RegistrationException(AuthenticationExceptionState.UnexpectedError, err);
    }

    public static void ThrowRefreshTokenException(ApiException err)
    {
        var state = err.StatusCode switch
        {
            401 => AuthenticationExceptionState.AuthenticationExpired,
            _ => AuthenticationExceptionState.UnexpectedError
        };
        throw new RefreshException(state, err);
    }
}