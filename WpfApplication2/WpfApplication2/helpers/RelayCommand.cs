using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfApplication2.helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Action actionAExecuter;

        public RelayCommand(Action action)
        {
            actionAExecuter = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            actionAExecuter();
        }
    }

}