
using System;
using System.Windows.Data;
using Watermelon.Models;
namespace Watermelon.Converters
{
    class WatermarkToImageSource :IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TextWatermark)
            {
                return new Uri(@"../Images/letterst.png", UriKind.Absolute);
            }
            else
            {
                return @"../Images/landscape.png";
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
