using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ContactBook.ViewModels.Commands
{
    public class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public BaseViewModel ViewModel { get; set; }
        
        public BaseCommand(BaseViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => ViewModel.AddContact_Clicked();
    }
}
