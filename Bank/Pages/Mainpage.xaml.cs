using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Bank.Pages
{
    using Pages.FullNewCredit;
    public partial class Mainpage : Page
    {
        public Mainpage()
        {   // Инициализация компонентов
            InitializeComponent();
        }
        // Переход на страницу для оформление кредита на уже существующего пользователя
        private void QuickCreditBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewCreditPage());
        }
        // Переход на страницу с курсами валют
        private void GoToCurrencyRatesPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CurrencyRatesPage());
        }
        // Добавление нового клиента и последующее оформление кредита на него
        private void FullNewCreditBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FULLAddNewClientPage());
        }
    }
}
