using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Passkeeper.Helpers
{
    public class FirstLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
            {
                // Extract domain from URL if it's a URL
                if (str.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var uri = new Uri(str);
                        var host = uri.Host;
                        return host.Length > 0 ? host[0].ToString().ToUpper() : "W";
                    }
                    catch
                    {
                        return str[0].ToString().ToUpper();
                    }
                }
                return str[0].ToString().ToUpper();
            }
            return "P";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 