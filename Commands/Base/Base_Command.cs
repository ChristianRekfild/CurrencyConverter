using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProgSim.Commands.Base
{
    public abstract class Base_Command : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add     => CommandManager.RequerySuggested += value;
            remove  => CommandManager.RequerySuggested -= value;
        }

        // Можно ли выполнить команду
        public abstract bool CanExecute(object? parameter);

        // Основная логика команды
        public abstract void Execute(object? parameter);
    }
}
