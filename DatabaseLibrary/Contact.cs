using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseLibrary
{
    /// <summary>
    /// Reprezentacja danych kontaktowych w bazie danych
    /// </summary>
    public class Contact : IEquatable<Contact>, IComparable
    {
        #region ---- Propertis ----
        
        public int ID { get; set; }
        public String City { get; set; }
        public String Street { get; set; }
        public String PostCode { get; set; }
        public String PostOffice { get; set; }
        public String HomeNumber { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }

        #endregion // Propertis

        /// <summary>
        /// Powiązanie tabel Contact oraz Person
        /// relacją jeden do jeden
        /// </summary>
        public int PersonID { get; set; }
        public Person Person { get; set; }

        #region ---- Implementacja interfejsu IEqutable ----
        /// <summary>
        /// Poruwnuje bieżący obiekt (this) do obiektu other
        /// </summary>
        /// <param name="other">obiekt do sprawdzenia</param>
        /// <returns>true - jeśli oba obiekty są sobie równe, w przeciwnym wypadku false</returns>
        public bool Equals(Contact other)
        {
            // sprawdza czy obiekt other ma wartość null
            if (other is null)
                return false;

            // spawdza czy obiekt this oraz other nie wskazują na ten sam obiekt
            // (czy nie są referencją do jednego obiektu)
            if (Object.ReferenceEquals(this, other))
                return true;

            return City == other.City &&
                Street == other.Street &&
                HomeNumber == other.HomeNumber &&
                PostCode == other.PostCode &&
                PostOffice == other.PostOffice &&
                Email == other.Email &&
                Phone == other.Phone;
        }

        public override bool Equals(object obj)
        {
            if (obj is Contact)
                return Equals(obj);
            else return false;
        }

        /// <summary>
        /// metoda statyczna służąca do porównynwania dwuch instancji klasy Contact
        /// </summary>
        /// <param name="contactA">obiekt klasy Contact do porównania</param>
        /// <param name="contactB">obiekt klasy Contact do porównania</param>
        /// <returns>true - jeśli obiekty są sobie równe, w przeciwnym wypadku false</returns>
        public static bool Equals(Contact contactA, Contact contactB)
        {
            // sprawdza czy oba parametry są wartościami null
            // jeśli tak to są sobie identyczne ;)
            if ((contactA is null) && (contactB is null))
                return true;

            if (contactA is null)
                return false;

            return contactA.Equals(contactB);
        }

        public override int GetHashCode() => (City, Street, HomeNumber, PostCode, PostOffice, Email, Phone).GetHashCode();

        /// <summary>
        /// przeciążanie operatora równości w celu łatwiejszego i intuicyjnego
        /// sprawdzania czy obiekty są sobie równe
        /// </summary>
        /// <param name="contactA">obiekt Contact do porównania</param>
        /// <param name="contactB">obiekt Contact do porównania</param>
        /// <returns>true - jeśli obiekty są identyczne, w przeciwnym wypadku false</returns>
        public static bool operator ==(Contact contactA, Contact contactB) => Equals(contactA, contactB);

        /// <summary>
        /// przeciążanie operatora różności w celu łatwego oraz intuicyjnego
        /// sprawdzania czy obiekty są różne
        /// </summary>
        /// <param name="contactA">obiekt Contact do sprawdzenia</param>
        /// <param name="contactB">obiekt Contact do sprawdzenia</param>
        /// <returns>true - jeśli obiekty są różne, w przeciwnym wypadku false</returns>
        public static bool operator !=(Contact contactA, Contact contactB) => !Equals(contactA, contactB);

        #endregion // Implementacja interfejsu IEqutable

        #region ---- Implementacja infterfejsu IComparable ----
        /// <summary>
        /// Implementacja interfejsu IComparable w celu
        /// prawidłowego poążdkowania elemetów w listach sortowania
        /// </summary>
        /// <param name="obj">obiekt z którym porównuje aktulany obiekt (this)</param>
        /// <returns>
        /// [ < 0] - bieżące wystąpienie poprzedza obiekt
        /// [   0] - bieżące wystąpienie występuje w tym samym pożądku sortowania
        /// [ > 0] - bieżące wystąpienie występuje po obiekcie określonym przez CompareTo
        /// </returns>
        public int CompareTo(object obj)
        {
            Contact otherContact = obj as Contact;

            // na początku kontakty są sortowane alfabetycznie po miejscowości
            if (otherContact.City != null)
                return City.CompareTo(otherContact.City);

            if (otherContact.Street != null)
                return Street.CompareTo(otherContact.Street);

            if (otherContact.PostOffice != null)
                return PostOffice.CompareTo(otherContact.PostOffice);

            if (otherContact.Email != null)
                return Email.CompareTo(otherContact.Email);

            if (otherContact.Phone != null)
                return Phone.CompareTo(otherContact.Phone);

            return HomeNumber.CompareTo(otherContact.HomeNumber);
        }

        #endregion // Implementacja interfejsu IComparable
    }
}
