using System;

namespace TFlic.Command
{
    internal class RelayCommand : Base.BaseCommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool>? canExecute;

        public RelayCommand(Action<object> Execute, Func<object, bool>? CanExecute = null)
        {
            execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            canExecute = CanExecute;
        }

        public override bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object? parameter) => execute?.Invoke(parameter);
    }
}
