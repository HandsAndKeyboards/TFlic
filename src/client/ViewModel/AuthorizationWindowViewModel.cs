using System.Net;
using System.Windows;
using System.Windows.Input;
using TFlic.Command;
using TFlic.Model;

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
        
        private async void OnLoginCommandExecuted(object p)
        {
            var result = await _authModel.Authorize(Login, Password);

            InfoMessage = result.StatusCode switch
            {
                HttpStatusCode.OK => "Успешно", // todo вместо сообщения нужно открывать следующее окно
                HttpStatusCode.Unauthorized => "Неверный логин или пароль",
                _ => "Произошла ошибка. Попробуйте снова"
            };
        }

        #endregion

        #endregion

        public AuthorizationWindowViewModel()
        {
            OpenRegistrationWindowCommand =
                new RelayCommand(OnOpenRegistrationWindowCommandExecuted, CanOpenRegistrationWindowCommandExecute);

            LoginCommand = 
                new RelayCommand(OnLoginCommandExecuted);
        }
    }
}
