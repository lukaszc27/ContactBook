using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;

namespace DatabaseLibrary.Models
{
    /// <summary>
    /// reprezentacja modelu danych przygotowanych do modelu GridView
    /// klasa również jest przygotowana do serializacji do XML
    /// klasa jest zapieczętowana aby z niej nie dzeidziczyć ponieważ stanowi pomost 
    /// pomiędzy tabelą Person a Contacts oraz jest modelem danych pobranych z bazy
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class Person : IEquatable<Person>, IComparable
    {
        #region ---- Propertis ----
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public String FirstName { get; set; }
        [DataMember]
        public String LastName { get; set; }
        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public String City { get; set; }
        [DataMember]
        public String Street { get; set; }
        [DataMember]
        public String HomeNumber { get; set; }
        [DataMember]
        public String PostOffice { get; set; }
        [DataMember]
        public String PostCode { get; set; }
        [DataMember]
        public String Email { get; set; }
        [DataMember]
        public String Phone { get; set; }

        #endregion // propertis

        #region ---- Konstruktory ----
        public Person(String firstName,
            String lastName,
            int age,
            String city,
            String street,
            String homeNumber,
            String postCode,
            String postOffice,
            String email,
            String phone,
            int id = 0)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Street = street;
            City = city;
            HomeNumber = homeNumber;
            PostCode = postCode;
            PostOffice = postOffice;
            Email = email;
            Phone = phone;
            ID = id;
        }

        public Person() { }
        #endregion // konstruktory

        #region ---- Implementacja interfejsu IEquatable ----
        /// <summary>
        /// Porównuje obiekty ze sobą
        /// </summary>
        /// <param name="other">obiekt do porównania z aktualnym obiektem (this)</param>
        /// <returns>true - jeśli obiekty są identyczne, w przeciwnym wypadku false</returns>
        public bool Equals(Person other)
        {
            if (other is null)
                return false;

            // sprawdzenie czy obiekt nie jest referencją do tego samego obiektu
            if (Object.ReferenceEquals(this, other))
                return true;

            return FirstName == other.FirstName &&
                LastName == other.LastName &&
                Age == other.Age &&
                City == other.City &&
                Street == other.Street &&
                HomeNumber == other.HomeNumber &&
                PostCode == other.PostCode &&
                PostOffice == other.PostOffice &&
                Email == other.Email &&
                Phone == other.Phone;
        }

        public override bool Equals(object obj)
        {
            if (obj is Models.Person)   // StackOverflowException !!!
                return Equals(obj);
            else return false;
        }

        /// <summary>
        /// funkcja statyczna opruwnująca dwa obiekty ze sobą
        /// </summary>
        /// <param name="personA">obiekt do porównania</param>
        /// <param name="personB">obiekt do porównania</param>
        /// <returns>true - jeśli obiekty są takie same, w przeciwnym wypadku false</returns>
        public static bool Equals(Person personA, Person personB)
        {
            if ((personA is null) && (personB is null))
                return true;

            if (personA is null)
                return false;

            return personA.Equals(personB);
        }
        public override int GetHashCode() => (FirstName, LastName, Age,
                                              City, Street, HomeNumber, PostOffice, PostCode, Email, Phone).GetHashCode();

        public static bool operator ==(Person personA, Person personB) => Equals(personA, personB);
        public static bool operator !=(Person personA, Person personB) => !Equals(personA, personB);
        #endregion // implementacja interfejsu IEquatable

        #region ---- Implementacja interfejsu IComparable ----
        /// <summary>
        /// Funkcja ustawiająca prawidłowy pożądek obiektów klasy Person
        /// </summary>
        /// <param name="obj">objekt z którym poruwnuje aktualny obiekt (this)</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            Person p = obj as Person;

            // pierwsze pożądkujemy według nazwisk
            if (LastName != p.LastName)
                return LastName.CompareTo(p.LastName);

            // następnie pożądkowanie według imion
            if (FirstName != p.FirstName)
                return FirstName.CompareTo(p.FirstName);

            if (Age != p.Age)
                return Age.CompareTo(p.Age);

            if (City != p.City)
                return City.CompareTo(p.City);

            if (Email != p.Email)
                return Email.CompareTo(p.Email);

            // na samym końcu sortowanie według numerów domu
            return HomeNumber.CompareTo(p.HomeNumber);
        }
        #endregion // implementacja interfejsu IComparable

        #region ---- ToString ----
        public override string ToString() => ToString(';');

        /// <summary>
        /// Zapisuje dane użytkownika w postaci ciągu (użyteczne podczas eksportu do CSV)
        /// </summary>
        /// <param name="separator">znak który rozdziela poszczególne sekcje</param>
        /// <returns>ciąg znaków gotowy do zapisu w pliku CSV (string)</returns>
        public string ToString(char separator)
        {
            return $"{ID}{separator}" +
                $"{FirstName}{separator}" +
                $"{LastName}{separator}" +
                $"{Age}{separator}" +
                $"{City}{separator}" +
                $"{Street}{separator}" +
                $"{HomeNumber}{separator}" +
                $"{PostCode}{separator}" +
                $"{PostOffice}{separator}" +
                $"{Phone}{separator}" +
                $"{Email}";
        }

        /// <summary>
        /// parsusje tekst przekazany jako argument
        /// dzieląc go na częsci i wyciągając dane z których następnie
        /// tworzy obiekt Person
        /// </summary>
        /// <param name="pattern">tekst przeznaczony parsowaniu</param>
        /// <param name="separator">znak według którego pattern jest dzielony na kolumny</param>
        /// <returns>obiekt Person utworzony z danych występujących w tekscie pattern</returns>
        public static Person Parse(string pattern, char separator = ';')
        {
            if (pattern == null)
                throw new NullReferenceException();

            pattern = pattern.Trim();   // usuwa wszystkie białe znaki z tekstu przeznaczonego do parsowania

            // pocięcie teksu według podanego separatora
            string[] cols = pattern.Split(separator);

            return new Person
            {
                ID = Convert.ToInt32(cols[0]),
                FirstName = cols[1],
                LastName = cols[2],
                Age = Convert.ToInt32(cols[3]),
                City = cols[4],
                Street = cols[5],
                HomeNumber = cols[6],
                PostCode = cols[7],
                PostOffice = cols[8],
                Phone = cols[9],
                Email = cols[10]
            };
        }
        #endregion // ToString
    }
}
