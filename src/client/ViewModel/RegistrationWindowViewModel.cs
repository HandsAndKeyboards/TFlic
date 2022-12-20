using System;
using System.Windows.Input;
using TFlic.ViewModel.Command;
using TFlic.Model.Authentication;
using TFlic.Model.Authentication.Exceptions;
using TFlic.ViewModel.Constants;

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
        
        #endregion


        #region Commands

        #region Команда "Зарегистрироватья"

        public ICommand RegisterCommand { get; }

        private void OnRegisterCommandExecuted(object p)
        {
            try { AuthenticationManager.Register($"{Name} {Surname}", Login, Password); }
            catch (RegistrationException) { InfoMessage = ErrorMessages.UnexpectedError; }
            catch (TimeoutException) { InfoMessage = ErrorMessages.TimeoutMessage; }
            catch (Exception) { InfoMessage = ErrorMessages.UnexpectedError; }
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
