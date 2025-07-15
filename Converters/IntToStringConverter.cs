using System;
using System.Globalization;
using System.Windows.Data;

namespace KalenderV6.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue == 0 ? string.Empty : intValue.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue && int.TryParse(strValue, out int result))
            {
                return result;
            }
            return 0;
        }
    }
}
