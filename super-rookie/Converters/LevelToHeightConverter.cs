using System;
using System.Globalization;
using System.Windows.Data;

namespace super_rookie.Converters
{
    public class LevelToHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2) return 0d;
                if (values[0] == null || values[1] == null) return 0d;
                double current = System.Convert.ToDouble(values[0], CultureInfo.InvariantCulture);
                double capacity = System.Convert.ToDouble(values[1], CultureInfo.InvariantCulture);
                double maxHeight = 200d;
                if (parameter != null)
                {
                    double.TryParse(parameter.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out maxHeight);
                }
                if (capacity <= 0) return 0d;
                var ratio = Math.Max(0, Math.Min(1, current / capacity));
                return maxHeight * ratio;
            }
            catch
            {
                return 0d;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


