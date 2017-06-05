
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Watermelon.ViewModels;
using Watermelon.Properties;

namespace Watermelon.Models
{
    public class GraphicWatermark : Watermark
    {
        private string imagePath;
        public GraphicWatermark()
        {
            Caption = Resources.exampleImage;
            Height = 100;
            Width = 100;
            ImagePath = null;
            Opacity = 0.6;
            WatermarkPosition = Position.TOPLEFT;
            IsVisible = true;
        }

        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
            }
        }

        public override System.Windows.Media.Imaging.BitmapImage GetMarkImage()
        {
            //return new BitmapImage(new System.Uri(@"pack://application:,,,/Images/landscape.png"));
            return new BitmapImage();
        }

        public override void AddWatermark(BitmapImage bmp, DrawingContext ctx)
        {
            if (IsVisible)
            {
                BitmapImage bmpMark = new BitmapImage(new System.Uri(ImagePath, System.UriKind.Absolute));
                Point drawPoint = base.GetDrawPoint((int)Width, (int)Height, WatermarkPosition, bmp.PixelWidth, bmp.PixelHeight);
                Point drawPoint2 = new Point(drawPoint.X + Width, drawPoint.Y + Height);

                ctx.PushOpacity(Opacity);
                ctx.DrawImage(bmpMark, new Rect(drawPoint, drawPoint2));
                ctx.Pop(); 
            }
        }

        public override int CompareTo(IWatermark obj)
        {
            if (!(obj is GraphicWatermark))
            {
                return -1;
            }
            GraphicWatermark objGw = obj as GraphicWatermark;
            if (Caption != objGw.Caption ||
                Height != objGw.Height ||
                ImagePath != objGw.ImagePath ||
                IsVisible != objGw.IsVisible ||
                Opacity != objGw.Opacity ||
                WatermarkPosition != objGw.WatermarkPosition ||
                Width != objGw.Width
                )
            {
                return 1;
            }
            return 0;
        }
    }
}
