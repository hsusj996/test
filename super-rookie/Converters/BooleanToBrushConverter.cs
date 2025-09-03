using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace super_rookie.Converters
{
    public class BooleanToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string colorString)
            {
                var colors = colorString.Split(':');
                if (colors.Length == 2)
                {
                    return boolValue ? new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[0])) 
                                     : new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[1]));
                }
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
