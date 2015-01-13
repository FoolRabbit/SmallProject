using System;
using System.Windows.Input;

namespace DelegateCommandSample
{
    public class DelegateCommand : ICommand
    {
        #region members
        /// <summary> 
        /// can execute function 
        /// </summary> 
        private readonly Func<bool> canExecute;

        /// <summary> 
        /// execute function 
        /// </summary> 
        private readonly Action execute;

        #endregion

        /// <summary> 
        /// Initializes a new instance of the DelegateCommand class. 
        /// </summary> 
        /// <param name="execute">indicate an execute function</param> 
        public DelegateCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary> 
        /// Initializes a new instance of the DelegateCommand class. 
        /// </summary> 
        /// <param name="execute">execute function </param> 
        /// <param name="canExecute">can execute function</param> 
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary> 
        /// can executes event handler 
        /// </summary> 
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary> 
        /// implement of icommand can execute method 
        /// </summary> 
        /// <param name="parameter">parameter by default of icomand interface</param> 
        /// <returns>can execute or not</returns> 
        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }

            return this.canExecute();
        }

        /// <summary> 
        /// implement of icommand interface execute method 
        /// </summary> 
        /// <param name="parameter">parameter by default of icomand interface</param> 
        public void Execute(object parameter)
        {
            this.execute();
        } 
    }

    /// <summary> 
    /// delegate command for view model 
    /// </summary> 
    public class DelegateCommand<T> : ICommand
    {
        #region members
        /// <summary> 
        /// can execute function 
        /// </summary> 
        private readonly Func<T, bool> canExecute;

        /// <summary> 
        /// execute function 
        /// </summary> 
        private readonly Action<T> execute;

        #endregion

        /// <summary> 
        /// Initializes a new instance of the DelegateCommand class. 
        /// </summary> 
        /// <param name="execute">indicate an execute function</param> 
        public DelegateCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary> 
        /// Initializes a new instance of the DelegateCommand class. 
        /// </summary> 
        /// <param name="execute">execute function </param> 
        /// <param name="canExecute">can execute function</param> 
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary> 
        /// can executes event handler 
        /// </summary> 
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary> 
        /// implement of icommand can execute method 
        /// </summary> 
        /// <param name="parameter">parameter by default of icomand interface</param> 
        /// <returns>can execute or not</returns> 
        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }

            return this.canExecute((T)parameter);
        }

        /// <summary> 
        /// implement of icommand interface execute method 
        /// </summary> 
        /// <param name="parameter">parameter by default of icomand interface</param> 
        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }
    } 
}