using System;
using System.Threading.Tasks;
using System.Windows;
using TFlic.ViewModel.Constants;
using TFlic.Model.Authentication.Exceptions;

namespace TFlic.ViewModel.Service;

public static class ExceptionUtils
{
    public static void HandleException(Exception err)
    {
        HandleException(FormExceptionMessage(err));
#if DEBUG
        MessageBox.Show($"{err.Message}\n{err.InnerException?.Message}", "Debug error description");
#endif
    }

    public static void HandleException(string message) =>
        MessageBox.Show(message, "Ошибка");

    public static string FormExceptionMessage(Exception err) =>
        err switch
        {
            AuthenticationModelException authException  => FormAuthenticationModelExceptionMessage(authException),
            TaskCanceledException => ErrorMessages.TimeoutMessage, 
            TimeoutException => ErrorMessages.TimeoutMessage, 
            _ =>  ErrorMessages.UnexpectedError,
        };

    private static string FormAuthenticationModelExceptionMessage(AuthenticationModelException err) =>
        err.State switch
        {
            AuthenticationExceptionState.AuthenticationExpired => ErrorMessages.InvalidCredentials,
            AuthenticationExceptionState.IncorrectCredentials => ErrorMessages.InvalidCredentials,
            _ => ErrorMessages.UnexpectedError,
        };
}