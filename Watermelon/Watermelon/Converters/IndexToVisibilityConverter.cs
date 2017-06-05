using System;
using System.Windows;
using System.Windows.Data;

namespace Watermelon.Converters
{
    class IndexToVisibilityConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value == 0)
            {
                return Visibility.Visible;
            }
            else {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
