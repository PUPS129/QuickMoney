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

namespace Bank
{
    using Model;
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Data.SqlClient;
    

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }


        // To Do - Реализовать нормальный (!!!) логин в систему
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTxtBx.Text.Length == 0)
            {
                MessageBox.Show("Введите логин", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (PasswordTxtBx.Password.Length == 0)
            {
                MessageBox.Show("Введите пароль", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            string login = LoginTxtBx.Text;
            string password = PasswordTxtBx.Password;

            //LoginConnection loginConnection = new LoginConnection();

            //DataTable table = new DataTable();

            //MySqlDataAdapter adapter = new MySqlDataAdapter();

            //MySqlCommand command = new MySqlCommand("SELECT * FROM `AdminLogPass` WHERE `Login` = @UL AND `Password` = @UP", loginConnection.getConnection());

            //command.Parameters.Add("@UL", MySqlDbType.VarChar).Value = login;
            //command.Parameters.Add("@UP", MySqlDbType.VarChar).Value = password;

            //adapter.SelectCommand = command;
            //adapter.Fill(table);

            //if (table.Rows.Count > 0)
            //{
            //    MainWindow taskWindow = new MainWindow();
            //    taskWindow.Show();
            //    this.Close();
            //}
            //else
            //{
            //    MessageBox.Show("err");
            //}

            if (login == "Admin" &&  password == "1234" || login == "Manager" && password == "1111")
            {
                MessageBox.Show($"Успешная авторизация как {login}","Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow taskWindow = new MainWindow();
                taskWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Result = MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
            else if (Result == MessageBoxResult.No)
            {

            }
        }
    }
}
