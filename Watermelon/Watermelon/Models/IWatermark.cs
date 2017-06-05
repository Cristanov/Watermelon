
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Watermelon.ViewModels;
namespace Watermelon.Models
{
    public interface IWatermark : IComparable<IWatermark>
    {
        string Caption { get; set; }
        double Opacity { get; set; }
        Position WatermarkPosition { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        bool IsVisible { get; set; }
        BitmapImage GetMarkImage();
        void AddWatermark(BitmapImage bmp, DrawingContext ctx);

       
    }
}
