using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using DatabaseLibrary;
using Microsoft.EntityFrameworkCore;
using ContactBook.Windows;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;

namespace ContactBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MyDatabaseContext _context = new MyDatabaseContext();
        public DatabaseLibrary.Models.DataModel personDataModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.BaseViewModel(this);

            personDataModel = new DatabaseLibrary.Models.DataModel();
        }

        /// <summary>
        /// Zdarzenie zamynkania okna głównego
        /// Przed zamknięciem okna pytam użytkownika czy napewno chce wyjść
        /// czy może kliknięcie było przypadkowe
        /// </summary>
        /// <param name="sender">obiekt wysyłający zdarzenie</param>
        /// <param name="e">parametry</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var iResult = MessageBox.Show(this, "Czy napewno chcesz zakończyć pracę z programem ContactBook?",
                "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            e.Cancel = iResult == MessageBoxResult.Yes ? false : true;
        }

        /// <summary>
        /// Zdarzenie wykonywane po załadowaniu okna do pamięci
        /// </summary>
        /// <param name="sender">obiekt wysyłający zdarzenie</param>
        /// <param name="e">parametry</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // tylko dla celów demonstracyjncy, aby łatowiej można było działać
            _context.Database.EnsureCreated();

            // ładowanie danych z bazy do EF core
            _context.Persons.Load();

            // bindowanie danych do modelu
            // personDataGrid.ItemsSource = new DatabaseLibrary.Models.DataModel();
            this.personListView.ItemsSource = personDataModel;
        }

        /// <summary>
        /// obsługa zdarzenia podwójnego kliknięcia myszką na kontrolce ListView
        /// które wyzwala możliwość edycji rekordu występującego pod kursorem myszy
        /// </summary>
        /// <param name="sender">obiekt wysyłający zdarzenie</param>
        /// <param name="e">dodatkowe argumenty</param>
        private void personListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = (DatabaseLibrary.Models.Person)personListView.SelectedItem;
            if (row != null)
            {
                var addWindow = new AddWindow(row.ID);
                if (addWindow.ShowDialog() == true)
                {
                    var p = (from person in _context.Persons
                             join contact in _context.Contacts
                             on person.ID equals contact.PersonID
                             where person.ID == row.ID
                             select person).ToList().First();

                    var c = (from contact in _context.Contacts
                             where contact.PersonID == row.ID
                             select contact).ToList().First();

                    p.FirstName = addWindow.Firstname.Text.ToUpper();
                    p.LastName = addWindow.Surname.Text.ToUpper();
                    p.Age = Convert.ToInt32(addWindow.Age.Text);
                    c.City = addWindow.City.Text.ToUpper();
                    c.Street = addWindow.Street.Text.ToUpper();
                    c.HomeNumber = addWindow.HomeNumer.Text.ToUpper();
                    c.PostCode = addWindow.PostCode.Text.ToUpper();
                    c.PostOffice = addWindow.PostOffice.Text.ToUpper();
                    c.Email = addWindow.Email.Text.ToUpper();
                    c.Phone = addWindow.Phone.Text.ToUpper();

                    p.Contact = c;

                    // zapisa danych w bazie
                    _context.Update(p);
                    _context.SaveChanges();

                    // odświerzenie listy głównej
                    personDataModel.Get();
                }
            }
        }

        /// <summary>
        /// zdarzenie wyzwalane po kliknięciu zakończ w menu głównym aplikacji
        /// </summary>
        /// <param name="sender">obiekt wysyłający zdarzenie</param>
        /// <param name="e">dodatkowe argumenty</param>
        private void MenuCloseItem_Click(object sender, RoutedEventArgs e) => this.Close();

        /// <summary>
        /// zdarzenie eksportu danych z listy do pliku CSV
        /// </summary>
        /// <param name="sender">obiekt wysyłający zdarzenie</param>
        /// <param name="e">argumenty</param>
        private void ExportCSV_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "CSV_data";
            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV documents (.csv)|*.csv";

            if (dialog.ShowDialog() == true)
            {
                // sprawdzenie czy plik o danej nazwie nie istnieje
                // co w przeciwnym razie mogło spowodować wyjątek
                if (!File.Exists(dialog.FileName))
                {
                    using (var stream = File.CreateText(dialog.FileName))
                    {
                        foreach (var person in personDataModel)
                            stream.WriteLine(person);
                    }
                }
            }
        }

        /// <summary>
        /// zdarzenie kliknięcia pocji eksportu do XML w menu kontekstowym listy
        /// export odbywa się poprzez serializację klasy PersonDataModel do pliku
        /// </summary>
        /// <param name="sender">obiekt wywołujący zdarzenie</param>
        /// <param name="e">argumenty</param>
        private void ExportXML_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "XML_data";
            dialog.DefaultExt = ".xml";
            dialog.Filter = "XML documents (.xml)|*.xml";

            if (dialog.ShowDialog() == true)
            {
                // sprawdzenie czy plik istnieje
                // jeśli tak to może to powodować wystąpenie wyjątków w przyszłości
                if (!File.Exists(dialog.FileName))
                {
                    var serialize = new XmlSerializer(typeof(DatabaseLibrary.Models.DataModel));
                    using (var stream = File.CreateText(dialog.FileName))
                    {
                        // zapis do XML odbywa się poprzez serializację do pliku klasy personDataModel
                        // przechowyjącej pobrane dane z bazy które są następnie prezentowane w ListView
                        serialize.Serialize(stream, personDataModel);
                    }
                }
            }
        }

        private void ImportCSV_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "CSV documents (.csv)|*.csv";
            dialog.DefaultExt = "*.csv";

            if (dialog.ShowDialog() == true)
            {
                if (File.Exists(dialog.FileName))
                {
                    using (var stream = new StreamReader(dialog.FileName))
                    {
                        try
                        {
                            if (personDataModel.Count > 0)
                            {
                                var iRet = MessageBox.Show("Czy chcesz przed dokonaniem importu usunąć wszystkie osoby z listy?",
                                    "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                                if (iRet == MessageBoxResult.Yes)
                                {
                                    personDataModel.Clear();

                                    // usuwanie wszystkich rekordów z bazy danych
                                    var rows = from p in _context.Persons
                                               select p;

                                    foreach (var row in rows)
                                        _context.Persons.Remove(row);

                                    _context.SaveChanges();
                                }
                            }

                            // importowanie danych z pliku CSV
                            string line;
                            while ((line = stream.ReadLine()) != null)
                            {
                                personDataModel.Add(DatabaseLibrary.Models.Person.Parse(line));
                            }

                            // przeglądanie wczytanych danych oraz zapis do bazy SQLite
                            foreach (var p in personDataModel)
                            {
                                var person = new Person
                                {
                                    ID = p.ID,
                                    FirstName = p.FirstName,
                                    LastName = p.LastName,

                                    Contact = new Contact
                                    {
                                        City = p.City,
                                        Street = p.Street,
                                        HomeNumber = p.HomeNumber,
                                        PostCode = p.PostCode,
                                        PostOffice = p.PostOffice,
                                        Phone = p.Phone,
                                        Email = p.Email
                                    }
                                };
                                _context.Persons.Add(person);
                            }
                            _context.SaveChanges();

                            MessageBox.Show("Import danych z pliku CSV zakończony!\r\n" +
                                "Jeśli dane nie pojawiły się na liście proszę ponownie uruchomić program!", "Import CSV",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show("Podczas odczytu pliku wystąpił błąd wejścia/wyjścia", "Błąd",
                                MessageBoxButton.OK, MessageBoxImage.Hand);
                        }
                        catch (NullReferenceException ex)
                        {
                            MessageBox.Show("Podczas odczytu wiersza z pliku wystąpiły błędy!", "ReadLine",
                                MessageBoxButton.OK, MessageBoxImage.Hand);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// zdarzenie wywoływane kliknięciem na import danych z pliku XML w menu kontekstowym listy
        /// import XML realizowany jest jako deserializacja klasy PersonDataModel
        /// </summary>
        /// <param name="sender">obiekt wysyłający zdarzenie</param>
        /// <param name="e">argumenty</param>
        private void ImportXML_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "XML documents (.xml)|*.xml";
            dialog.DefaultExt = ".xml";

            if (dialog.ShowDialog() == true)
            {
                // w przypadku importu plik musi istnieć
                // aby nie wystąpiły rzadne wyjątki
                if (File.Exists(dialog.FileName))
                {
                    using (var stream = new StreamReader(dialog.FileName))
                    {
                        var serialize = new XmlSerializer(typeof(DatabaseLibrary.Models.DataModel));

                        // deserializacja danych
                        personDataModel = null;
                        personDataModel = (DatabaseLibrary.Models.DataModel)serialize.Deserialize(stream);

                        // zapis wczytanych danych do bazy
                        using (var db = new MyDatabaseContext())
                        {
                            foreach (var p in personDataModel)
                            {
                                var person = new Person
                                {
                                    ID = p.ID,
                                    FirstName = p.FirstName,
                                    LastName = p.LastName,
                                    Age = p.Age,
                                    Contact = new Contact
                                    {
                                        City = p.City,
                                        Street = p.Street,
                                        HomeNumber = p.HomeNumber,
                                        PostCode = p.PostCode,
                                        PostOffice = p.PostOffice,
                                        Email = p.Email,
                                        Phone = p.Phone
                                    }
                                };
                                db.Persons.Add(person);
                            }
                            db.SaveChanges();
                        }
                        personDataModel.Get();

                        MessageBox.Show("Import danych z XML zakończony powodzeniem!\r\n" +
                            "Jeśli zaimportowanych danych nie widać na liście uruchom ponownie program.", "Import z XML",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}
