using System.Windows.Controls;
using System.Windows.Media.Imaging;
namespace Watermelon.Models
{
    public class WatermarkDTO
    {
        private IWatermark watermark;
        public WatermarkDTO(IWatermark watermark)
        {
            Watermark = watermark;
        }

        public IWatermark Watermark
        {
            get
            {
                return watermark;
            }
            set
            {
                watermark = value;
            }
        }
    }
}
