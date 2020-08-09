using System;
using System.Globalization;
using Xamarin.Forms;

namespace Songer.UI.ViewHelpers
{
    public class SongCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "successfull_img.png" 
                               : "add_img.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("SongCheckConverter can only be used OneWay.");
        }
    }
}