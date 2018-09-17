using System;
using System.Windows.Input;

namespace SpaceEngineers2D.View
{
    public class Command : ICommand
    {
        private readonly Func<bool> _canExecuteFunc;
        private readonly Action _executeFunc;

        public event EventHandler CanExecuteChanged;

        public Command(Action executeFunc)
            : this(() => true, executeFunc)
        {
        }

        public Command(Func<bool> canExecuteFunc, Action executeFunc)
        {
            _canExecuteFunc = canExecuteFunc;
            _executeFunc = executeFunc;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteFunc();
        }

        public void Execute(object parameter)
        {
            _executeFunc();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Command<TParameter> : ICommand
    {
        private readonly Func<TParameter, bool> _canExecuteFunc;
        private readonly Action<TParameter> _executeFunc;

        public event EventHandler CanExecuteChanged;

        public Command(Func<TParameter, bool> canExecuteFunc, Action<TParameter> executeFunc)
        {
            _canExecuteFunc = canExecuteFunc;
            _executeFunc = executeFunc;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteFunc((TParameter) parameter);
        }

        public void Execute(object parameter)
        {
            _executeFunc((TParameter) parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}