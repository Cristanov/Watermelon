using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace Watermelon.Models
{
    public abstract class Watermark : IWatermark
    {
        public string Caption { get; set; }

        public double Opacity { get; set; }

        public Position WatermarkPosition { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public bool IsVisible { get; set; }
        abstract public System.Windows.Media.Imaging.BitmapImage GetMarkImage();

        abstract public void AddWatermark(System.Windows.Media.Imaging.BitmapImage bmp, System.Windows.Media.DrawingContext ctx);

        public Point GetDrawPoint(int objectWidth, int objectHeight, Position position, int maxWidth, int maxHeight)
        {
            Point resultPoint = new Point();

            switch (position)
            {
                case Position.TOPLEFT:
                    {
                        resultPoint = new Point(0, 0);
                        break;
                    }
                case Position.TOPCENTER:
                    {
                        resultPoint = new Point(GetCenter(objectWidth, maxWidth), 0);
                        break;
                    }
                case Position.TOPRIGHT:
                    {
                        resultPoint = new Point(GetRight(objectWidth, maxWidth), 0);
                        break;
                    }
                case Position.LEFTCENTER:
                    {
                        resultPoint = new Point(0, GetCenter(objectHeight, maxHeight));
                        break;
                    }
                case Position.CENTER:
                    {
                        resultPoint = new Point(GetCenter(objectWidth, maxWidth), GetCenter(objectHeight, maxHeight));
                        break;
                    }
                case Position.RIGHTCENTER:
                    {
                        resultPoint = new Point(GetRight(objectWidth, maxWidth), GetCenter(objectHeight, maxHeight));
                        break;
                    }
                case Position.BOTTOMLEFT:
                    {
                        resultPoint = new Point(0, GetRight(objectHeight, maxHeight));
                        break;
                    }
                case Position.BOTTOMCENTER:
                    {
                        resultPoint = new Point(GetCenter(objectWidth, maxWidth), GetRight(objectHeight, maxHeight));
                        break;
                    }
                case Position.BOTTOMRIGHT:
                    {
                        resultPoint = new Point(GetRight(objectWidth, maxWidth), GetRight(objectHeight, maxHeight));
                        break;
                    }
                default:
                    break;
            }

            return resultPoint;
        }

        private double GetCenter(double objectSize, double maxSize)
        {
            return (maxSize / 2.0) - (objectSize / 2.0);
        }

        private double GetRight(double objectSize, double maxSize)
        {
            return maxSize - objectSize;
        }

        abstract public int CompareTo(IWatermark obj);

        public static ICollection<Watermark> Open(string fileName)
        {
            List<Watermark> watermarks = new List<Watermark>();
            try
            {
                using (TextReader reader = new StreamReader(fileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Watermark>), new Type[] { typeof(TextWatermark), typeof(GraphicWatermark) });
                    watermarks = (List<Watermark>)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return watermarks;
        }

        public static void Save(ICollection<Watermark> watermarks, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Watermark>), new Type[] { typeof(TextWatermark), typeof(GraphicWatermark) });
                    serializer.Serialize(writer, new List<Watermark>(watermarks));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
