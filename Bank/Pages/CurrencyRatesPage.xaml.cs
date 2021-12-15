using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace Bank.Pages
{
    public partial class CurrencyRatesPage : Page
    {
        string eurParse { get; set; }
        string dollarParse { get; set; }
        double euro { get; set; }
        double dollar { get; set; }
        public CurrencyRatesPage()
        {
            InitializeComponent();
            CurrencyCheck();
            this.ValueEuro.PreviewTextInput += new TextCompositionEventHandler(HandleText);
            this.ValueDollar.PreviewTextInput += new TextCompositionEventHandler(HandleText);
            this.ValueEuro.Text = "1";
            this.ValueDollar.Text = "1";
        }
        private void CurrencyCheck()
        {
            try
            {
                WebClient client = new WebClient();
                var xml = client.DownloadString("https://www.cbr-xml-daily.ru/daily.xml");
                XDocument xdoc = XDocument.Parse(xml);
                var el = xdoc.Element("ValCurs").Elements("Valute");
                dollarParse = el.Where(x => x.Attribute("ID").Value == "R01235").Select(x => x.Element("Value").Value).FirstOrDefault();
                eurParse = el.Where(x => x.Attribute("ID").Value == "R01239").Select(x => x.Element("Value").Value).FirstOrDefault();
                euro = Math.Round(Convert.ToDouble(eurParse), 2);
                dollar = Math.Round(Convert.ToDouble(dollarParse), 2);
                Euro.Text = euro + " ₽";
                Dollar.Text = dollar + " ₽";
            }
            catch
            {
                MessageBox.Show("Ошибка получения данных курса, попробуйте позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                CurrUpdateBtn.Visibility = Visibility.Visible;
            }
        }

        private void CalculateCurrency_Click(object sender, RoutedEventArgs e)
        {
            if (ValueEuro.Text.StartsWith("0,") && ValueEuro.Text.Length > 1)
            {
                ValueEuro.Text = ValueEuro.Text;
            }
            else if (ValueEuro.Text.StartsWith("00") && ValueEuro.Text.Length > 1)
            {
                ValueEuro.Text = null;
            }
            double ResValueEuro = Math.Round(Convert.ToDouble(ValueEuro.Text) * euro, 2);
            Euro.Text = ResValueEuro.ToString() + " ₽";


            if (ValueEuro.Text.StartsWith("0,") && ValueEuro.Text.Length > 1)
            {
                ValueEuro.Text = ValueEuro.Text;
            }
            else if (ValueDollar.Text.StartsWith("00") && ValueDollar.Text.Length > 1)
            {
                ValueDollar.Text = null;
            }
            double ResValueDollar = Math.Round(Convert.ToDouble(ValueDollar.Text) * dollar, 2);
            Dollar.Text = ResValueDollar.ToString() + " ₽";
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Mainpage());
        }
        void HandleText(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
            if (e.Text == ",") e.Handled = false;
        }

        private void RepeatClick(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalculateCurrency_Click(sender, e);
            }
            catch
            {

            }
        }

        private void CurrUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrencyCheck();
            }
            catch
            {
                MessageBox.Show("Ошибка получения данных курса, попробуйте позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
