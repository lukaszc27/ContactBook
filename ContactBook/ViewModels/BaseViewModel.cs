using ContactBook.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContactBook.Windows;
using System.Windows.Input;
using DatabaseLibrary;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace ContactBook.ViewModels
{
    /// <summary>
    /// Model danych dla okna głównego aplikacji
    /// (projektowanie w modelu MVVM)
    /// </summary>
    public class BaseViewModel
    {
        #region ---- Propertis ----
        /// <summary>
        /// Komenda dodawania nowej osoby do listy
        /// </summary>
        public ICommand BaseCommand { get; set; }

        /// <summary>
        /// Komenda wyświetlająca okno informacyjne o programie
        /// </summary>
        public ICommand AboutCommand { get; set; }

        /// <summary>
        /// Komenda usuwania zaznaczonej osoby z listy
        /// i bazy danych SQLite
        /// </summary>
        public ICommand RemoveCommand { get; set; }
        #endregion

        #region ---- Konstruktory ----
        private MainWindow mainWindow;

        public BaseViewModel(MainWindow window)
        {
            mainWindow = window;

            BaseCommand = new BaseCommand(this);
            AboutCommand = new AboutCommand(this);
            RemoveCommand = new RemoveCommand(this);
        }
        #endregion // konstruktory

        /// <summary>
        /// Reakcja na komendę BaseCommand
        /// nawiązuje połączenie z bazą danych oraz wyświetla
        /// dialog dodawania nowej osoby
        /// </summary>
        public void AddContact_Clicked()
        {
            using var db = new MyDatabaseContext();

            var addWindow = new AddWindow();
            if (addWindow.ShowDialog() == true)
            {
                var person = new Person
                {
                    FirstName = addWindow.Firstname.Text.ToUpper(),
                    LastName = addWindow.Surname.Text.ToUpper(),
                    Age = Convert.ToInt32(addWindow.Age.Text),
                    
                    Contact = new Contact
                    {
                        City = addWindow.City.Text.ToUpper(),
                        Street = addWindow.Street.Text.ToUpper(),
                        HomeNumber = addWindow.HomeNumer.Text.ToUpper(),
                        PostCode = addWindow.PostCode.Text,
                        PostOffice = addWindow.PostOffice.Text.ToUpper(),
                        Phone = addWindow.Phone.Text,
                        Email = addWindow.Email.Text
                    }
                };

                // dodaje użytkownika do bazy oraz zapsiuje zmainy
                db.Persons.Add(addWindow.Person);
                db.SaveChanges();

                // ponowne wczytanie osób z bazy
                mainWindow.personDataModel.Get();
            }
        }

        /// <summary>
        /// Reakcja na kliknięcie przycisku Usuń w oknie głównym
        /// </summary>
        public void RemoveButton_Clicked()
        {
            using var db = new MyDatabaseContext();

            var item = mainWindow.personListView.SelectedItem;
            if (item != null)
            {
                var row = (DatabaseLibrary.Models.Person)item;
                var person = (from p in db.Persons
                              where p.ID == row.ID
                              select p).ToList().First();

                var iRet = MessageBox.Show($"Czy napewno chcesz usunąć {person.FirstName} {person.LastName} z listy kontaktów?\r\n" +
                    $"UWAGA! Operacja nie może być cofnięta!", "Pytanie", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (iRet == MessageBoxResult.Yes)
                {
                    // usunięcie osoby z bazy oraz zapisanie zmain
                    db.Remove(person);
                    db.SaveChanges();

                    // ponowne pobranie danych oraz umieszczenie ich na liście aby zmiany były widoczne w trybie natychmiastowym
                    mainWindow.personDataModel.Get();
                }
            }
            else
            {
                MessageBox.Show("Przed wykonaniem operacji Usuń musisz wskazać która osoba ma zostać usunięta", "Operacja usuń",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Reakcja na AboutCommand
        /// wyświetla okno informacyjne o programie
        /// </summary>
        public void AboutApplication_Clicked()
        {
            var window = new AboutWindow();
            window.ShowDialog();
        }
    }
}
