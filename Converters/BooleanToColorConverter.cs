using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace KalenderV6.Converters
{
    public class BooleanToColorConverter : IValueConverter
    {
        public Brush CheckedColor { get; set; } = Brushes.Blue;
        public Brush UncheckedColor { get; set; } = Brushes.Black;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked)
            {
                return isChecked ? CheckedColor : UncheckedColor;
            }
            return UncheckedColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
