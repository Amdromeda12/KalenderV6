using System;
using System.Globalization;
using System.Windows.Data;

namespace KalenderV6.Converters
{
    public class IsNotValidDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateStr)
            {
                return !DateTime.TryParse(dateStr, out _);
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
