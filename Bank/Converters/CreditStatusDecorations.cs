using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Bank.Converters
{
    class CreditStatusDecorations: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (string)value;
            if (status == "0")
            {
                return TextDecorations.Underline;
            }
            else if (status == "2")
            {
                return new TextDecoration();
            }
            else
            {
                return TextDecorations.Strikethrough;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
