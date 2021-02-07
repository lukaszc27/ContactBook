using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ContactBook.ViewModels.Commands
{
    public class AboutCommand : ICommand
    {
        private BaseViewModel windowModel;
        public AboutCommand(BaseViewModel baseModel) => windowModel = baseModel;

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                windowModel.AboutApplication_Clicked();
        }
    }
}
