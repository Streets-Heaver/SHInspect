using System;
using System.Globalization;
using System.Windows.Data;

namespace SHInspect.Converters
{
    public class NumericValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert the double value to a string
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Try to parse the input string as a double
            if (double.TryParse((string)value, out double result) && result > 0)
            {
                // Return the double value if it is valid and greater than zero
                return result;
            }
            else
            {
                // Otherwise, return 0
                return 0;
            }
        }
    }
}
