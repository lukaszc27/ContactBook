using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using DatabaseLibrary;

namespace ContactBook.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        #region ---- Konstruktor ----
        public AddWindow(int id = 0)
        {
            ID = id;

            InitializeComponent();

            var model = new ViewModels.AddWindowModel(this);
            AcceptButton.Command = model.AcceptCommand;
            RejectButton.Command = model.RejectCommand;
            
            if (ID != 0)
            {
                // nawiązanie połączenia z bazą danych
                using var db = new MyDatabaseContext();

                // wyszukanie przy pomocy LINQ osoby o podanym ID
                // oraz utworzenie odpowiedniego obiektu (klasy Person)
                var person = (from p in db.Persons
                             join c in db.Contacts
                             on p.ID equals c.PersonID
                             where p.ID == ID
                             select new Person {
                                 ID = p.ID,
                                 FirstName = p.FirstName,
                                 LastName = p.LastName,
                                 Age = p.Age,
                                 Contact = new Contact {
                                     City = c.City,
                                     Street = c.Street,
                                     HomeNumber = c.HomeNumber,
                                     PostCode = c.PostCode,
                                     PostOffice = c.PostOffice,
                                     Email = c.Email,
                                     Phone = c.Phone
                                 }
                             }).First();

                //// wyświetlanie pobrancyh danych w odpowiednich kontrolkach

                // ustawienie nowego tytułu okna
                this.Caption.Text = $"Aktualizacja użytkownika: {person.FirstName} {person.LastName}";
                this.Title = this.Caption.Text;

                // adekwatny tekst na przycisku (zamiast zapisz zmiana na Zaaktualizuj)
                this.AcceptButton.Content = "Zaaktualizuj";

                // wprowadzenie danych do odpowiednich pól edytowalnych
                this.Firstname.Text = person.FirstName;
                this.Surname.Text = person.LastName;
                this.Age.Text = Convert.ToString(person.Age);
                this.City.Text = person.Contact.City;
                this.Street.Text = person.Contact.Street;
                this.HomeNumer.Text = person.Contact.HomeNumber;
                this.PostCode.Text = person.Contact.PostCode;
                this.PostOffice.Text = person.Contact.PostOffice;
                this.Email.Text = person.Contact.Email;
                this.Phone.Text = person.Contact.Phone;
            }
        }
        #endregion // konstruktor

        #region ---- Propertis ----
        /// <summary>
        /// Id aktualnie edytowanego użytkownika
        /// jeśli jest to nowo dodawany użytkownik to Id = 0
        /// </summary>
        private int ID { get; set; }

        /// <summary>
        /// Zwraca obiekt Person utworzony na podstawie danych wprowadzonych w dialogu
        /// </summary>
        public Person Person
        {
            get
            {
                return new Person
                {
                    ID = this.ID != 0 ? this.ID : 0,
                    FirstName = this.Firstname.Text.ToUpper(),
                    LastName = this.Surname.Text.ToUpper(),
                    Age = Convert.ToInt32(this.Age.Text),

                    Contact = new Contact
                    {
                        City = this.City.Text.ToUpper(),
                        Street = this.Street.Text.ToUpper(),
                        HomeNumber = this.HomeNumer.Text,
                        PostCode = this.PostCode.Text.ToUpper(),
                        PostOffice = this.PostOffice.Text.ToUpper(),
                        Email = this.Email.Text.ToUpper(),
                        Phone = this.Phone.Text.ToUpper(),
                    }
                };
            }
        }
        #endregion // propertis
    }
}
