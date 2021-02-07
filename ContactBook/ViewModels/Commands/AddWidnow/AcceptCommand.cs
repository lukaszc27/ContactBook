using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ContactBook.ViewModels.Commands.AddWidnow
{
    class AcceptCommand : ICommand
    {
        private ViewModels.AddWindowModel dataContext;
        public AcceptCommand(ViewModels.AddWindowModel context) => dataContext = context;

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                dataContext.AcceptButton_Clicked();
        }
    }
}
