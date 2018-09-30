using System;
using System.Globalization;
using System.Windows.Data;

namespace SpaceEngineers2D.Converters
{
    public class BooleanConverter : IValueConverter
    {
        public object TrueInput { get; set; } = true;
        public object FalseInput { get; set; } = false;
        public object TrueOutput { get; set; } = true;
        public object FalseOutput { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
