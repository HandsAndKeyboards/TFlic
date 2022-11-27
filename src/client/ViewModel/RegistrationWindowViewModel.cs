using System.Windows;
using System.Windows.Input;
using TFlic.Command;

namespace TFlic.ViewModel
{
    internal class RegistrationWindowViewModel : Base.ViewModelBase
    {
        #region Fields and properties

        string login = string.Empty;
        public string Login
        {
            get => login;
            set => Set(ref login, value);
        }

        string surname = string.Empty;
        public string Surname
        {
            get => surname;
            set => Set(ref surname, value);
        }

        string name = string.Empty;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        string password = string.Empty;
        public string Password
        {
            get => password;
            set => Set(ref password, value);
        }

        string repeatPassword = string.Empty;
        public string RepeatPassword
        {
            get => repeatPassword;
            set => Set(ref repeatPassword, value);
        }

        // Сообщение об ошибке 
        string wrongDataMessage = string.Empty;
        public string WrongDataMessage
        {
            get => wrongDataMessage;
            set => Set(ref wrongDataMessage, value);
        }

        #endregion


        #region Commands

        #region Команда "Зарегистрироватья"

        public ICommand RegisterCommand { get; }

        private void OnRegisterCommandExecuted(object p)
        {

        }

        private bool CanRegisterCommandExecute(object p)
        { 
            string errorMessage = string.Empty;
            return true;
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
