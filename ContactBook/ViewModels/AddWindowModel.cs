using ContactBook.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ContactBook.ViewModels
{
    /// <summary>
    /// Model danych dla okna AddWindow
    /// (model MVVM) => Model, View, View, Model
    /// </summary>
    class AddWindowModel
    {
        public ICommand AcceptCommand { get; set; }
        public ICommand RejectCommand { get; set; }

        private AddWindow window;
        public AddWindowModel(Window window)
        {
            this.window = (AddWindow)window;
            AcceptCommand = new Commands.AddWidnow.AcceptCommand(this);
            RejectCommand = new Commands.AddWidnow.RejectCommand(this);
        }

        /// <summary>
        /// Reakcja na kliknęcie przycisku "Dodaj"
        /// na dialogu dodawania Kontaktu
        /// Sprawdzenie czy wszystkie dane zostały wprowadzone poprawnie,
        /// jeśli którekolwiek pole jest błędnie wypełnione wyświetlenie komunikatu
        /// </summary>
        public void AcceptButton_Clicked()
        {
            bool result = window.Firstname.IsValid &&
                window.Surname.IsValid &&
                window.Age.IsValid &&
                window.Phone.IsValid &&
                window.Email.IsValid &&
                window.City.IsValid &&
                window.Street.IsValid &&
                window.PostCode.IsValid &&
                window.PostOffice.IsValid &&
                window.HomeNumer.IsValid;

            if (!result)
            {
                MessageBox.Show(window, "Nie można zapisać danych ponieważ nie wszystkie pola są uzupełnione poprawnie!\r\n" +
                    "Popraw pola zaznaczone na czerwono a następnie spróbuj zapisać dane", "Błąd sprawdzania poprawności",
                    MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else window.DialogResult = true;
        }

        /// <summary>
        /// Reakcja na przycisnięcie przycisku "Anuluj"
        /// odrzuca wprowadzone dane zamykając okno i zwracając wartość false
        /// </summary>
        public void RejectButton_Clicked() => window.DialogResult = false;
    }
}
