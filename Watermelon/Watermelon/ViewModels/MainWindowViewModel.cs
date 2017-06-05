using Actualization;
using Caliburn.Micro;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Watermelon.Views;
using Watermelon.Properties;
using Watermelon.Models;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Watermelon.Services;
using Watermelon.Interfaces;

namespace Watermelon.ViewModels
{
    public class MainWindowViewModel : Conductor<object>
    {
        private bool isActual;
        private const int PROJECTPATH = 1;
        private string newVersionPath = @"http://www.mwlodarz.cba.pl/Watermelon/version.txt";
        private string currentVersionPath = Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "version.txt");
        private WindowManager windowManager;
        private IDialogService dialogService;
        public MainWindowViewModel()
        {
            ImagesViewModel = new ImagesViewModel(this);
            WatermarkViewModel = new WatermarkViewModel(this);
            SaveViewModel = new SaveViewModel(this);
            Title = "New Project";
            windowManager = new WindowManager();
            IsProjectOpen = false;
            dialogService = new DialogService();
        }

        public ImagesViewModel ImagesViewModel { get; set; }
        public WatermarkViewModel WatermarkViewModel { get; set; }
        public SaveViewModel SaveViewModel { get; set; }

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value + ": Watermelon " + AppVersion;
                DisplayName = title;
            }
        }

        public IDialogService DialogService
        {
            get
            {
                return dialogService;
            }
            set
            {
                dialogService = value;
            }
        }

        private void LoadStartProject()
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                string projectPath = Environment.GetCommandLineArgs()[PROJECTPATH];
                if (File.Exists(projectPath))
                {
                    LoadWatermelonProject(projectPath);
                    Title = Path.GetFileNameWithoutExtension(projectPath) + " " + Title;
                    IsProjectOpen = true;
                    CurrentProjectPath = projectPath;
                }
            }
        }

        public string AppVersion
        {
            get
            {
                return Actualizer.GetCurrentVersion(currentVersionPath);
            }
        }

        private string _updateMessage;
        public string UpdateMessage
        {
            get
            {
                return _updateMessage;
            }
            set
            {
                _updateMessage = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsActual
        {
            get
            {
                return isActual;
            }
            set
            {
                isActual = value;
                NotifyOfPropertyChange("IsActual");
            }
        }
        public bool PolishLanguage
        {
            get
            {
                if (Thread.CurrentThread.CurrentUICulture.ToString() == "pl-PL")
                {
                    return true;
                }
                else return false;
            }
        }

        public bool EnglishLanguage
        {
            get
            {
                if (Thread.CurrentThread.CurrentUICulture.ToString() == "en-US")
                    return true;
                else
                    return false;
            }
        }

       
        public string CurrentProjectPath { get; set; }

        public bool IsProjectOpen { get; set; }

        private string statusLabel;
        public string StatusLabel
        {
            get
            {
                return statusLabel;
            }
            set
            {
                statusLabel = value;
                NotifyOfPropertyChange();
            }
        }

        public void spImagesMouseDown()
        {
            ActivateItem(ImagesViewModel);
        }

        public void LoadedMainWindow()
        {
            CreateAppDirectory();
            LoadStartProject();
            try
            {
                IsActual = Actualizer.IsActual(Actualizer.GetCurrentVersion(currentVersionPath), Actualizer.GetNewVersion(newVersionPath));
                UpdateMessage = Resources.availableNewVersion + " " + Actualizer.GetNewVersion(newVersionPath) + ". " + Resources.clickToUpdate;
            }
            catch (Exception ex)
            {
                IsActual = true;
                StatusLabel = ex.ToString();
            }
            spImagesMouseDown();
        }

        public void AboutClick()
        {
            windowManager.ShowDialog(new AboutViewModel());
        }

        public void CreateAppDirectory()
        {
            string appDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Watermelon");
            if (!Directory.Exists(appDirPath))
            {
                Directory.CreateDirectory(appDirPath);
            }
        }

        public void PolishLanguageChecked()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pl-PL");
            Settings.Default.Language = Thread.CurrentThread.CurrentUICulture.ToString();
            Settings.Default.Save();
            dialogService.ShowMessage(Resources.langCheckedMessage);
        }

        public void EnglishLanguageChecked()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            Settings.Default.Language = Thread.CurrentThread.CurrentUICulture.ToString();
            Settings.Default.Save();
            dialogService.ShowMessage(Resources.langCheckedMessage);
        }

        public void spWatermarkMouseDown()
        {
            ActivateItem(WatermarkViewModel);
            if (ImagesViewModel.Images.Count > 0)
            {
                WatermarkViewModel.ImageUrl = ImagesViewModel.Images[0].Source.ToString();
                WatermarkViewModel.RefreshPreview();
            }
        }

        public void spSettingsMouseDown()
        {
            ActivateItem(SaveViewModel);
        }

        public void UpdateButton()
        {
            if (!IsActual)
            {
                Actualizer.RunActualizer(Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "Actualizer.exe"));
            }
        }

        public void Closing()
        {
            SaveOrSaveAs();
        }

        private WatermelonProject GetCurrentWatermelonProject()
        {
            WatermelonProject watermelonProject;
            List<Watermark> watermarks = new List<Watermark>();
            foreach (var item in WatermarkViewModel.Watermarks)
            {
                watermarks.Add(item);
            }
            return watermelonProject = new WatermelonProject(ImagesViewModel.ImagePaths, watermarks.ToArray(), SaveViewModel.Suffix);
        }

        public void New()
        {
            SaveOrSaveAs();
            LoadEmptyProject();
        }

        private void SaveOrSaveAs()
        {
            if (IsProjectOpen && GetCurrentWatermelonProject().CompareTo(WatermelonProject.Open(CurrentProjectPath)) != 0)
            {
                bool save = dialogService.ShowMessageYesNo(Resources.DoYouWantSaveChanges +" " + Path.GetFileNameWithoutExtension(CurrentProjectPath) + "?",
                    Resources.saveLab);
                if (save)
                {
                    Save();
                }
            }
            if (!IsProjectOpen && GetCurrentWatermelonProject().CompareTo(WatermelonProject.GetEmptyProject()) != 0)
            {
                bool save = dialogService.ShowMessageYesNo(Resources.doYouWantSaveNewProject, Resources.saveLab);
                if (save)
                {
                    SaveAs();
                }
            }
        }

        public void Open()
        {
            SaveOrSaveAs();
            string fileName = dialogService.ShowOpenDialog("Watermelon Files(.watermelon)|*.watermelon", false);
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                LoadWatermelonProject(fileName);
                Title = Path.GetFileNameWithoutExtension(fileName) + " " + Title;
                IsProjectOpen = true;
                CurrentProjectPath = fileName;
            }
        }

        public void Save()
        {
            if (IsProjectOpen)
            {
                WatermelonProject watermelonProject = GetCurrentWatermelonProject();
                watermelonProject.Save(CurrentProjectPath);
                StatusLabel = Resources.saved + " " + Path.GetFileNameWithoutExtension(CurrentProjectPath);
            }
            else
            {
                SaveAs();
            }
        }

        public void SaveAs()
        {
            string fileName = dialogService.ShowSaveDialog("Watermelon Files(.watermelon)|*.watermelon");
            if (fileName != String.Empty)
            {
                WatermelonProject watermelonProject = GetCurrentWatermelonProject();
                watermelonProject.Save(fileName);
                Title = Path.GetFileNameWithoutExtension(fileName);
                IsProjectOpen = true;
                CurrentProjectPath = fileName;
            }
        }

        /// <summary>
        /// Function loads an object of type WatermelonProject to view models
        /// </summary>
        /// <param name="watermelonProject">Object of type WatermelonProject</param>
        public void LoadWatermelonProject(WatermelonProject watermelonProject)
        {
            Title = watermelonProject.Name;
            SaveViewModel.Suffix = watermelonProject.Suffix;
            ImagesViewModel.Images.Clear();
            ImagesViewModel.LoadImages(watermelonProject.Images);
            WatermarkViewModel.Watermarks.Clear();
            WatermarkViewModel.LoadWatermarks(watermelonProject.Watermarks);
        }
        /// <summary>
        /// Function loads an object of type WatermelonProject to view models.
        /// </summary>
        /// <param name="filePath">Path to watermelon file (*.watermelon)</param>
        private void LoadWatermelonProject(string filePath)
        {
            WatermelonProject watermelonProject = WatermelonProject.Open(filePath);
            LoadWatermelonProject(watermelonProject);
        }

        private void LoadEmptyProject()
        {
            LoadWatermelonProject(WatermelonProject.GetEmptyProject());
            IsProjectOpen = false;
            CurrentProjectPath = string.Empty;
        }
    }
}