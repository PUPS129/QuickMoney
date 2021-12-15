using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Bank.Pages;

namespace Bank
{
    public partial class MainWindow : Window
    {
        bool restoreIfMove = false;
        public MainWindow()
        { // Инициализация компонентов и автоматический переход на главную страницу
            InitializeComponent();
            Mainframe.Navigate(new Mainpage());
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }
        // Пункт меню для перехода на главную страницу
        private void MainPageBtn_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new Mainpage());
        }
        // Пункт меню для перехода на страницу с клиентами
        private void ClientsPageBtn_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new Clientspage());
        }
        // Пункт меню для перехода на страницу с калькулятором ежемесячных платежей
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new CalculatePage());
        }
        // Пункт меню для перехода на страницу с оформленными кредитами
        private void CreditListPageBtn_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new CreditListPage());
        }
        // Пункт меню для перехода на страницу с кредитными решениями
        private void CreditSolutionsBtn_Click(object sender, RoutedEventArgs e)
        {
            Mainframe.Navigate(new ServiceListPage());
        }
        // Пункт меню для выхода из приложения с подтверждением
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
        // Метод реализующий перетаскивание окна
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if ((ResizeMode == ResizeMode.CanResize) ||
                    (ResizeMode == ResizeMode.CanResizeWithGrip))
                {
                    SwitchState();
                }
            }
            else
            {
                if (WindowState == WindowState.Maximized)
                {
                    restoreIfMove = true;
                }

                DragMove();
            };
        }
        // Метод реализующий сворачивание окна
        private void TurnBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ResizeWindow_Click(object sender, RoutedEventArgs e)
        {
            SwitchState();
        }

        private void SwitchState()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        WindowState = WindowState.Maximized;
                        break;
                    }
                case WindowState.Maximized:
                    {
                        WindowState = WindowState.Normal;
                        break;
                    }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.BorderThickness = new System.Windows.Thickness(7);
                this.ResizeMode = ResizeMode.CanResize;
            }
            else
            {
                this.BorderThickness = new System.Windows.Thickness(0);
                this.ResizeMode = ResizeMode.CanResize;
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            restoreIfMove = false;
        }

        private void LockAppBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Result = MessageBox.Show("Вы действительно хотите заблокировать приложение?\nСеанс этого пользователя будет завершен.", "Блокировка", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                LoginWindow taskWindow = new LoginWindow();
                taskWindow.Show();
                this.Close();
            }
            else if (Result == MessageBoxResult.No)
            {

            }


        }
    }
}
