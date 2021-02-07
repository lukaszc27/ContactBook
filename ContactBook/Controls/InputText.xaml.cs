using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContactBook.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy InputText.xaml
    /// </summary>
    public partial class InputText : UserControl
    {
        public InputText()
        {
            InitializeComponent();
            this.rootLayout.DataContext = this;
        }

        #region --- dependency property ---
        /// <summary>
        /// Typy danych wprowadzanych do kontrolki
        /// </summary>
        public enum DataTypes
        {
            Text,
            Number,
            Phone,
            EMail,
            PostCode,
            Street,
            HomeNumber
        };

        /// <summary>
        /// Właściwość Label
        /// tekst ustawainy nad polem wprowadzania danych
        /// </summary>
        public String Label
        {
            get => (String)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        /// <summary>
        /// Właściwość Text
        /// służy do ustawienia lub pobrania tekstu z kontrolki TextBox
        /// </summary>
        public String Text
        {
            get => (String)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Właściwość Feedback
        /// ustawia treść komunikatu widoczną pod polem TextBox
        /// </summary>
        public String Feedback
        {
            get => (String)GetValue(FeedbackProperty);
            set => SetValue(FeedbackProperty, value);
        }

        /// <summary>
        /// Właściwość Type
        /// określa rodzaj wprowadzanych danych do kontrolki
        /// i na jej podstawie dokonuje walidacji danych
        /// przy użyciu wyrażeń regularnych
        /// </summary>
        public DataTypes Type
        {
            get => (DataTypes)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        /// <summary>
        /// Właściwość IsValid (tylko getter)
        /// informuje czy dane wprowadzone są poprawne
        /// poprawność danych określna na podstawie właściwości Type
        /// </summary>
        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
        }

        /// <summary>
        /// rejestracja właściwości, aby mozna było ich użyć w składni XAML
        /// </summary>
        public static readonly DependencyProperty LabelProperty
            = DependencyProperty.Register("Label", typeof(String), typeof(InputText), new PropertyMetadata());

        public static readonly DependencyProperty TextProperty
            = DependencyProperty.Register("Text", typeof(String), typeof(InputText), new PropertyMetadata());

        public static readonly DependencyProperty FeedbackProperty
            = DependencyProperty.Register("Feedback", typeof(String), typeof(InputText), new PropertyMetadata());

        public static readonly DependencyProperty TypeProperty
            = DependencyProperty.Register("Type", typeof(DataTypes), typeof(InputText), new PropertyMetadata());

        public static readonly DependencyProperty IsValidProperty
            = DependencyProperty.Register("IsValid", typeof(bool), typeof(InputText), new PropertyMetadata());
        #endregion

        /// <summary>
        /// Zdarzenie emitowane w przypadku zmiany danych w kontrolce TextBox
        /// służy do walidacji danych
        /// </summary>
        /// <param name="sender">Obiekt emitujący zdarzenie</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var obj = (TextBox)sender;

            // walidacja danych ze względu na typ wprowadzanych danych
            switch (this.Type)
            {
                case DataTypes.Text:
                    if ((new Regex(@"^[A-Za-z]+$")).IsMatch(obj.Text))
                    {
                        this.Feedback = "";
                        _isValid = true;
                    }
                    else
                    {
                        this.Feedback = "Wprowadzone dane są nieprawidłowe!";
                        _isValid = false;
                    }
                    break;

                case DataTypes.Number:
                    if ((new Regex(@"^[0-9]+$")).IsMatch(obj.Text))
                    {
                        this.Feedback = "";
                        _isValid = true;
                    }
                    else
                    {
                        Feedback = "Wprowadzone dane są nieprawidłowe!";
                        _isValid = false;
                    }
                    break;

                case DataTypes.Phone:
                    if ((new Regex(@"^\d{3}\s?\d{3}\s?\d{3}$")).IsMatch(obj.Text))
                    {
                        Feedback = "";
                        _isValid = true;
                    }
                    else
                    {
                        Feedback = "Wprowadzony numer ma nieprawidłową postać!";
                        _isValid = false;
                    }
                    break;

                case DataTypes.EMail:
                    if ((new Regex(@"^[A-Za-z0-9.-]+\@{1}[A-Za-z.]+$")).IsMatch(obj.Text))
                    {
                        Feedback = "";
                        _isValid = true;
                    }
                    else
                    {
                        Feedback = "Wprowadzony adres e-mail ma nieprawidłową postać!";
                        _isValid = false;
                    }
                    break;

                case DataTypes.PostCode:
                    if ((new Regex(@"^\d{2}\-{1}\d{3}$")).IsMatch(obj.Text))
                    {
                        Feedback = "";
                        _isValid = true;
                    }
                    else
                    {
                        Feedback = "Nieprawidłowy kod pocztowy";
                        _isValid = false;
                    }
                    break;

                case DataTypes.Street:
                    if ((new Regex(@"^[A-Za-z\s]+\s?[0-9]*[A-Za-z]?$")).IsMatch(obj.Text))
                    {
                        Feedback = "";
                        _isValid = true;
                    }
                    else
                    {
                        Feedback = "Wprowadzone dane są nieprawidłowe";
                        _isValid = false;
                    }
                    break;

                case DataTypes.HomeNumber:
                    if ((new Regex(@"^\d+[A-Za-z]?$")).IsMatch(obj.Text))
                    {
                        Feedback = "";
                        _isValid = true;
                    }
                    else
                    {
                        Feedback = "Wprowadzone dane są nieprawidłowe!";
                        _isValid = false;
                    }
                    break;
            };
        }
    }
}
