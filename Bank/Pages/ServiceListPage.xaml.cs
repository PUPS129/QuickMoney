using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Bank.Pages
{
    using Model;
    public partial class ServiceListPage : Page
    {
        public List<Service> services { get; set; }
        public ServiceListPage()
        {
            InitializeComponent();
            try
            {
                Connection.bankEntities = new Model.BankEntitiesH(); //   <----
                services = Connection.bankEntities.Service.ToList();
            }
            catch
            {
                MessageBox.Show("База данных не отвечает, обратитесь к системному администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //AddClientBtn.IsEnabled = false;
                //CllientsSearchTextBox.IsEnabled = false;
            }
            DataContext = this;
        }

        private void AddServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddServicePage());
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selected = button.Tag as Service;
            if (selected != null)
            {
                NavigationService.Navigate(new ServiceChangeInfoPage(selected));
            }
        }
    }
}
