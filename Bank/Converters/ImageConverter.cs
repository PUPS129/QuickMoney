using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bank.Converters
{
    class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value == null) return "";
            //string filename = Path.GetFileName(value.ToString());
            //string path = $"pack:/siteoforigin:,,,/ClientPhotos/{filename}";
            //return path;



            string img = (string)value;
            if (img != null)
            {
                //return $"./bin/Debug/ClientPhotos/{value}";
                return $"../Images/ClientsPhoto/{value}";
            }
            else
            {
                return $"../Images/profile.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
