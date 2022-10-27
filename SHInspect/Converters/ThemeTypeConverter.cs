using ModernWpf;
using SHInspect.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SHInspect.Converters
{
    public class ThemeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ThemeType && value != null)
            {
                ThemeType status = (ThemeType)value;
                var statusString = status == ThemeType.Automatic ? ThemeManager.Current.ActualApplicationTheme.ToString() : status.ToString();
                return statusString;
            }

            return ThemeType.Light.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
