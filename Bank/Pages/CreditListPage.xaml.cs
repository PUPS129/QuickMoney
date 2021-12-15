using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Bank.Pages
{
    using Model;
    public partial class CreditListPage : Page
    {   // Получение данных из БД
        public List<ServiceClient>  serviceClients { get; set; }
        public List<Client> clients { get; set; }
        public List<Payment> payments { get; set; }
        public CreditListPage()
        {
            InitializeComponent();
            try // Попытка подключения к БД
            {
                Connection.bankEntities = new Model.BankEntitiesH();
                serviceClients = Connection.bankEntities.ServiceClient.ToList();
                clients = Connection.bankEntities.Client.ToList();
                payments = Connection.bankEntities.Payment.ToList();
            }
            catch
            {
                MessageBoxResult Result = MessageBox.Show("База данных не отвечает, обратитесь к системному администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                CreditSearchTxtBx.IsEnabled = false;
            }
            DataContext = this;
            OrderComboBox.SelectedIndex = 0;
            MainListBox.Items.Refresh();
        }
        private void StatusCheck()
        {
            

            Update();
        }
        // Метод обновления данных
        private void Update()
        {
            DataContext = null;
            DataContext = this;
        }
        // Метод перехода на страницу с ежемесячными платежами
        private void WatchInfoAboutCreditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedCr = button.Tag as ServiceClient;
            if (selectedCr != null)
            {
                NavigationService.Navigate(new CreditListPageChange(selectedCr));
            }
        }
        // Метод для поиска записей
        private void CreditSearchTxtBx_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = CreditSearchTxtBx.Text;
            serviceClients = Connection.bankEntities.ServiceClient.Where(s =>
             s.Client1.Name.Contains(input) ||
             s.Client1.Surname.Contains(input) ||
             s.Client1.Patronymic.Contains(input) ||
             s.Client1.Passport.Contains(input) ||
             s.Service1.Title.Contains(input)).ToList();
            Update();
        }
        // Метод для сортировки записей
        private void SelectSort(object sender, SelectionChangedEventArgs e)
        {
            if (OrderComboBox.SelectedIndex == 0)
            {
                serviceClients = serviceClients.OrderByDescending(s => s.DateRegistr).ToList();
            }
            else if (OrderComboBox.SelectedIndex == 1)
            {
                serviceClients = serviceClients.OrderBy(s => s.DateRegistr).ToList();
            }
            Update();
        }

        private void UpdateListBox_Click(object sender, RoutedEventArgs e)
        {
            MainListBox.Items.Refresh();
        }
    }
}
