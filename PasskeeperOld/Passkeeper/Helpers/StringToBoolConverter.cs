using System.Globalization;

namespace Passkeeper.Helpers;

public class StringToBoolConverter : IValueConverter
{
    public static StringToBoolConverter Instance { get; } = new StringToBoolConverter();
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            return !string.IsNullOrWhiteSpace(stringValue);
        }
        return false;
    }
    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 