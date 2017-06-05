
using System.Windows.Data;
namespace Watermelon.Converters
{
    class CountToBoolConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
