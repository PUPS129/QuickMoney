using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bank.Pages
{
    public partial class CalculatePage : Page
    {
        public CalculatePage()
        {
            InitializeComponent();
            this.TotalAmountTxtBx.PreviewTextInput += new TextCompositionEventHandler(TotalAmountTxtBx_TextChanged);
            this.RatePercentTxtBx.PreviewTextInput += new TextCompositionEventHandler(RatePercentTxtBx_TextChanged);

        }
        // Метод подсчёта ежемесячного платежа
        private void CountCreditAmountBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TotalAmountTxtBx.Text.Length == 0 || TotalAmountTxtBx.Text == "0")
            {
                MessageBox.Show("Введите сумму кредита", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (RatePercentTxtBx.Text.Length == 0 || RatePercentTxtBx.Text == "0")
            {
                MessageBox.Show("Введите процентую ставку", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                double Amount = Convert.ToDouble(TotalAmountTxtBx.Text);
                double PercentRate = Convert.ToDouble(RatePercentTxtBx.Text);
                double Duration;
                if (DurationCmbBx.SelectedIndex == 0)
                {
                    Duration = 1;
                }
                else if (DurationCmbBx.SelectedIndex == 1)
                {
                    Duration = 2;
                }
                else if (DurationCmbBx.SelectedIndex == 2)
                {
                    Duration = 3;
                }
                else if (DurationCmbBx.SelectedIndex == 3)
                {
                    Duration = 4;
                }
                else if (DurationCmbBx.SelectedIndex == 4)
                {
                    Duration = 5;
                }
                else
                {
                    MessageBox.Show("Выберите срок кредита", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                double MonthPay = (Amount + Amount * PercentRate * Duration / 100) / (Duration * 12);
                double MonthPayResult = Math.Round(MonthPay, 2);
                ResultTxtBx.Text = MonthPayResult.ToString() + " ₽/мес";
            }
            catch
            {
            }
        }
        // Метод валидации вводмых данных
         void TotalAmountTxtBx_TextChanged(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
            }
            catch
            {
            }
        }
        // Метод валидации вводмых данных
        void RatePercentTxtBx_TextChanged(object sender, TextCompositionEventArgs e)
        {
            try
            {
                var regex = RatePercentTxtBx.Text;
                if (RatePercentTxtBx.Text.Length > 2) e.Handled = true;
                if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
                if (e.Text == ",") e.Handled = false;
            }
            catch
            {
            }
        }
    }
}
