using Actualization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace WatermelonTests
{
    [TestClass]
    public class ActualizerTest
    {
        string currentVersionFilePath;
        [TestInitialize]
        public void Setup()
        {
            currentVersionFilePath = "currentVersion.txt";
            using (StreamWriter writer = new StreamWriter(currentVersionFilePath))
            {
                writer.Write("1.0");
            }
        }
        [TestMethod]
        public void GetCurrentVersionTest()
        {
            string currentVersion = Actualizer.GetCurrentVersion(currentVersionFilePath);
            Assert.AreEqual("1.0", currentVersion);
        }

        [TestMethod]
        public void IsActualTest()
        {
            Assert.IsTrue(Actualizer.IsActual("1.0", "1.0"));
            Assert.IsFalse(Actualizer.IsActual("1.0", "1.5"));
        }

        [TestCleanup]
        public void Clean()
        {
            File.Delete(currentVersionFilePath);
        }

    }
}
