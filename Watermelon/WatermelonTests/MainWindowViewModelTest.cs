using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Watermelon.Models;
using Watermelon.ViewModels;
using System.Windows.Controls;
using Watermelon.Interfaces;

namespace WatermelonTests
{
    [TestClass]
    public class MainWindowViewModelTest
    {
        [TestMethod]
        public void CanLoadWatermelonProjectToUI()
        {
            Watermark[] watermarks = { new TextWatermark(), new GraphicWatermark() };
            WatermelonProject wp = new WatermelonProject(new string[] { }, watermarks, "suf", "newPro");
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

            mainWindowViewModel.LoadWatermelonProject(wp);
            Assert.AreEqual(wp.Images.Length, mainWindowViewModel.ImagesViewModel.Images.Count);
            Assert.AreEqual(wp.Watermarks.Length, mainWindowViewModel.WatermarkViewModel.Watermarks.Count);
            Assert.AreEqual(wp.Suffix, mainWindowViewModel.SaveViewModel.Suffix);
        }
    }
}
