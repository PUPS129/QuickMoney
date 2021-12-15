using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Bank.Pages
{
    using Model;
    public partial class Clientspage : Page
    {
        public List<Client> clients { get; set; }
        public Clientspage()
        {
            InitializeComponent();
            Update();
            try
            {
                Connection.bankEntities = new Model.BankEntitiesH();
                clients = Connection.bankEntities.Client.ToList();
            }
            catch
            {
                MessageBox.Show("База данных не отвечает, обратитесь к системному администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                AddClientBtn.IsEnabled = false;
                CllientsSearchTextBox.IsEnabled = false;
            }

            DataContext = this;
        }
        private void Update()
        {
            DataContext = null;
            DataContext = this;
        }

        private void AddClientBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddClientBtn());
        }

        private void DeleteClientBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult Result = MessageBox.Show("Вы действительно хотите удалить этого клиента?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    var button = sender as Button;
                    int id = (int)button.Tag;
                    Client toRemove = Connection.bankEntities.Client.Find(id);
                    Connection.bankEntities.Client.Remove(toRemove);
                    clients.Remove(toRemove);
                    Update();
                    Connection.bankEntities.SaveChanges();
                }
                else if (Result == MessageBoxResult.No)
                {

                }


            }
            catch
            {
                MessageBox.Show("Ошибка удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CllientsSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = CllientsSearchTextBox.Text;
            clients = Connection.bankEntities.Client.Where(s =>
             s.Name.Contains(input) ||
             s.Surname.Contains(input) ||
             s.Patronymic.Contains(input) ||
             s.Passport.Contains(input) ||
             s.Phone.Contains(input)).ToList();
            Update();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selected = button.Tag as Client;
            if (selected != null)
            {
                NavigationService.Navigate(new ClientChangeInfoPage(selected));
            }
        }
    }
}
