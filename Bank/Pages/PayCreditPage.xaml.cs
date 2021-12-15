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

namespace Bank.Pages
{
    using Model;
    /// <summary>
    /// Логика взаимодействия для PayCreditPage.xaml
    /// </summary>
    public partial class PayCreditPage : Page
    {
        // Получение данных из БД
        private Payment originalPayment;
        private ServiceClient originalServiceClient;
        public Payment currnetPayment { get; set; }
        public List<ServiceClient> serviceClients {get;set;}
        public ServiceClient currentServiceClient { get; set; }
        public PayCreditPage(Model.Payment selected)
        {
            InitializeComponent();
            this.PaymentTxtBx.PreviewTextInput += new TextCompositionEventHandler(TotalPayment_TextChanged);
            // Получение данных с прошлой страницы
            originalPayment = selected;
            currnetPayment = new Payment
            {
                Amount = originalPayment.Amount,
                Date = originalPayment.Date,
                ServiceClientID = originalPayment.ServiceClientID,
                CurPayStatus = originalPayment.CurPayStatus,

                PaymentID = -1
            };
            //currentServiceClient = new ServiceClient()
            //{
            //    //Status = originalServiceClient.Status
            //};
            //Check();
            DataContext = this;
            toPayBx.Text =  currnetPayment.Amount;
        }
        // Оплата ежемесячного платежа
        private void PayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentTxtBx.Text.Length == 0)
            {
                MessageBox.Show("Введите сумму оплаты", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            double currentPay = Convert.ToDouble(currnetPayment.Amount);
            double pay = Convert.ToDouble(PaymentTxtBx.Text);
            double PayResult = currentPay - pay;
            if (PayResult < 0)
            {
                PayResult = 0;
            }
            if (PayResult == 0)
            {
                currnetPayment.CurPayStatus = "1";
            }
            if (PayResult > 0)
            {
                currnetPayment.CurPayStatus = "0";
            }
            currnetPayment.Amount = PaymentTxtBx.Text;
            try
            {
                MessageBoxResult Result = MessageBox.Show($"Подтвердите платёж на сумму :\n" +
                    $"\n {PaymentTxtBx.Text} ₽", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (Result == MessageBoxResult.Yes)
                {
                    originalPayment.CurPayStatus = currnetPayment.CurPayStatus;
                    originalPayment.Amount = PayResult.ToString();
                    // Сохранение изменений в БД
                    Connection.bankEntities.SaveChanges();
                    MessageBox.Show("Успешное внесение платежа", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                else if (Result == MessageBoxResult.No)
                {
                }
            }
            catch
            {
                MessageBox.Show("Ошибка внесения платежа", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Метод валидации вводимых данных
        void TotalPayment_TextChanged(object sender, TextCompositionEventArgs e)
        {
            try
            {
                var regex = PaymentTxtBx.Text;
                if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
                if (e.Text == ",") e.Handled = false;
            }
            catch
            {

            }
        }

        private void Check()
        {

             // HALF WORKING
             //
             //
            //var groups = Connection.bankEntities.Payment.GroupBy(currnetPayment => currnetPayment.CurPayStatus == "1");
            
            //foreach (var group in groups)
            //    MessageBox.Show($"{group.Key} {group.Count()}");
        }
        // Переход на предыдущую страницу
        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
                NavigationService.GoBack();
        }
    }
}
