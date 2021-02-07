using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ContactBook.ViewModels.Commands
{
    /// <summary>
    /// Komenda, rekacja na kliknięcie przycisku usuń w oknie głównym
    /// </summary>
    public class RemoveCommand : ICommand
    {
        private BaseViewModel viewModel;
        public RemoveCommand(BaseViewModel model)
        {
            viewModel = model;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                viewModel.RemoveButton_Clicked();
        }
    }
}
