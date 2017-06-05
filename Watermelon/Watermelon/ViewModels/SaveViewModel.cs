using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Watermelon.Models;
using Watermelon.Properties;

namespace Watermelon.ViewModels
{
    public class SaveViewModel : PropertyChangedBase
    {
        private string saveDirectory;
        private MainWindowViewModel mainWindowViewModel;
        private string message;
        private bool savedSuccessful;
        private string saveMessage;
        public SaveViewModel(MainWindowViewModel mainWindowViewModel)
        {
            SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Message = String.Empty;
            SaveMessage = String.Empty;
            SavedSuccessful = false;
            this.mainWindowViewModel = mainWindowViewModel;
            Suffix = "_watermelon";
        }

        public string Suffix { get; set; }

        public string SaveDirectory
        {
            get
            {
                return saveDirectory;
            }
            set
            {
                saveDirectory = value;
                NotifyOfPropertyChange("SaveDirectory");
            }
        }
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                NotifyOfPropertyChange("Message");
            }
        }

        public bool CanSaveEnable
        {
            get
            {
                return CanSave();
            }
        }

        public bool SavedSuccessful
        {
            get
            {
                return savedSuccessful;
            }
            set
            {
                savedSuccessful = value;
                NotifyOfPropertyChange("SavedSuccessful");
            }
        }

        public string SaveMessage
        {
            get
            {
                return saveMessage;
            }
            set
            {
                saveMessage = value;
                NotifyOfPropertyChange("SaveMessage");
            }   
        }

        public void Loaded()
        {
            Message = String.Empty;
            if (mainWindowViewModel.ImagesViewModel.Images.Count == 0)
            {
                Message += Resources.noImagesMessage;
            }
            if (mainWindowViewModel.WatermarkViewModel.Watermarks.Count == 0)
            {
                Message += " " + Resources.noWatermarksMessage;
            }
        }

        public void BrowseButton()
        {
            string folderPath = mainWindowViewModel.DialogService.ShowFolderDialog();
            if (folderPath!= string.Empty)
            {
                SaveDirectory = folderPath;
            }
        }

        public void Save()
        {
            Image[] images = mainWindowViewModel.ImagesViewModel.Images.ToArray();
            int savedImagesCounter = 0;
            foreach (Image item in images)
            {
                BitmapImage bmp = new BitmapImage(new System.Uri(item.Source.ToString(), UriKind.Absolute));
                RenderTargetBitmap target = AddWaterMarks(bmp);
                string imgSavePath = Path.Combine(SaveDirectory, Path.GetFileNameWithoutExtension(
                    (item.Source.ToString())) + Suffix + Path.GetExtension(item.Source.ToString()));
                SaveImage(target, imgSavePath);
                savedImagesCounter++;
            }
            SaveMessage = String.Format("{0} {1}",Resources.successfulSavedMessage, savedImagesCounter);
            SavedSuccessful = true;
        }

        public bool CanSave()
        {
            if (mainWindowViewModel.ImagesViewModel.Images.Count > 0 && mainWindowViewModel.WatermarkViewModel.Watermarks.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CloseSaveMessage()
        {
            SavedSuccessful = false;
        }

        private RenderTargetBitmap AddWaterMarks(BitmapImage bmp)
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingCtx = visual.RenderOpen())
            {
                drawingCtx.DrawImage(bmp, new Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight));

                foreach (Watermark item in mainWindowViewModel.WatermarkViewModel.Watermarks)
                {
                    item.AddWatermark(bmp, drawingCtx);
                }
            }

            RenderTargetBitmap target = new RenderTargetBitmap(bmp.PixelWidth, bmp.PixelHeight, 96, 96, PixelFormats.Default);
            target.Render(visual);
            return target;
        }
        private static void SaveImage(RenderTargetBitmap target, string imgSavePath)
        {
            BitmapEncoder encoder = GetEncoder(Path.GetExtension(imgSavePath));
            encoder.Frames.Add(BitmapFrame.Create(target));
            using (FileStream fs = new FileStream(imgSavePath, FileMode.OpenOrCreate))
            {
                encoder.Save(fs);
            }
        }

        private static BitmapEncoder GetEncoder(string extention)
        {
            switch (extention)
            {
                case ".jpg":
                    {
                        return new JpegBitmapEncoder();
                    }
                case ".png":
                    {
                        return new PngBitmapEncoder();
                    }
                case ".bmp":
                    {
                        return new BmpBitmapEncoder();
                    }
                case ".gif":
                    {
                        return new GifBitmapEncoder();
                    }
                default:
                    return new JpegBitmapEncoder();
            }
        }
    }
}
