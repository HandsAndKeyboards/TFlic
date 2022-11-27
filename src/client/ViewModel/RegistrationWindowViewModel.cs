using System.Net;
using System.Windows.Input;
using TFlic.Command;
using TFlic.Model;

namespace TFlic.ViewModel
{
    internal class RegistrationWindowViewModel : Base.ViewModelBase
    {
        #region Fields and properties

        private string _login = string.Empty;
        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        private string _surname = string.Empty;
        public string Surname
        {
            get => _surname;
            set => Set(ref _surname, value);
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private string _repeatPassword = string.Empty;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set => Set(ref _repeatPassword, value);
        }

        // Сообщение об ошибке 
        private string _infoMessage = string.Empty;
        public string InfoMessage
        {
            get => _infoMessage;
            set => Set(ref _infoMessage, value);
        }

        private readonly AuthenticationModel _authModel = new();
        
        #endregion


        #region Commands

        #region Команда "Зарегистрироватья"

        public ICommand RegisterCommand { get; }

        private async void OnRegisterCommandExecuted(object p)
        {
            var result = await _authModel.Register(Name, Login, Password);
            
            InfoMessage = result.StatusCode switch
            {
                HttpStatusCode.OK => "Успешно", // todo вместо сообщения нужно открывать следующее окно
                _ => "Произошла ошибка. Попробуйте снова"
            };
        }

        private bool CanRegisterCommandExecute(object p)
        {
            var canExecute = _password == _repeatPassword;
            if (!canExecute) { _infoMessage = "Пароли не совпадают"; }
            
            return canExecute;
        }

        #endregion

        #endregion


        #region Constructors

        public RegistrationWindowViewModel()
        {
            RegisterCommand =
                new RelayCommand(OnRegisterCommandExecuted, CanRegisterCommandExecute);
        }

        #endregion
    }
}
