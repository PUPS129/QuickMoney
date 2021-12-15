using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Bank.Pages
{
    using Microsoft.Win32;
    using Model;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Media.Imaging;

    public partial class AddClientBtn : Page
    {
        string photoname { get; set; }
        public List<Client> clients { get; set; }
        
        public AddClientBtn()
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
        //Переход на страницу с клиентами
        private void GoBackBtn(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Clientspage());
        }
        // Метод добавления нового клиента
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
                MessageBox.Show("Выберите дату рождения", "", MessageBoxButton.OK , MessageBoxImage.Information);
                return;
            }
            if (BirthdayDataPick.SelectedDate > DateTime.Now.AddYears(-18))
            {
                MessageBox.Show("Клиент несовершеннолетний\n Добавление невозможно","",MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
            newClient.Name = NameTxtBx.Text;
            newClient.Surname = SurnameTxtBx.Text;
            newClient.Patronymic = PatronymicTxtBx.Text;
            newClient.Phone = PhoneTxtBx.Text;
            newClient.Birthday = BirthdayDataPick.SelectedDate;
            newClient.Passport = PassportTxtBx.Text;
            newClient.Photo = photoname;
            if(tbImage.Text != "")
            {
                //string exp = Assembly.GetExecutingAssembly().;
                string exepath = Assembly.GetExecutingAssembly().Location;
                string salt = string.Empty;
                //if (File.Exists($"{Path.GetDirectoryName(exepath)}\\Images\\ClientsPhoto\\{Path.GetFileName(tbImage.Text)}"))
                //{
                //    salt = DateTime.Now.ToString("yyyy.MM.dd HH.mm.ss").Replace(".", "");
                //}
                //if (!Directory.Exists("ClientPhotos"))
                //{
                //    Directory.CreateDirectory("ClientPhotos");
                //}
                File.Copy(tbImage.Text, $"{Path.GetDirectoryName(exepath)}\\Images\\ClientPhotos\\{salt + Path.GetFileName(tbImage.Text)}");
                newClient.Photo = photoname;
            }
            //if (clients.Contains(newClient.Passport) == true)
            //{
            //    MessageBox.Show("aboba");
            //}
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
                {  // Добавление данных клиента в базу данных
                    Connection.bankEntities.Client.Add(newClient);
                    Connection.bankEntities.SaveChanges();
                    MessageBox.Show("Клиент успешно добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new Clientspage());
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
        // Метод для валидации вводимых данных
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
        // Метод для валидации вводимых данных
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
        // Метод для валидации вводимых данных
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
        // Метод для валидации вводимых данных
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
        // Метод для выбора фотографий клиентов
        private void PhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            //string curr = Environment.CurrentDirectory;
            //MessageBox.Show(curr);




            ////OpenFileDialog openFileDialog = new OpenFileDialog();
            ////openFileDialog.Multiselect = false;
            ////openFileDialog.Filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg|jpeg files (*.jpeg)|*.jpeg";
            ////openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ////if (openFileDialog.ShowDialog() == true)
            ////{
            ////    var newClient = new Client();
            ////    photoname = openFileDialog.SafeFileName;


            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            filedialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if ((bool)filedialog.ShowDialog())
            {
                photoname = filedialog.SafeFileName;
                tbImage.Text = filedialog.FileName;
                ImgProf.ImageSource = new BitmapImage(new Uri($"{filedialog.FileName}"));
            }
            else
            {
                return;
            }
        }
    }
}
