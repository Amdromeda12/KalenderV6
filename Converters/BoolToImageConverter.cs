using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace KalenderV6.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public string TrueImage { get; set; }
        public string FalseImage { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isExpanded)
            {
                var imageUri = isExpanded ? TrueImage : FalseImage;
                if (string.IsNullOrEmpty(imageUri))
                {
                    throw new ArgumentException("Image URI is not specified.");
                }

                try
                {
                    return new BitmapImage(new Uri(imageUri, UriKind.Absolute));
                }
                catch (UriFormatException ex)
                {
                    throw new UriFormatException("Invalid URI format.", ex);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
