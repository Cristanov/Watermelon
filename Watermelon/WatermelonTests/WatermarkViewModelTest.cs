using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Watermelon.Models;
using Watermelon.ViewModels;
using Moq;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using Watermelon.Interfaces;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace WatermelonTests
{
    [TestClass]
    public class WatermarkViewModelTest
    {
        Mock<MainWindowViewModel> mainWindowViewModel;
        WatermarkViewModel watermarkViewModel;
        string testFilePath, currentLocation;

        [TestInitialize]
        public void Setup()
        {
            mainWindowViewModel = new Mock<MainWindowViewModel>();
            watermarkViewModel = new WatermarkViewModel(mainWindowViewModel.Object);
            currentLocation = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            testFilePath = Path.Combine(currentLocation, "test.watermark");
            var dialogService = new Mock<IDialogService>();
            dialogService.Setup(d => d.ShowSaveDialog(It.IsAny<string>())).Returns(testFilePath);
            dialogService.Setup(d => d.ShowOpenDialog(It.IsAny<string>(),false)).Returns(testFilePath);
            mainWindowViewModel.Object.DialogService = dialogService.Object;
        }

        [TestMethod]
        public void CanLoadWatermarks()
        {
            ICollection<Watermark> watermarks = new List<Watermark>();
            watermarks.Add(new TextWatermark());
            watermarks.Add(new TextWatermark());
            watermarkViewModel.LoadWatermarks(watermarks);

            Assert.AreEqual(watermarks.Count, watermarkViewModel.Watermarks.Count);
        }

        [TestMethod]
        public void CanSaveWatermark()
        {
            var dialogService = new Mock<IDialogService>();

            dialogService.Setup(s => s.ShowSaveDialog(It.IsAny<string>())).Returns(testFilePath);
            mainWindowViewModel.Object.DialogService = dialogService.Object;
            TextWatermark textWatermark = GetFakeTextWatermark();
            watermarkViewModel.Watermarks.Add(textWatermark);
            watermarkViewModel.SelectedWatermark = watermarkViewModel.Watermarks[0];

            watermarkViewModel.SaveSelectedWatermark();

            Assert.IsTrue(File.Exists(testFilePath));

            XDocument xDocument = XDocument.Load(testFilePath);
            int count = xDocument.Descendants("Watermark").Count();

            Assert.AreEqual(1, count);

            TextWatermark tw = (from w in xDocument.Descendants("Watermark")
                                select new TextWatermark()
                                {
                                    Caption = (string)w.Element("Caption"),
                                    IsVisible = (bool)w.Element("IsVisible"),
                                    FontSize = (int)w.Element("FontSize")
                                }).FirstOrDefault();

            Assert.AreEqual(textWatermark.Caption, tw.Caption);
            Assert.AreEqual(textWatermark.FontSize, tw.FontSize);
            Assert.AreEqual(textWatermark.IsVisible, tw.IsVisible);
        }

        [TestMethod]
        public void CanSaveWatermarkTest()
        {
            Assert.IsFalse(watermarkViewModel.CanSaveWatermark);
            watermarkViewModel.Watermarks.Add(new TextWatermark());
            watermarkViewModel.SelectedWatermark = watermarkViewModel.Watermarks[0];
            Assert.IsTrue(watermarkViewModel.CanSaveWatermark);
        }

        [TestMethod]
        public void CanSaveAllWatermarks()
        {
            

            watermarkViewModel.Watermarks.Add(GetFakeTextWatermark("caption 1"));
            watermarkViewModel.Watermarks.Add(GetFakeTextWatermark("caption 2"));
            watermarkViewModel.Watermarks.Add(GetFakeTextWatermark("caption 3"));

            watermarkViewModel.SaveAllWatermarks();

            Assert.IsTrue(File.Exists(testFilePath));

            XDocument xDocument = XDocument.Load(testFilePath);
            Watermark[] watermarks = (from w in xDocument.Descendants("Watermark")
                                      select new TextWatermark()
                                      {
                                          Caption = (string)w.Element("Caption")
                                      }).ToArray();
            Assert.AreEqual(watermarkViewModel.Watermarks[0].Caption, watermarks[0].Caption);
            Assert.AreEqual(watermarkViewModel.Watermarks[1].Caption, watermarks[1].Caption);
            Assert.AreEqual(watermarkViewModel.Watermarks[2].Caption, watermarks[2].Caption);
        }

        [TestMethod]
        public void CanSaveAllWatermarksTest()
        {
            Assert.IsFalse(watermarkViewModel.CanSaveAllWatermarks);
            watermarkViewModel.Watermarks.Add(GetFakeTextWatermark());
            Assert.IsTrue(watermarkViewModel.CanSaveAllWatermarks);
        }

        [TestMethod]
        public void CanOpenWatermarks()
        {
           string caption = "example graphic watermark";
            watermarkViewModel.Watermarks.Add(GetFakeGraphicWatermark(caption));
            watermarkViewModel.SaveAllWatermarks();
            watermarkViewModel.Watermarks.Clear();

            watermarkViewModel.OpenWatermarks();

            Assert.AreEqual(1, watermarkViewModel.Watermarks.Count);
            Assert.IsTrue(watermarkViewModel.Watermarks[0] is GraphicWatermark);
            Assert.AreEqual(caption, watermarkViewModel.Watermarks[0].Caption);
        }

        [TestMethod]
        public void CanAddNewTextWatermark()
        {
            watermarkViewModel.Watermarks.Clear();
            watermarkViewModel.AddNewTextWatermark();
            Assert.AreEqual(1, watermarkViewModel.Watermarks.Count);
        }

        [TestMethod]
        public void CanAddNewGraphicWatermark()
        {
            watermarkViewModel.Watermarks.Clear();
            var dialogService = new Mock<IDialogService>();
            string exampleImagePath = Path.Combine(currentLocation, "logo.png");
            dialogService.Setup(m => m.ShowOpenDialog(It.IsAny<string>(), false)).Returns(exampleImagePath);
            mainWindowViewModel.Object.DialogService = dialogService.Object;
            watermarkViewModel.AddNewGraphicWatermark();
            Assert.AreEqual(1, watermarkViewModel.Watermarks.Count);
        }

        [TestMethod]
        public void CanOnAndOffWatermark()
        {
            Watermark watermark = GetFakeGraphicWatermark();
            watermarkViewModel.Watermarks.Add(watermark);
            Assert.IsTrue(watermarkViewModel.Watermarks[0].IsVisible);
            watermarkViewModel.OnOffWatermark(watermark);
            Assert.IsFalse(watermarkViewModel.Watermarks[0].IsVisible);
            watermarkViewModel.OnOffWatermark(watermark);
            Assert.IsTrue(watermarkViewModel.Watermarks[0].IsVisible);

        }

        private GraphicWatermark GetFakeGraphicWatermark(string caption = "example caption")
        {
            return new GraphicWatermark()
            {
                Caption = caption,
                Height = 100,
                Width = 55,
                IsVisible = true,
                Opacity = 0.5
            };
        }

        private TextWatermark GetFakeTextWatermark(string caption = "example caption")
        {
            return new TextWatermark()
            {
                Caption = caption,
                IsVisible = true,
                WatermarkPosition = Position.CENTER,
                Color = Colors.Aqua,
                FontSize = 20
            };
        }
    }
}
