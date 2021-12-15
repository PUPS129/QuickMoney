using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bank.Converters
{
    class YearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string year = value.ToString().Trim();
            string yearConv;

            if (year.EndsWith("1") && year!="11")
            {
                yearConv = $"{year} год";
                return yearConv;
            }
            else if (year.EndsWith("2") || year.EndsWith("3") || year.EndsWith("4"))
            {
                yearConv = $"{year} года";
                return yearConv;
            }
            else
            {
                yearConv = $"{year} лет";
                return yearConv;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
