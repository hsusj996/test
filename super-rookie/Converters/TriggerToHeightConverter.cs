using System;
using System.Globalization;
using System.Windows.Data;

namespace super_rookie.Converters
{
    public class TriggerToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return 0d;
                double triggerAmount = System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
                double maxHeight = 200d;
                if (parameter != null)
                {
                    double.TryParse(parameter.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out maxHeight);
                }
                // Assume trigger amount is a percentage of tank capacity (0-100)
                // Convert to height position (0 = top, 100 = bottom)
                var ratio = Math.Max(0, Math.Min(1, triggerAmount / 100.0));
                return maxHeight * (1 - ratio); // Invert so 0% = top, 100% = bottom
            }
            catch
            {
                return 0d;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
