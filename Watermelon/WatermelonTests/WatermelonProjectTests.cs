using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Watermelon.Models;
using Moq;

namespace WatermelonTests
{
    [TestClass]
    public class WatermelonProjectTests
    {
        string[] images;
        TextWatermark tw;
        GraphicWatermark gw, gw2;
        Watermark[] watermarks;
        WatermelonProject WP1,WP2,WP3,WP4,WP5,WP6;
        string suffix;
        [TestInitialize]
        public void Setup()
        {
            images = new string[] { "a", "b", "c" };
            tw = new TextWatermark() { Caption = "cap", IsVisible = true, WatermarkPosition = Watermelon.Models.Position.CENTER, Opacity = 0.2 };
            gw = new GraphicWatermark() { Caption = "gcap", Height = 10, Width = 10, Opacity = 0.3, IsVisible = true, ImagePath = "imgPath" };
            gw2 = new GraphicWatermark() { Caption = "gcap", Height = 10, Width = 10, Opacity = 0.3, IsVisible = false, ImagePath = "imgPath" };
            watermarks = new Watermark[] { tw, gw };
            suffix = "suf";

            WP1 = new WatermelonProject(images, watermarks,suffix);
            WP2 = new WatermelonProject(images, watermarks,suffix);
            WP3 = new WatermelonProject(new string[] { "a", "b", "d" }, watermarks,suffix);
            WP4 = new WatermelonProject(images, new Watermark[]{tw, gw2},suffix);
            WP5 = new WatermelonProject(images, new Watermark[] { gw, gw2 },suffix);
            WP6 = new WatermelonProject(images, watermarks, "sss");
        }

        [TestMethod]
        public void CompareToTest()
        {
            Assert.AreEqual(0, WP1.CompareTo(WP2));
            Assert.AreNotEqual(0, WP1.CompareTo(WP3));
            Assert.AreNotEqual(0, WP1.CompareTo(WP4));
            Assert.AreNotEqual(0, WP1.CompareTo(WP5));
            Assert.AreNotEqual(0, WP1.CompareTo(WP6));
        }

        [TestMethod]
        public void CanGetDefaultWatermelonProject()
        {
            WatermelonProject wp = WatermelonProject.GetEmptyProject();
            Assert.IsNotNull(wp);
            Assert.IsNotNull(wp.Name);
            Assert.IsNotNull(wp.Images);
            Assert.IsNotNull(wp.Watermarks);
            Assert.IsNotNull(wp.Suffix);
        }
    }
}
