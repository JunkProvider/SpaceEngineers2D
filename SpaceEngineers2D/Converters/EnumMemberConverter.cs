using System;
using System.Globalization;
using System.Windows.Data;

namespace SpaceEngineers2D.Converters
{
    public class EnumMemberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new ArgumentException(@"Can only convert to string.", nameof(targetType));
            }

            if (value == null)
            {
                return string.Empty;
            }
            
            try
            {
                return Enum.GetName(value.GetType(), value);
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
