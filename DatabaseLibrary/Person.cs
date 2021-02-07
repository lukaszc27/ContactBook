using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseLibrary
{
    /// <summary>
    /// Reprezentacja osób z listy kontaktów w bazie danych
    /// </summary>
    public class Person : IEquatable<Person>, IComparable
    {
        #region ---- Propertis ----
        
        public int ID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int Age { get; set; }
        
        #endregion // Propertis

        /// <summary>
        /// Powiązanie tabel Person oraz Contact
        /// relacją jeden do jeden (EF)
        /// </summary>
        public Contact Contact { get; set; }

        #region ---- Implementacja interfejsu IEquatable ----

        /// <summary>
        /// Porównuje obiekty ze sobą
        /// </summary>
        /// <param name="other">obiekt do porównania z obiektem this (aktualnym)</param>
        /// <returns>true - jeśli obiekty są identyczne, w przeciwnym wypadku false</returns>
        public bool Equals(Person other)
        {
            // sprawdzenie czy obiekt jest utworzony
            // (czy nie posiada wartości null)
            if (other is null)
                return false;

            // sprawdzanie czy obiekt nie wskazuje na samego siebie (referencja)
            if (Object.ReferenceEquals(this, other))
                return true;

            return FirstName == other.FirstName &&
                LastName == other.LastName &&
                Age == other.Age;
        }

        /// <summary>
        /// Porównywanie na podstawie obiektów
        /// metoda pracuje dla wszystkich typów C# dziedziczących po object
        /// </summary>
        /// <param name="obj">obiekt do sprawdzenia</param>
        /// <returns>true - jeśli obj jest obiektem Person oraz jeśli obiekt obj oraz this są identyczne</returns>
        public override bool Equals(object obj)
        {
            if (obj is Person)
                return Person.Equals(this, obj);
            else return false;
        }

        public static bool Equals(Person personA, Person personB)
        {
            // sprawdza czy oba obiekty podane do sprawdzenia mają wartość null
            if ((personA is null) && (personB is null))
                return true;

            if (personA is null)
                return false;

            return personA.Equals(personB);
        }

        public override int GetHashCode() => (FirstName, LastName, Age).GetHashCode();

        /// <summary>
        /// Przeciążanie operatora równości
        /// ma to na celu proste i intiucyjne 
        /// sprawdzanie równości dwóch obiektów
        /// </summary>
        /// <param name="personA"></param>
        /// <param name="personB"></param>
        /// <returns>true jeśli oba obiekty są identyczne, w przeciwnym wpadku false</returns>
        public static bool operator ==(Person personA, Person personB) => Equals(personA, personB);

        /// <summary>
        /// przeciążanie operatora różności
        /// jest przeciwieństwem do operatora równości
        /// </summary>
        /// <param name="personA"></param>
        /// <param name="personB"></param>
        /// <returns> true jeśli oba obiekty są różne, w przeciwnym wypadku false</returns>
        public static bool operator !=(Person personA, Person personB) => !Equals(personA, personB);

        #endregion // Implementacja interfejstu IEqutable

        #region ---- Implementacja interfejsu IComparable ----
        /// <summary>
        /// implementacja interfejsu IComparable
        /// w celu organizacji prawodłowego pożądku
        /// (Obiekty klasy Person są sortowane alfabetycznie rozpoczynająć od nazwiska
        /// następnie imię i na końcu wiek)
        /// </summary>
        /// <param name="obj">obiekt z którym porównuje aktualny obiekt (this)</param>
        /// <returns>
        /// [ < 0] - bieżące wystąpienie poprzedza obiekt
        /// [   0] - bieżące wystąpienie występuje w tym samym pożądku sortowania
        /// [ > 0] - bieżące wystąpienie występuje po obiekcie określonym przez CompareTo
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Person otherPerson = obj as Person;
            
            if (otherPerson.LastName != null)
                return LastName.CompareTo(otherPerson.LastName);

            if (otherPerson.FirstName != null)
                return FirstName.CompareTo(otherPerson.FirstName);

            return Age.CompareTo(otherPerson.Age);
        }
        #endregion
    }
}
