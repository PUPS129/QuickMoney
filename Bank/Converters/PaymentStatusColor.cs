using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Bank.Converters
{
    class PaymentStatusColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //int status = (int)value;

            //if (status == 0)
            //{
            //    return "Yellow";
            //}
            //else if (status == 2)
            //{
            //    return "Red";
            //}
            //else
            //{
            //    return "Green";
            //}
            throw new NotImplementedException();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
