using System.Windows;
using System.Windows.Input;
using TFlic.Command;

namespace TFlic.ViewModel
{
    internal class AuthorizationWindowViewModel : Base.ViewModelBase
    {
        #region Поля

        private string login = string.Empty;
        public string Login
        {
            get => login;
            set => Set(ref login, value);
        }

        private string password = string.Empty;
        public string Password
        {
            get => password;
            set => Set(ref password, value);
        }

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
            
        }

        private bool CanLoginCommandExecute(object p) { return true; }

        #endregion

        #endregion

        public AuthorizationWindowViewModel()
        {
            OpenRegistrationWindowCommand =
                new RelayCommand(OnOpenRegistrationWindowCommandExecuted, CanOpenRegistrationWindowCommandExecute);

            LoginCommand = 
                new RelayCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
        }
    }
}
