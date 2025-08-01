using System.Globalization;

namespace Passkeeper.Helpers
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isExpiringSoon)
            {
                return isExpiringSoon ?
                    Color.FromArgb("#EF4444") : // Red for expiring soon
                    Color.FromArgb("#10B981");  // Green for normal
            }
            return Color.FromArgb("#10B981"); // Default to green
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}