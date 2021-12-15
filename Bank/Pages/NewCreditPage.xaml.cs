using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Bank.Pages
{
    using Model;
    public partial class NewCreditPage : Page
    {
        // Получение списков из БД
        public List<Client> clients { get; set; }
        public List<Service> services { get; set; }
        public List<Payment> payments { get; set; }
        public List<ServiceClient> serviceClients { get; set; }

        public double Duration { get; set; }
        public double PercentRate { get; set; }
        public NewCreditPage()
        {
            InitializeComponent();
            DataContext = this;
            try  // Попытка подключения к БД
            {
                Connection.bankEntities = new Model.BankEntitiesH();
                clients = Connection.bankEntities.Client.ToList();
                services = Connection.bankEntities.Service.ToList();
                payments = Connection.bankEntities.Payment.ToList();
                serviceClients = Connection.bankEntities.ServiceClient.ToList();

            }
            catch
            {
                MessageBox.Show("База данных не отвечает, обратитесь к системному администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClientsList.IsEnabled = false;
                ServiceList.IsEnabled = false;
                TotalAmountTxtBx.IsEnabled = false;
                ArrangeCreditBtn.IsEnabled = false;
            }
            this.TotalAmountTxtBx.PreviewTextInput += new TextCompositionEventHandler(TotalAmountTxtBx_TextChanged);
        }
        // Метод для оформления кредита
        private void ArrangeCreditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsList.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите клиента", "",MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (ServiceList.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите услугу", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (TotalAmountTxtBx.Text.Length == 0)
            {
                MessageBox.Show("Введите сумму кредита", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var newServiceClient = new ServiceClient();
            newServiceClient.Client1 = ClientsList.SelectedItem as Client;
            newServiceClient.Service1 = ServiceList.SelectedItem as Service;
            newServiceClient.Amount = TotalAmountTxtBx.Text;
            int MaxDuration = Convert.ToInt32(newServiceClient.Service1.MaximumDuration.ToString());
            newServiceClient.DateRegistr = DateTime.Now;
            newServiceClient.DateExpires = DateTime.Now.AddYears(MaxDuration);
            newServiceClient.Status = "0";

            var newPayment = new Payment();
            PercentRate = Convert.ToDouble(newServiceClient.Service1.PercentRate.ToString());
            Duration = Convert.ToDouble(newServiceClient.Service1.MaximumDuration.ToString());
            newPayment.Date = DateTime.Today;

            //double amount = Convert.ToDouble(TotalAmountTxtBx.Text);
            //double MonthPay = (amount + amount * PercentRate * Duration / 100) / (Duration * 12);
            //double OverPay = amount - (MonthPay * Duration);   // \nСумма переплаты составит{OverPay

            MessageBoxResult Result = MessageBox.Show($"Вы действительно хотите оформить кредит\nна сумму {TotalAmountTxtBx.Text} ₽" +
                $"\nна {newServiceClient.Client1.Name} {newServiceClient.Client1.Surname} {newServiceClient.Client1.Patronymic},\nПаспортные данные №{newServiceClient.Client1.Passport}.","Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                try // Оформление кредита
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
        // Метод для расчёта ежемесячного платежа
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
        // Метод валидации вводимых данных
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
        // Кнопка навигации на главную страницу
        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Mainpage());
        }
    }
}
