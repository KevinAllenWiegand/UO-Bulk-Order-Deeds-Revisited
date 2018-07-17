using System;
using System.Windows.Input;

namespace Npe.UO.BulkOrderDeeds.SampleImportPlugin
{
    public class RelayCommand : ICommand
    {
        private Action<object> _Action;
        private Func<bool> _CanExecute;

        public RelayCommand(Action<object> action, Func<bool> canExecute = null)
        {
            _Action = action;

            if (canExecute != null)
            {
                _CanExecute = canExecute;
            }
            else
            {
                _CanExecute = () => true;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _CanExecute();
        }

        public void Execute(object parameter)
        {
            _Action(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
