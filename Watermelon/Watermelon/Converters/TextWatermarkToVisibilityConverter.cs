
using System;
using System.Windows;
using System.Windows.Data;
using Watermelon.Models;
namespace Watermelon.Converters
{
    class TextWatermarkToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                
                if (value is TextWatermark)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                } 
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
