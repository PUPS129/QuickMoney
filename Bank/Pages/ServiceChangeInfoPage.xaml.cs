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
    public partial class ServiceChangeInfoPage : Page
    {
        private Service originalService;
        private Client selected;
        public List<Service> services { get; set; }
        public Service currentService { get; set; }
        //public List<Client> clients { get; set; }
        public ServiceChangeInfoPage(Service service)
        {

            InitializeComponent();
            originalService = service;
            services = Connection.bankEntities.Service.ToList();
            //clients = Connection.bankEntities.Client.ToList();

            currentService = new Service
            {
                Title = originalService.Title,
                PercentRate = originalService.PercentRate,
                MaximumDuration = originalService.MaximumDuration,
                ServiceID = -1
            };
            DataContext = this;
        }
        public ServiceChangeInfoPage(Client selected)
        {
            this.selected = selected;
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServiceListPage());
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentService.Title == "")
            {
                MessageBox.Show("Введите наименование услуги");
                return;
            }
            if (currentService.PercentRate == "")
            {
                MessageBox.Show("Введите процентную ставку");
                return;
            }
            if (currentService.MaximumDuration == "")
            {
                MessageBox.Show("Введите срок кредита");
                return;
            }
            
            MessageBoxResult Result = MessageBox.Show("Подтвердите верность введненных данных:\n" +
                    $"\nНаименование услуги - {currentService.Title}\n" +
                    $"Процентная ставка - {currentService.PercentRate}\n" +
                    $"Срок кредита - {currentService.MaximumDuration}\n", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
            {
                originalService.Title = currentService.Title;
                originalService.PercentRate = currentService.PercentRate;
                originalService.MaximumDuration = currentService.MaximumDuration;
            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                Connection.bankEntities.SaveChanges();
                MessageBox.Show("Успешно сохранено", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new ServiceListPage());
            }

            catch
            {
                MessageBox.Show("Ошибка сохранения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
