using System.Globalization;

namespace Passkeeper.Helpers;

public class StringToBoolConverter : IValueConverter
{
    public static StringToBoolConverter Instance { get; } = new StringToBoolConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string stringValue ? !string.IsNullOrWhiteSpace(stringValue) : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}