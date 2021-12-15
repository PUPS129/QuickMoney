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

namespace Bank.Pages.FullNewCredit
{
    using Model;
    public partial class FULLNewCreditPage : Page
    {
        private Client selected;

        public Client currentClient { get; set; }
        public List<Client> clients { get; set; }
        public List<Service> services { get; set; }
        public List<Payment> payments { get; set; }
        public List<ServiceClient> serviceClients { get; set; }
        public double Duration { get; set; }
        public double PercentRate { get; set; }

        public FULLNewCreditPage(Client selected)
        {
            currentClient = selected;
            InitializeComponent();
            DataContext = this;
            try
            {
                Connection.bankEntities = new Model.BankEntitiesH();
                clients = Connection.bankEntities.Client.ToList();
                services = Connection.bankEntities.Service.ToList();

                currentClient = new Client
                {
                    ClientID = currentClient.ClientID,
                    Surname = currentClient.Surname,
                    Name = currentClient.Name,
                    Patronymic = currentClient.Patronymic,
                    Passport = currentClient.Passport,
                    Phone = currentClient.Phone,
                    Birthday = currentClient.Birthday

                    //ClientID = -1
                };
                var crClient = currentClient.Name.ToList();
                //MessageBox.Show(crClient);
                //ClientsList.ItemsSource = crClient;
            }
            catch
            {
                MessageBox.Show("База данных не отвечает, обратитесь к системному администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //ClientsList.IsEnabled = false;
                ServiceList.IsEnabled = false;
                TotalAmountTxtBx.IsEnabled = false;
                ArrangeCreditBtn.IsEnabled = false;
            }
            this.TotalAmountTxtBx.PreviewTextInput += new TextCompositionEventHandler(TotalAmountTxtBx_TextChanged);
        }
        private void ArrangeCreditBtn_Click(object sender, RoutedEventArgs e)
        {
            var newServiceClient = new ServiceClient();
           // var cl = Connection.bankEntities.Client.Find(currentClient.ClientID);
            //ClientsList.SelectedIndex = cl.ClientID;
            //newServiceClient.Client1 = currentClient;

            //if (ClientsList.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Выберите клиента");
            //    return;
            //}
            if (ServiceList.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите услугу");
                return;
            }
            if (TotalAmountTxtBx.Text.Length == 0)
            {
                MessageBox.Show("Введите сумму кредита");
                return;
            }

            newServiceClient.Client1 = currentClient as Client;
            newServiceClient.Service1 = ServiceList.SelectedItem as Service;
            newServiceClient.Amount = TotalAmountTxtBx.Text;
            int MaxDuration = Convert.ToInt32(newServiceClient.Service1.MaximumDuration);
            newServiceClient.DateRegistr = DateTime.Now;
            newServiceClient.DateExpires = DateTime.Now.AddYears(MaxDuration);
            newServiceClient.Status = "0";

            var newPayment = new Payment();
            PercentRate = Convert.ToDouble(newServiceClient.Service1.PercentRate.ToString());
            Duration = Convert.ToDouble(newServiceClient.Service1.MaximumDuration.ToString());
            newPayment.Date = DateTime.Today;

            //double amount = Convert.ToDouble(TotalAmountTxtBx.Text);
            //double Overpay = ((amount + amount * PercentRate * Duration / 100) / (Duration * 12) * 12) - amount;   \nСумма переплаты составит {Overpay}

            MessageBoxResult Result = MessageBox.Show($"Вы действительно хотите оформить кредит\n на сумму {TotalAmountTxtBx.Text} ₽" +
                $"\n на {newServiceClient.Client1.Name} {newServiceClient.Client1.Surname} {newServiceClient.Client1.Patronymic},\n Паспортные данные №{newServiceClient.Client1.Passport}.", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
            {
                try
                {
                    
                    Connection.bankEntities.ServiceClient.Add(newServiceClient);
                    Payment();
                    Connection.bankEntities.SaveChanges();
                    MessageBox.Show("Кредит успешно оформлен", "Оформление", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new Mainpage());
                }
                catch
                {
                    MessageBox.Show("Ошибка оформления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (Result == MessageBoxResult.No)
            {

            }
        }

        private void Payment()
        {
            int DurationInMonths = Convert.ToInt32(Duration * 12);
            DateTime date = DateTime.Now;
            double amount = Convert.ToDouble(TotalAmountTxtBx.Text);
            double MonthPay = (amount + amount * PercentRate * Duration / 100) / (Duration * 12);
            double MonthPayResult = Math.Round(MonthPay, 2);

            for (int i = 0; i < DurationInMonths; i++)
            {
                var newPayment = new Payment();
                newPayment.Date = date.AddMonths(1);
                newPayment.CurPayStatus = "0";
                date = newPayment.Date;

                newPayment.Amount = MonthPayResult.ToString();
                Connection.bankEntities.Payment.Add(newPayment);
            }
        }


        void TotalAmountTxtBx_TextChanged(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
            }
            catch
            {

            }
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
