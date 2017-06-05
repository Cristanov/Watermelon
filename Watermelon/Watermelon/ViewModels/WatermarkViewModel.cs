
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Watermelon.Controls;
using Watermelon.Models;
namespace Watermelon.ViewModels
{
    public class WatermarkViewModel : PropertyChangedBase
    {
        private MainWindowViewModel mainWindowViewModel;
        private System.Windows.Controls.Image exampleImage;
        private string imageUrl;
        private int currentExampleIdx;
        private string tempImagePath;
        
        public WatermarkViewModel(MainWindowViewModel mainWindowVM)
        {
            mainWindowViewModel = mainWindowVM;
            Watermarks = new BindableCollection<Watermark>();
            ExampleImage = new Image();
            currentExampleIdx = 0;
        }

        private BindableCollection<Watermark> _watermarks;
        public BindableCollection<Watermark> Watermarks
        {
            get
            {
                return _watermarks;
            }
            set
            {
                _watermarks = value;
                NotifyOfPropertyChange();
            }
        }

        private Watermark _selectedWatermark;
        public Watermark SelectedWatermark
        {
            get
            {
                return _selectedWatermark;
            }
            set
            {
                _selectedWatermark = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanSaveAllWatermarks);
                NotifyOfPropertyChange(() => CanSaveWatermark);
            }
        }

        public string ImageUrl
        {
            get
            {
                return imageUrl;
            }
            set
            {
                imageUrl = value;
                NotifyOfPropertyChange("ImageUrl");
            }
        }

        public System.Windows.Controls.Image ExampleImage
        {
            get
            {
                return exampleImage;
            }
            set
            {
                exampleImage = value;
                NotifyOfPropertyChange("ExampleImage");
            }
        }

        public int ImagesCount
        {
            get
            {
                return mainWindowViewModel.ImagesViewModel.Images.Count;
            }
        }

        public void NextExampleButton()
        {
            currentExampleIdx = mainWindowViewModel.ImagesViewModel.NextIdx(currentExampleIdx);
            ImageUrl = mainWindowViewModel.ImagesViewModel.Images[currentExampleIdx].Source.ToString();
            RefreshPreview();
        }

        public void PreviousExampleButton()
        {
            currentExampleIdx = mainWindowViewModel.ImagesViewModel.PreviousIdx(currentExampleIdx);
            ImageUrl = mainWindowViewModel.ImagesViewModel.Images[currentExampleIdx].Source.ToString();
            RefreshPreview();
        }

        /// <summary>
        /// Function shows save file dialog and saves selected watermark to file
        /// </summary>
        public void SaveSelectedWatermark()
        {
            string fileName = mainWindowViewModel.DialogService.ShowSaveDialog("Watermark Files(.watermark)|*.watermark;");
            if (fileName != String.Empty)
            {
                Watermark.Save(new Watermark[] { SelectedWatermark }, fileName);
            }
        }

        /// <summary>
        /// Specifies whether the SaveSelectedWatermark method can be execute
        /// </summary>
        public bool CanSaveWatermark
        {
            get
            {
                return SelectedWatermark != null ? true : false;
            }
        }

        /// <summary>
        /// Function shows save file dialog and saves all watermarks to file
        /// </summary>
        public void SaveAllWatermarks()
        {
            string fileName = mainWindowViewModel.DialogService.ShowSaveDialog("Watermark Files(.watermark)|*.watermark;");
            if (fileName != String.Empty)
            {
                Watermark.Save(Watermarks, fileName);
            }
        }

        /// <summary>
        /// Specifies whether the SaveAllWatermarks method can be executed
        /// </summary>
        public bool CanSaveAllWatermarks
        {
            get
            {
                return (Watermarks.Count > 0) ? true : false;
            }
        }

        /// <summary>
        /// Function shows open file dialog and loads selected watermark file into project
        /// </summary>
        public void OpenWatermarks()
        {
            string fileName = mainWindowViewModel.DialogService.ShowOpenDialog("Watermark Files(.watermark)|*.watermark;", false);
            if (fileName != string.Empty)
            {
                ICollection<Watermark> watermarks = Watermark.Open(fileName);
                if (watermarks.Count != 0)
                {
                    Watermarks.AddRange(watermarks);
                    RefreshPreview();
                    NotifyOfPropertyChange(() => CanSaveAllWatermarks);
                }
            }
        }

        public void RefreshPreview()
        {
            if (mainWindowViewModel.ImagesViewModel.Images.Count > 0)
            {
                if (currentExampleIdx > mainWindowViewModel.ImagesViewModel.Images.Count - 1)
                {
                    currentExampleIdx = mainWindowViewModel.ImagesViewModel.Images.Count - 1;
                }
                tempImagePath = mainWindowViewModel.ImagesViewModel.Images[currentExampleIdx].Source.ToString();
                AddWatermarks();
            }
        }

        private void AddWatermarks()
        {
            BitmapImage bmp = new BitmapImage(new System.Uri(tempImagePath, UriKind.Absolute));

            int width = (int)bmp.PixelWidth;
            int height = (int)bmp.PixelHeight;
            var visual = new DrawingVisual();

            using (DrawingContext drawingCtx = visual.RenderOpen())
            {
                drawingCtx.DrawImage(bmp, new Rect(0, 0, width, height));

                foreach (Watermark item in Watermarks)
                {
                    item.AddWatermark(bmp, drawingCtx);
                }
            }

            RenderTargetBitmap target = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            target.Render(visual);
            ExampleImage.Source = target;
        }

        public void ChangePosition(int buttonNumber)
        {
            if (SelectedWatermark != null)
            {
                SelectedWatermark.WatermarkPosition = (Position)buttonNumber;
                RefreshPreview();
            }
        }

        /// <summary>
        /// Function adds new text watermark to Watermark collection
        /// </summary>
        public void AddNewTextWatermark()
        {
            Watermark watermark = new TextWatermark();
            Watermarks.Add(watermark);
            SelectedWatermark = watermark;
        }

        /// <summary>
        /// Function adds new graphic watermark to Watermark collection
        /// </summary>
        public void AddNewGraphicWatermark()
        {
           string filename = mainWindowViewModel.DialogService.ShowOpenDialog("Image files(.jpg, .jpeg, .bmp, .png)|*.jpg;*.jpeg;*.bmp;*.png",false);
            if (!string.IsNullOrWhiteSpace(filename))
            {
                BitmapImage bmpMark = new BitmapImage(new Uri(filename));
                GraphicWatermark watermark = new GraphicWatermark()
                {
                    Caption = Path.GetFileName(filename),
                    ImagePath = filename,
                    Height = bmpMark.PixelHeight,
                    Width = bmpMark.PixelWidth
                };
                Watermarks.Add(watermark);
                SelectedWatermark = watermark;
            }
        }

        /// <summary>
        /// Function removes watermark from collection
        /// </summary>
        /// <param name="watermark">Watermark to remove.</param>
        public void DeleteWatermark(Watermark watermark)
        {
            Watermarks.Remove(watermark);
            NotifyOfPropertyChange(() => CanSaveAllWatermarks);
            RefreshPreview();
        }

        /// <summary>
        /// Function enables or disables the visibility of the watermark.
        /// </summary>
        /// <param name="watermark">Object type Watermark</param>
        public void OnOffWatermark(Watermark watermark)
        {
            int idx = Watermarks.IndexOf(watermark);
            Watermark tempWatermark = watermark;
            Watermarks.Remove(watermark);
            watermark.IsVisible = !watermark.IsVisible;
            Watermarks.Insert(idx, tempWatermark);
            SelectedWatermark = tempWatermark;
            RefreshPreview();
        }
        
        /// <summary>
        /// Function loads watermarks to collection in view model.
        /// </summary>
        /// <param name="watermarks">Collection of watermarks</param>
        public void LoadWatermarks(ICollection<Watermark> watermarks)
        {
            foreach (var item in watermarks)
            {
                Watermarks.Add(item);
            }
        }
    }
}
