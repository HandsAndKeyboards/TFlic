using System;
using TFlic.ViewModel.Command.Base;

namespace TFlic.ViewModel.Command
{
    internal class RelayCommand : BaseCommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool>? _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object? parameter) => parameter != null && (_canExecute?.Invoke(parameter) ?? true);

        public override void Execute(object? parameter)
        {
            if (parameter != null) _execute.Invoke(parameter);
        }
    }
}
