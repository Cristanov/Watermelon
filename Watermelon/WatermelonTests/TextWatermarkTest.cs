using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Watermelon.Models;

namespace WatermelonTests
{
    [TestClass]
    public class TextWatermarkTest
    {
        TextWatermark textWatermark1, textWatermark2, textWatermark3, textWatermark4;
        [TestInitialize]
        public void Setup()
        {
            textWatermark1 = new TextWatermark()
            {
                Caption = "caption",
                Color = System.Windows.Media.Colors.Black,
                FontName = "Times New Roman",
                FontSize = 20,
                IsVisible = true,
                Opacity = 0.2,
                WatermarkPosition = Watermelon.Models.Position.BOTTOMCENTER
            };
            textWatermark2 = new TextWatermark()
            {
                Caption = "caption",
                Color = System.Windows.Media.Colors.Black,
                FontName = "Times New Roman",
                FontSize = 20,
                IsVisible = true,
                Opacity = 0.2,
                WatermarkPosition = Watermelon.Models.Position.BOTTOMCENTER
            };
            textWatermark3 = new TextWatermark()
            {
                Caption = "caption",
                Color = System.Windows.Media.Colors.Black,
                FontName = "Times New Roman",
                FontSize = 20,
                IsVisible = true,
                Opacity = 0.3, // różnica
                WatermarkPosition = Watermelon.Models.Position.BOTTOMCENTER
            };
            textWatermark4 = new TextWatermark()
            {
                Caption = "caption",
                Color = System.Windows.Media.Colors.Black,
                FontName = "Times New Roman",
                FontSize = 20,
                IsVisible = true,
                Opacity = 0.2, // różnica
                WatermarkPosition = Watermelon.Models.Position.BOTTOMRIGHT
            };
        }
        [TestMethod]
        public void CompareToTest()
        {
            Assert.AreEqual(0, textWatermark1.CompareTo(textWatermark2));
            Assert.AreNotEqual(0, textWatermark1.CompareTo(textWatermark3));
            Assert.AreNotEqual(0, textWatermark1.CompareTo(textWatermark4));
            Assert.AreNotEqual(0, textWatermark1.CompareTo(new GraphicWatermark()));
        }
    }
}
