using System;
using System.Globalization;
using System.Windows.Data;

namespace KalenderV6.Converters
{
    public class IsValidDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateStr)
            {
                // Check if the string can be parsed to a valid DateTime
                return DateTime.TryParse(dateStr, out _);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
