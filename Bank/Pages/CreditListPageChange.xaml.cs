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
    using System.Collections.ObjectModel;
    public partial class CreditListPageChange : Page
    {   // Получение данных из Бд
        private ServiceClient originalServiceClient;
        public ServiceClient currentServiceClient { get; set; }
        public ServiceClient serviceClient { get; set; }
        public Payment currnetPayment { get; set; }
        public List<Payment> payments { get; set; }
        public CreditListPageChange(Model.ServiceClient selected)
        {
            InitializeComponent();
            MainDataGrid.ItemsSource = payments;
            // Передача данных выбранного клиента
            originalServiceClient = selected;
            currentServiceClient = new ServiceClient()
            {
                ServiceClientID = originalServiceClient.ServiceClientID,
                Service = originalServiceClient.Service,
                Amount = originalServiceClient.Amount,
                Client = originalServiceClient.Client,
                DateRegistr = originalServiceClient.DateRegistr,
                DateExpires = originalServiceClient.DateExpires,
                Status = originalServiceClient.Status
            };
            DataContext = this;
            paymentFiltered();
            StsCheck();
            SurnameTxtBl.Text = originalServiceClient.Client1.Surname;
            NameTxtBl.Text = originalServiceClient.Client1.Name;
            PatronymicTxtBl.Text = originalServiceClient.Client1.Patronymic;
        }
        // Фильтрация значений в зависимости от выбранного платежа
        private void paymentFiltered()
        {
            var cr = currentServiceClient.ServiceClientID;
            var paym = Connection.bankEntities.Payment.Where(s =>
            s.ServiceClientID == cr).ToList();
            MainDataGrid.ItemsSource = paym;

            //int countPays = paym.Count();
            //MessageBox.Show(countPays.ToString());

            if (paym.All(s => s.CurPayStatus.ToString() == "1"))
            {
                //currentServiceClient.Status = "1";
                originalServiceClient.Status = "1";
                Connection.bankEntities.SaveChanges();
            }
            //else if (paym.All(s => s.Date.Date < DateTime.Now && Convert.ToDouble(s.Amount) < Convert.ToDouble(1)))
            //{
            //    currnetPayment.CurPayStatus = "2";
            //    originalServiceClient.Status = "2";
            //    Connection.bankEntities.SaveChanges();
            //}
            Update();
        }

        private void StsCheck()
        {
            
        }
        // Метод обновления контекста данных
        private void Update()
        {
            DataContext = null;
            DataContext = this;
        }
        // Кнопка для перехода на форму оплаты выбранного платежа 
        private void PayBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selected = button.Tag as Payment;

            if (selected != null)
            {
                NavigationService.Navigate(new PayCreditPage(selected));
            }
        }
        // Переход на страницу с кредитами
        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            paymentFiltered();
            NavigationService.Navigate(new CreditListPage());
        }
        // Обновления данных в datagrid 
        private void Updt_Click(object sender, RoutedEventArgs e)
        {
            MainDataGrid.Items.Refresh();
        }
    }
}
