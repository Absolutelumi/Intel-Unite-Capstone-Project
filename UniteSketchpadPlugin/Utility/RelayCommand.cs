using System;
using System.Windows.Input;

namespace UnitePluginTest.Utility
{
    [Serializable]
    internal class RelayCommand<T> : ICommand
    {
        [field: NonSerialized]
        private readonly Action<T> execute;

        [field: NonSerialized]
        private readonly Predicate<T> canExecute;

        [field: NonSerialized]
        private EventHandler canExecuteChanged;

        public event EventHandler CanExecuteChanged
        {
            add => canExecuteChanged += value;
            // ReSharper disable once DelegateSubtraction
            remove => canExecuteChanged -= value;
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute((T)parameter);
        }


        public void RaiseCanExecuteChanged()
        {
            canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}