using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseLibrary;


namespace UnitTests
{
    /// <summary>
    /// Klasa testuj¹ca obiekty oraz metody zawarte w bibliotece DatabaseLibrary
    /// MSTest NET Core
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Metoda testuj¹ca klasê Person
        /// czy wszystkie dane zapisywane s¹ prawid³owo do odpowiednich zmiennych
        /// </summary>
        [TestMethod]
        public void PersonObject_Test()
        {
            var person = new Person
            {
                FirstName = "Lukasz",
                LastName = "Ciesla",
                Age = 24,
                Contact = new Contact
                {
                    City = "Lipinki",
                    Street = "Lipinki",
                    HomeNumber = "626", // numer domu reprezentowany jest przez String aby mo¿na by³o zapisaæ np 626a
                    PostCode = "38-305",
                    PostOffice = "Lipinki",
                    Email = "lukaszciesla52@gmail.com",
                    Phone = "661 233 757"
                }
            };

            Assert.AreEqual(person.FirstName, "Lukasz");
            Assert.AreEqual(person.LastName, "Ciesla");
            Assert.AreEqual(person.Age, 24);
            Assert.AreEqual(person.Contact.City, "Lipinki");
            Assert.AreEqual(person.Contact.HomeNumber, "626");
            Assert.AreEqual(person.Contact.Street, "Lipinki");
            Assert.AreEqual(person.Contact.PostCode, "38-305");
            Assert.AreEqual(person.Contact.Email, "lukaszciesla52@gmail.com");
            Assert.AreEqual(person.Contact.Phone, "661 233 757");

            var text = person.Contact.City.ToUpper();
            Assert.AreEqual(text, "LIPINKI");
            Assert.AreNotEqual(text, "Lipinki");
        }

        /// <summary>
        /// Metoda sprawdzaj¹ca prawid³ow¹ implementacje
        /// interfejsu IEquatable dla klasy Person
        /// </summary>
        [TestMethod]
        public void PersonEquatable_Test()
        {
            var personA = new Person
            {
                FirstName = "Lukasz",
                LastName = "Ciesla",
                Age = 24
            };

            var personB = new Person
            {
                FirstName = "Aneta",
                LastName = "Ciesla",
                Age = 20
            };

            var personC = new Person
            {
                FirstName = "Lukasz",
                LastName = "Ciesla",
                Age = 24
            };

            Assert.IsFalse(personA == personB);
            Assert.IsTrue(personA == personC);
        }

        /// <summary>
        /// Sprawdzanie implementacji interfejsu
        /// IEquatable dla klasy Contact
        /// </summary>
        [TestMethod]
        public void ContactEquatable_Test()
        {
            var contactA = new Contact
            {
                City = "Krakow",
                Street = "Marii Bobrzeckiej 5",
                HomeNumber = "3",
                PostCode = "32-216",
                PostOffice = "Krakow",
                Email = "lukaszciesla52@gmail.com",
                Phone = "661 233 757"
            };

            var contactB = new Contact
            {
                City = "Slawkowice",
                Street = "Slawkowice",
                HomeNumber = "248",
                PostOffice = "Wieliczka",
                PostCode = "30-020",
                Email = "ola.dziuba.58@gmail.com",
                Phone = "660 098 600"
            };

            var contactC = new Contact
            {
                City = "Krakow",
                Street = "Marii Bobrzeckiej 5",
                HomeNumber = "3",
                PostCode = "32-216",
                PostOffice = "Krakow",
                Email = "lukaszciesla52@gmail.com",
                Phone = "661 233 757"
            };

            Assert.IsFalse(contactA == contactB);
            Assert.IsTrue(contactA == contactC);
        }

        /// <summary>
        /// Test metody parsuj¹cej tekst na obiekt klasy Person
        /// </summary>
        [TestMethod]
        public void PersonParse_Test()
        {
            string pattern = "3;LUKASZ;CIESLA;24;MIEJSCOWOSC;ULICA;626;38-305;POCZTA;987 654 321;LUKASZCIESLA52@GMAIL.COM";
            var person = DatabaseLibrary.Models.Person.Parse(pattern);

            Assert.AreEqual(person.ID, 3);
            Assert.AreEqual(person.FirstName, "LUKASZ");
            Assert.AreEqual(person.LastName, "CIESLA");
            Assert.AreEqual(person.Age, 24);
            Assert.AreEqual(person.City, "MIEJSCOWOSC");
            Assert.AreEqual(person.Street, "ULICA");
            Assert.AreEqual(person.HomeNumber, "626");
            Assert.AreEqual(person.PostCode, "38-305");
            Assert.AreEqual(person.PostOffice, "POCZTA");
            Assert.AreEqual(person.Phone, "987 654 321");
            Assert.AreEqual(person.Email, "LUKASZCIESLA52@GMAIL.COM");

            var personB = DatabaseLibrary.Models.Person.Parse("4;ALEKSANDRA;DZIUBA;21;MIEJSCOWOSC;ULICA;248;32-020;POCZTA;660 098 600;OLA.DZIUBA.58@GMAIL.COM");

            Assert.IsFalse(person == personB);
        }

        [TestMethod]
        public void PersonToString_Test()
        {
            var person = new DatabaseLibrary.Models.Person
            {
                ID = 3,
                FirstName = "LUKASZ",
                LastName = "CIESLA",
                Age = 24,
                City = "MIEJSCOWOSC",
                Street = "ULICA",
                HomeNumber = "626",
                PostCode = "38-305",
                PostOffice = "POCZTA",
                Phone = "987 654 321",
                Email = "LUKASZCIESLA52@GMAIL.COM"
            };
            Assert.AreEqual(person.ToString(), "3;LUKASZ;CIESLA;24;MIEJSCOWOSC;ULICA;626;38-305;POCZTA;987 654 321;LUKASZCIESLA52@GMAIL.COM");
        }
    }
}
