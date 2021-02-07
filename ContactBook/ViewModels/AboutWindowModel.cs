using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ContactBook.ViewModels
{
    public class AboutWindowModel
    {
        private Window window;
        public ICommand AcceptCommand { get; set; }

        public AboutWindowModel(Window window)
        {
            this.window = window;
            AcceptCommand = new Commands.AboutWindow.AcceptCommand(this);
        }

        /// <summary>
        /// Reakcja na kliknięcie "O programie..." w menu okna głównego
        /// </summary>
        public void AcceptButton_Clicked() => window.DialogResult = true;
    }
}
