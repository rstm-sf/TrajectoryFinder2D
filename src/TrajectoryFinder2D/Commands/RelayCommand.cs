using System;
using System.Windows.Input;

namespace TrajectoryFinder2D.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;

        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged;

        internal RelayCommand(Action<object> execute)
            : this(execute, parameter => true)
        {
        }

        internal RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object parameter) =>
            _canExecute(parameter);

        public void Execute(object parameter) =>
            _execute.Invoke(parameter);

        public void RaiseCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
