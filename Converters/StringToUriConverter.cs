using System.Globalization;
using System.Windows.Data;

namespace KalenderV6.Converters
{
    public class StringToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string uriString && !string.IsNullOrWhiteSpace(uriString))
            {
                return new Uri(uriString, UriKind.RelativeOrAbsolute);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Uri uri)
            {
                return uri.ToString();
            }
            return null;
        }
    }
}
