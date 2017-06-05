using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Watermelon.Models;

namespace WatermelonTests
{
    [TestClass]
    public class GraphicWatermarkTests
    {
        GraphicWatermark gw1, gw2, gw3;
        [TestInitialize]
        public void Setup()
        {
            gw1 = new GraphicWatermark()
            {
                Caption = "caption",
                Height = 100,
                ImagePath = "imagePath",
                IsVisible = true,
                Opacity = 0.4,
                WatermarkPosition = Watermelon.Models.Position.CENTER,
                Width = 100
            };
            gw2 = new GraphicWatermark()
            {
                Caption = "caption",
                Height = 100,
                ImagePath = "imagePath",
                IsVisible = true,
                Opacity = 0.4,
                WatermarkPosition = Watermelon.Models.Position.CENTER,
                Width = 100
            };
            gw3 = new GraphicWatermark()
            {
                Caption = "caption",
                Height = 100,
                ImagePath = "imagePath",
                IsVisible = false,
                Opacity = 0.4,
                WatermarkPosition = Watermelon.Models.Position.CENTER,
                Width = 100
            };
        }
        [TestMethod]
        public void CompareToTest()
        {
            Assert.AreEqual(0, gw1.CompareTo(gw2));
            Assert.AreNotEqual(0, gw1.CompareTo(gw3));
            Assert.AreNotEqual(0, gw1.CompareTo(new TextWatermark()));
        }
    }
}
