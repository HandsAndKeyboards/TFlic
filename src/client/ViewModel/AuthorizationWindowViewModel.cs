using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TFlic.Model;
using TFlic.Model.ModelExceptions;
using TFlic.ViewModel.Command;
using TFlic.View;

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

        private readonly AuthenticationModel _authModel = new();

        #endregion

        #region Команды

        #region Команда открытия окна регистрации

        public ICommand OpenRegistrationWindowCommand { get; }

        private void OnOpenRegistrationWindowCommandExecuted(object p)
        {
            View.RegistrationWindow registrationWindow = new();
            registrationWindow.Show();
            Application.Current.MainWindow.Close();
        }

        private bool CanOpenRegistrationWindowCommandExecute(object p) { return true; }

        #endregion

        #region Команда "Войти"

        // TODO После входа открывать окно StartWindow, если пользователь не находится в организации.
        // TODO Иначе открывать BoardWindow

        public ICommand LoginCommand { get; }
        
        private void OnLoginCommandExecuted(object p)
        {
            InfoMessage = string.Empty;

            try 
            { 
                AuthenticationModel.Authorize(Login, Password);
                HandleSuccessLogin();
            }
            catch (AuthenticationModelException err) { InfoMessage = err.Message; }
        }

        #endregion

        #endregion

        public AuthorizationWindowViewModel()
        {
            // если успешно авторизовались, вызываем обработчик события
            if (AuthenticationModel.TryValidateCredentials()) { HandleSuccessLogin(); }
            
            OpenRegistrationWindowCommand =
                new RelayCommand(OnOpenRegistrationWindowCommandExecuted, CanOpenRegistrationWindowCommandExecute);

            LoginCommand = 
                new RelayCommand(OnLoginCommandExecuted);
        }

        private static void HandleSuccessLogin()
        {
            OrganizationWindow organizationWindow = new();
            organizationWindow.Show();
            Application.Current.MainWindow.Close();
        }
    }
}
