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
    public partial class ClientChangeInfoPage : Page
    {
        private Client originalClient;
        //private Service selected;
        public Client currentClient { get; set; }

        public List<Service> service { get; set; }
        public List<Client> clients { get; set; }


        public ClientChangeInfoPage(Client client)
        {
            InitializeComponent();
            originalClient = client;

            service = Connection.bankEntities.Service.ToList();
            clients = Connection.bankEntities.Client.ToList();
            currentClient = new Client
            {
                Surname = originalClient.Surname,
                Name = originalClient.Name,
                Patronymic = originalClient.Patronymic,
                Phone = originalClient.Phone,
                Birthday = originalClient.Birthday,
                Passport = originalClient.Passport,

                ClientID = -1
            };


            DataContext = this;
            this.NameTxtBx.PreviewTextInput += new TextCompositionEventHandler(LettersOnly);
            this.SurnameTxtBx.PreviewTextInput += new TextCompositionEventHandler(LettersOnly);
            this.PatronymicTxtBx.PreviewTextInput += new TextCompositionEventHandler(LettersOnly);
            this.PhoneTxtBx.PreviewTextInput += new TextCompositionEventHandler(PhoneBox);
            this.PassportTxtBx.PreviewTextInput += new TextCompositionEventHandler(DigitsOnly);
            this.BirthdayDataPick.PreviewTextInput += new TextCompositionEventHandler(DatePick);
        }

        //public ClientChangeInfoPage(Service selected)
        //{
        //    this.selected = selected;
        //}
        private void Update()
        {
            DataContext = null;
            DataContext = this;
        }
        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Clientspage());
        }

        private void SaveInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentClient.Surname == "")
            {
                MessageBox.Show("Введите Фамилию");
                return;
            }
            if (currentClient.Name == "")
            {
                MessageBox.Show("Введите Имя");
                return;
            }
            if (currentClient.Patronymic == "")
            {
                MessageBox.Show("Введите Отчество");
                return;
            }
            if (currentClient.Phone == "")
            {
                MessageBox.Show("Введите Телефон");
                return;
            }
            if (currentClient.Birthday.HasValue == false)
            {
                MessageBox.Show("Введите Дату рождения");
                return;
            }
            if (currentClient.Passport == "")
            {
                MessageBox.Show("Введите Номер паспорта");
                return;
            }
            if (PassportTxtBx.Text.Length != 10)
            {
                MessageBox.Show("Введите 10 цифр паспорта");
                return;
            }
            MessageBoxResult Result = MessageBox.Show("Подтвердите верность введненных данных:\n" +
                    $"\nФамилия - {currentClient.Surname}\n" +
                    $"Имя - {currentClient.Name}\n" +
                    $"Отчество - {currentClient.Patronymic}\n" +
                    $"Номер телефона - {currentClient.Phone}\n" +
                    $"Дата рождения - {currentClient.Birthday.Value.Day}.{currentClient.Birthday.Value.Month}.{currentClient.Birthday.Value.Year}\n" +
                    $"Номер паспорта - {currentClient.Passport}", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
            {
                originalClient.Surname = currentClient.Surname;
                originalClient.Name = currentClient.Name;
                originalClient.Patronymic = currentClient.Patronymic;
                originalClient.Phone = currentClient.Phone;
                originalClient.Birthday = currentClient.Birthday;
                originalClient.Passport = currentClient.Passport;
            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                Connection.bankEntities.SaveChanges();
                MessageBox.Show("Успешно сохранено", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new Clientspage());
            }

            catch
            {
                MessageBox.Show("Ошибка сохранения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void DelBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        MessageBoxResult Result = MessageBox.Show("Вы действительно хотите удалить этого клиента?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (Result == MessageBoxResult.Yes)
        //        {
        //            var button = sender as Button;
        //            //if (button != null)
        //            //{

        //            //}
        //            int id = originalClient.ClientID;

        //            Client toRemove = Connection.bankEntities.Client.Find(id);
        //            Connection.bankEntities.Client.Remove(toRemove);
        //            Connection.bankEntities.SaveChanges();
        //            Update();
                    
        //            MessageBox.Show("Успешно удалено", "", MessageBoxButton.OK, MessageBoxImage.Information);
        //            NavigationService.Navigate(new Mainpage());
        //        }
        //        else if (Result == MessageBoxResult.No)
        //        {

        //        }
        //}
        //    catch
        //    {
        //        MessageBox.Show($"Ошибка удаления{originalClient.Name}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
//}

        private void LettersOnly(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (Char.IsDigit(e.Text, 0)) e.Handled = true;
            }
            catch
            {
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
        private void DatePick(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
                if (e.Text == ".") e.Handled = false;
            }
            catch
            {

            }
        }
        private void PhoneBox(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
                if (e.Text == "+") e.Handled = false;
                
            }
            catch
            {

            }
        }
        //private void SurnameTxtBx_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.LeftAlt | e.Key == Key.D1)
        //    {
        //        e.Handled = true;
        //    }
        //}

    }
}
