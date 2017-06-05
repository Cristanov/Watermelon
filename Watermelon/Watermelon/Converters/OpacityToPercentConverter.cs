using System;
using System.Windows.Data;

namespace Watermelon.Converters
{
    class OpacityToPercentConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value * 100.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value / 100.0;
        }
    }
}
