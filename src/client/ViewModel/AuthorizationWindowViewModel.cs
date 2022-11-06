using System.Windows;
using System.Windows.Input;
using TFlic.Command;

namespace TFlic.ViewModel
{
    internal class AuthorizationWindowViewModel : Base.ViewModelBase
    {
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

        #endregion

        public AuthorizationWindowViewModel()
        {
            OpenRegistrationWindowCommand =
                new RelayCommand(OnOpenRegistrationWindowCommandExecuted, CanOpenRegistrationWindowCommandExecute);
        }
    }
}
