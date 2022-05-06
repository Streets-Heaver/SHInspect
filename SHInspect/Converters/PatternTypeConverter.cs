using SHInspect.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SHInspect.Converters
{
    public class PatternTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PatternType && value != null)
            {
                PatternType status = (PatternType)value;
                var statusString = status.ToString();
                return statusString;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
