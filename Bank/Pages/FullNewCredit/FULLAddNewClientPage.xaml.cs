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
    using Microsoft.Win32;
    using Model;
    public partial class FULLAddNewClientPage : Page
    {
        string photoname { get; set; }
        public FULLAddNewClientPage()
        {
            InitializeComponent();
            Connection.bankEntities = new Model.BankEntitiesH();
            this.NameTxtBx.PreviewTextInput += new TextCompositionEventHandler(LettersOnly);
            this.SurnameTxtBx.PreviewTextInput += new TextCompositionEventHandler(LettersOnly);
            this.PatronymicTxtBx.PreviewTextInput += new TextCompositionEventHandler(LettersOnly);
            this.PhoneTxtBx.PreviewTextInput += new TextCompositionEventHandler(PhoneBox);
            this.PassportTxtBx.PreviewTextInput += new TextCompositionEventHandler(DigitsOnly);
            this.BirthdayDataPick.PreviewTextInput += new TextCompositionEventHandler(DatePick);
        }
        private void GoBackBtn(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Mainpage());
        }

        private void AddNewClient(object sender, RoutedEventArgs e)
        {
            var newClient = new Client();
            if (SurnameTxtBx.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Поле 'фамилия' не может быть пустым", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (NameTxtBx.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Поле 'имя' не может быть пустым", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (PatronymicTxtBx.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Поле 'отчество' не может быть пустым", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (PhoneTxtBx.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Введите номер телефона", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (!BirthdayDataPick.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату рождения", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (BirthdayDataPick.SelectedDate > DateTime.Now.AddYears(-18))
            {
                MessageBox.Show("Клиент несовершеннолетний\n Добавление невозможно", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (PassportTxtBx.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Введите номер паспорта", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (PassportTxtBx.Text.Length != 10)
            {
                MessageBox.Show("Введите 10 цифр паспорта", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //using (StreamWriter sw = new StreamWriter($@"..\Image\ClientsPhoto\{NameTxtBx.Text}.png"))
            newClient.Name = NameTxtBx.Text;
            newClient.Surname = SurnameTxtBx.Text;
            newClient.Patronymic = PatronymicTxtBx.Text;
            newClient.Phone = PhoneTxtBx.Text;
            newClient.Birthday = BirthdayDataPick.SelectedDate;
            newClient.Passport = PassportTxtBx.Text;
            newClient.Photo = photoname;

            try
            {
                MessageBoxResult Result = MessageBox.Show($"Проверьте верность введенных данных:\n" +
                    $"\n {SurnameLbl.Text} - {SurnameTxtBx.Text}" +
                    $"\n {NameLbl.Text} - {NameTxtBx.Text}" +
                    $"\n {PatronymicLbl.Text} - {PatronymicTxtBx.Text}" +
                    $"\n {PhoneLbl.Text} - {PhoneTxtBx.Text}" +
                    $"\n {BirthdayLbl.Text} - {newClient.Birthday.Value.Day}.{newClient.Birthday.Value.Month}.{newClient.Birthday.Value.Year}" +
                    $"\n {PassportLbl.Text} - {PassportTxtBx.Text}", "Проверка", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (Result == MessageBoxResult.Yes)
                {
                    Connection.bankEntities.Client.Add(newClient);
                    Connection.bankEntities.SaveChanges();
                    MessageBox.Show("Клиент успешно добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    var button = sender as Button;
                    var selected = newClient as Client;
                    if (selected != null)
                    {
                        NavigationService.Navigate(new FULLNewCreditPage(selected));
                    }
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

        private void PhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg|jpeg files (*.jpeg)|*.jpeg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                var newClient = new Client();
                photoname = openFileDialog.SafeFileName;


                //sw.Write(textBox.Text);
                //SaveFileDialog saveFileDialog = new SaveFileDialog();
                //if (saveFileDialog.ShowDialog() == true)
                //    saveFileDialog.InitialDirectory = $"../Images/ClientsPhoto/";
            }
        }
    }
}
