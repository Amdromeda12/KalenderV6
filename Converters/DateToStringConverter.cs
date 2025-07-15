using System;
using System.Globalization;
using System.Windows.Data;

namespace KalenderV6.Converters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("yyyy-MM-dd");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                if (DateTime.TryParse(dateString, out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            return DateTime.Now;
        }
    }
}
