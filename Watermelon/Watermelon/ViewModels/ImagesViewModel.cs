
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
namespace Watermelon.ViewModels
{
    public class ImagesViewModel : PropertyChangedBase
    {

        public ImagesViewModel(MainWindowViewModel mainWindowViewModel)
        {
            Images = new BindableCollection<Image>();
            this.mainWindowViewModel = mainWindowViewModel;
            SelectedImage = -1;
        }

        MainWindowViewModel mainWindowViewModel;
        public BindableCollection<Image> Images { get; set; }

        private int selectedImage;
        public int SelectedImage
        {
            get
            {
                return selectedImage;
            }
            set
            {
                selectedImage = value;
                NotifyOfPropertyChange("SelectedImage");
            }
        }

        public void loadImagesMouseDown()
        {
            string filter = "Image files(.jpg, .jpeg, .bmp, .png)|*.jpg;*.jpeg;*.bmp;*.png";
            string[] fileNames = mainWindowViewModel.DialogService.ShowOpenDialog(filter);
            if (fileNames.Length != 0)
            {
                LoadImages(fileNames);
            }
        }

        public void LoadImages(string[] imagePaths)
        {
            foreach (string item in imagePaths)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(item, UriKind.Absolute));
                Images.Add(img);
            }
        }

        public int PreviousIdx(int currentIdx)
        {
            if (currentIdx - 1 < 0)
            {
                return Images.Count - 1;
            }
            else
            {
                return --currentIdx;
            }
        }

        public int NextIdx(int currentIdx)
        {
            if (currentIdx + 1 >= Images.Count)
            {
                return 0;
            }
            else
            {
                return ++currentIdx;
            }
        }

        public void RightButton()
        {
            mainWindowViewModel.ActivateItem(mainWindowViewModel.WatermarkViewModel);
            if (Images.Count > 0)
            {
                mainWindowViewModel.WatermarkViewModel.ImageUrl = Images[0].Source.ToString();
            }
        }

        public void PreviewButton()
        {
            string path = Images[SelectedImage].Source.ToString();
            Process.Start(path);
        }

        public bool CanPreviewButton
        {
            get
            {
                return SelectedImage != -1 ? true : false;
            }
        }

        public void DeleteButton()
        {
            Images.RemoveAt(SelectedImage);
            SelectedImage = -1;
        }

        public void ClearButton()
        {
            Images.Clear();
        }

        public string[] ImagePaths
        {
            get
            {
                List<string> imagesPaths = new List<string>();
                foreach (var item in Images)
                {
                    imagesPaths.Add(item.Source.ToString());
                }
                return imagesPaths.ToArray();
            }
        }
    }
}
