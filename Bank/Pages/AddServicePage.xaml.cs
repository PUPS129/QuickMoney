using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Bank.Pages
{
    using Model;

    public partial class AddServicePage : Page
    {
        public AddServicePage()
        {
            InitializeComponent();
            Connection.bankEntities = new Model.BankEntitiesH();
            this.ServiceTitleTxtBx.PreviewTextInput += new TextCompositionEventHandler(ErrorChck);
            this.PercentRateTxtBx.PreviewTextInput += new TextCompositionEventHandler(DigitsOnly);
            this.PaymentDurationTxtBx.PreviewTextInput += new TextCompositionEventHandler(DigitsOnly);

        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServiceListPage());
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var newService = new Service();
            if (ServiceTitleTxtBx.Text.Trim() == string.Empty)
            {
                MessageBox.Show($"'{ServiceTitleLbl.Text}' не может быть пустым","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PaymentDurationTxtBx.Text.Trim() == string.Empty)
            {
                MessageBox.Show($"'{PaymentDurationLbl.Text}' не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PercentRateTxtBx.Text.Trim() == string.Empty)
            {
                MessageBox.Show($"'{PercentRateLbl.Text}' не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            newService.Title = ServiceTitleTxtBx.Text;
            newService.MaximumDuration = PaymentDurationTxtBx.Text;
            newService.PercentRate = PercentRateTxtBx.Text;

            string durres;
            string duration = PaymentDurationTxtBx.Text;
            if (duration.EndsWith("1"))
            {
                 durres = $"{duration} год";
            }
            else if (duration.EndsWith("2") || duration.EndsWith("3") || duration.EndsWith("4"))
            {
                durres = $"{duration} года";
            }
            else
            {
                durres = $"{duration} лет";
            }

            try
            {
                MessageBoxResult Result = MessageBox.Show($"Проверьте верность введенных данных:" +
                    $"\n {ServiceTitleLbl.Text} - {ServiceTitleTxtBx.Text}" +
                    $"\n {PaymentDurationLbl.Text} - {durres}" +
                    $"\n {PercentRateLbl.Text} - {PercentRateTxtBx.Text}%", "Проверка", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    Connection.bankEntities.Service.Add(newService);
                    Connection.bankEntities.SaveChanges();
                    MessageBox.Show("Услуга успешно добавлена", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (Result == MessageBoxResult.No)
                {

                }
            }
            catch
            {
                MessageBox.Show("Ошибка добавления клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DigitsOnly(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
                if (Char.IsSymbol(e.Text, 0)) e.Handled = true;
            }
            catch
            {

            }
        }
        private void ErrorChck(object sender, TextCompositionEventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }
    }
}
