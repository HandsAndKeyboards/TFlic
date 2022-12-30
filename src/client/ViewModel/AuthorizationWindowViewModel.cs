using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TFlic.Model.Authentication.Exceptions;
using TFlic.View;
using TFlic.ViewModel.Constants;
using TFlic.ViewModel.Service;
using AuthenticationManager = TFlic.Model.Authentication.AuthenticationManager;
using RelayCommand = TFlic.ViewModel.Command.RelayCommand;

namespace TFlic.ViewModel
{
    internal class AuthorizationWindowViewModel : Base.ViewModelBase
    {
        #region Поля

        private string _login = string.Empty;

        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        private string _password = string.Empty;

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private string _infoMessage = string.Empty;

        public string InfoMessage
        {
            get => _infoMessage;
            set => Set(ref _infoMessage, value);
        }

        #endregion

        #region Команды

        #region Команда открытия окна регистрации

        public ICommand OpenRegistrationWindowCommand { get; }

        private static void OnOpenRegistrationWindowCommandExecuted(object p) => Task.Run(() =>
        {
            Application.Current.Dispatcher.Invoke(() =>
                {
                    var registrationWindow = new RegistrationWindow();
                    ShowNextWindow(registrationWindow);
                }
            );
        });

        private static bool CanOpenRegistrationWindowCommandExecute(object p) { return true; }

        #endregion

        #region Команда "Войти"

        public ICommand LoginCommand { get; }

        private void OnLoginCommandExecuted(object p) => Task.Run(() =>
        {
            InfoMessage = string.Empty;

            try
            {
                AuthenticationManager.Authorize(Login, Password);
                HandleSuccessLogin();
            }
            catch (Exception err) { InfoMessage = ExceptionUtils.FormExceptionMessage(err); }
        });

        #endregion

        #endregion

        public AuthorizationWindowViewModel()
        {
            // если успешно авторизовались, вызываем обработчик события
            if (AuthenticationManager.TryValidateCredentials()) { HandleSuccessLogin(); } 

            OpenRegistrationWindowCommand =
                new RelayCommand(OnOpenRegistrationWindowCommandExecuted, CanOpenRegistrationWindowCommandExecute);

            LoginCommand = 
                new RelayCommand(OnLoginCommandExecuted);
        }

        private static void HandleSuccessLogin()
        {
            Application.Current.Dispatcher.Invoke(() =>
                {
                    var organizationWindow = new OrganizationWindow();
                    ShowNextWindow(organizationWindow);
                }
            );
        }

        private static void ShowNextWindow(Window window)
        {
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    Application.Current.MainWindow!.Hide();
                    Application.Current.MainWindow = window;
                    Application.Current.MainWindow.Show();
                }
            );
        }
    }
}
