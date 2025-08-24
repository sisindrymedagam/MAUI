using System.Globalization;

namespace Looply.MAUI.Converters;

public class AspectRatioConverter : IValueConverter
{
    // ConverterParameter = "9:16" (width:height)
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double height && parameter is string ratio)
        {
            string[] parts = ratio.Split(':');
            if (parts.Length == 2 &&
                double.TryParse(parts[0], out double w) &&
                double.TryParse(parts[1], out double h))
            {
                return height * (w / h); // keep 9:16 ratio
            }
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
