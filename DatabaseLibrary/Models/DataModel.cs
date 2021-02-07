using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace DatabaseLibrary.Models
{
    [Serializable]
    public class DataModel : ObservableCollection<Person>
    {
        public DataModel() => this.Get();


        /// <summary>
        /// Pobiera dane z bazy SQLite przy użyciu Enity Framework
        /// </summary>
        public void Get()
        {
            this.Clear(); // wyczyszczenie listy przed ponowym uzupełnieneim danymi z bazy

            using var db = new MyDatabaseContext();

            // pobranie danych z bazy 
            // oraz połączenie informacji z dwóch tabel
            var persons = (from person in db.Persons
                           join contact in db.Contacts
                           on person.ID equals contact.PersonID
                           select new Person { 
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Age = person.Age,
                                City = contact.City,
                                Street = contact.Street,
                                HomeNumber = contact.HomeNumber,
                                PostCode = contact.PostCode,
                                PostOffice = contact.PostOffice,
                                Email = contact.Email,
                                Phone = contact.Phone,
                                ID = person.ID
                           }).ToList();

            // dodanie każdej pobranej osoby do listy w celu wyświetlenia w kontrolce
            foreach (var person in persons)
                Add(person);
        }
    }
}
