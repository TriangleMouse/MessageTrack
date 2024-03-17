using Serilog;
using System.Windows;
using System.Windows.Input;

namespace MessageTrack.PL.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public async void Execute(object parameter)
        {
            try
            {
                await execute();
            }
            catch (Exception ex)
            {
                Log.Error("Ошибка: {0}", ex.Message);
                Log.Verbose("Трассировка: {0}", ex.StackTrace);
                MessageBox.Show($"Ошибка! Обратитесь к разработчику системы и передайте логи программы. Текст ошибки: {ex.Message}", "Ошибка");
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T, Task> execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Func<T, Task> execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public async void Execute(object parameter)
        {
            try
            {
                await execute((T)parameter);
            }
            catch (Exception ex)
            {
                Log.Error("Ошибка: {0}", ex.Message);
                Log.Verbose("Трассировка: {0}", ex.StackTrace);
                MessageBox.Show($"Ошибка! Обратитесь к разработчику системы и передайте логи программы. Текст ошибки: {ex.Message}", "Ошибка");
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
