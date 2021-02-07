using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;


namespace ContactBook.ViewModels.Commands.AboutWindow
{
    public class AcceptCommand : ICommand
    {
        private AboutWindowModel windowModel;
        public AcceptCommand(AboutWindowModel aboutWindowModel) => windowModel = aboutWindowModel;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                windowModel.AcceptButton_Clicked();
        }
    }
}
