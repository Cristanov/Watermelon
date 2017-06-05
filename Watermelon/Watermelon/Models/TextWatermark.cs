using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Watermelon.ViewModels;
using Watermelon.Properties;
using System;
using System.Xml.Serialization;


namespace Watermelon.Models
{
    
    public class TextWatermark : Watermark
    {
        public TextWatermark()
        {
            Caption = Resources.exampleText;
            Color = Colors.Black;
            Font = new FontFamily("Times New Roman");
            Opacity = 0.6;
            FontSize = 100;
            WatermarkPosition = Position.CENTER;
            IsVisible = true;
        }
        public int FontSize { get; set; }

        public System.Windows.Media.Color Color { get; set; }

        [XmlIgnore]
        public System.Windows.Media.FontFamily Font { get; set; }

        public string FontName
        {
            get
            {
                return Font.ToString();
            }
            set
            {
                Font = new FontFamily(value);
            }
        }

        public override System.Windows.Media.Imaging.BitmapImage GetMarkImage()
        {
            return new BitmapImage();
            
        }

        public override void AddWatermark(BitmapImage bmp, DrawingContext drawingContext)
        {
            if (IsVisible)
            {
                int width = (int)bmp.PixelWidth;
                int height = (int)bmp.PixelHeight;
                Brush brush = new SolidColorBrush(Color);
                brush.Opacity = Opacity;
                FontSize = FontSize <= 0 ? 1 : FontSize;

                FormattedText formatedText = new FormattedText(Caption, CultureInfo.InvariantCulture, System.Windows.FlowDirection.LeftToRight,
                    new Typeface(Font.ToString()), FontSize, brush);
                Point drawingPoint = base.GetDrawPoint((int)formatedText.Width, (int)formatedText.Height, WatermarkPosition, width, height);

                drawingContext.DrawText(formatedText, drawingPoint);
            }
        }

        public override int CompareTo(IWatermark obj)
        {
            if (!(obj is TextWatermark))
            {
                return -1;
            }
            TextWatermark objTw = obj as TextWatermark;
            if (Caption != objTw.Caption ||
                Color != objTw.Color ||
                FontName != objTw.FontName ||
                FontSize != objTw.FontSize ||
                Opacity != objTw.Opacity ||
                WatermarkPosition != objTw.WatermarkPosition ||
                IsVisible != objTw.IsVisible
                )
            {
                return 1;
            }
            return 0;
        }
    }
}
