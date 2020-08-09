using System;
using System.Globalization;
using Xamarin.Forms;

namespace Songer.UI.ViewHelpers
{
    public class IsPauseImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "play_img.png"
                               : "pause_img.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("SongCheckConverter can only be used OneWay.");
        }
    }
}
