using TFlic.Model.ModelExceptions;

namespace TFlic.Model;

public static class ThrowHelper
{
    public static void ThrowAuthenticationException(ApiException err)
    {
        var message = err.StatusCode switch
        {
            404 => "Неверный логин или пароль",
            _ => "Произошла ошибка, попробуйте позже"
        };

        throw new AuthenticationModelException(message);
    }
    
    public static void ThrowRegistrationException(ApiException err)
    {
        throw new RegistrationException(err.Message);
    }

    public static void ThrowRefreshTokenException(ApiException err)
    {
        var message = err.StatusCode switch
        {
            401 => "Срок действия авторизации истек истек",
            _ => "Произошла непредвиденная ошибка"
        };

        throw new RefreshTokenException(message);
    }
}