using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Watermelon.Models
{
    public class WatermelonProject : IComparable<WatermelonProject>
    {
        public WatermelonProject()
        {
        }

        public WatermelonProject(string projectPath)
        {
            WatermelonProject watermelonProject;
            using (TextReader reader = new StreamReader(projectPath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(WatermelonProject), new Type[] { typeof(TextWatermark), typeof(GraphicWatermark) });
                watermelonProject = deserializer.Deserialize(reader) as WatermelonProject;
            }
            Name = System.IO.Path.GetFileNameWithoutExtension(projectPath);
            Suffix = watermelonProject.Suffix;
            Path = projectPath;
            Images = watermelonProject.Images;
            Watermarks = watermelonProject.Watermarks;
        }

        public WatermelonProject(string[] images, Watermark[] watermarks, string suffix)
        {
            Images = images;
            Watermarks = watermarks;
            Suffix = suffix;
        }
        public WatermelonProject(string[] images, Watermark[] watermarks, string suffix, string name)
        {
            Images = images;
            Watermarks = watermarks;
            Suffix = suffix;
            Name = name;
        }
        public WatermelonProject(string[] images, ICollection<Watermark> watermarks, string suffix)
        {
            Images = images;
            Watermarks = watermarks.ToArray();
            Suffix = suffix;
        }
        public WatermelonProject(string[] images, ICollection<Watermark> watermarks, string suffix, string name)
        {
            Name = name;
            Images = images;
            Watermarks = watermarks.ToArray();
            Suffix = suffix;
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public string[] Images { get; set; }
        public Watermark[] Watermarks { get; set; }
        public string Suffix { get; set; }

        public void Save(string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(WatermelonProject), new Type[] { typeof(TextWatermark), typeof(GraphicWatermark) });
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public static WatermelonProject Open(string filePath)
        {
            WatermelonProject watermelonProject;
            using (TextReader reader = new StreamReader(filePath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(WatermelonProject), new Type[] { typeof(TextWatermark), typeof(GraphicWatermark) });
                watermelonProject = deserializer.Deserialize(reader) as WatermelonProject;
            }
            return watermelonProject;
        }

        public void Save()
        {
            Save(Path);
        }

        public int CompareTo(WatermelonProject other)
        {
            if (IsImagesEqual(this.Images, other.Images) && WatermarksEqual(this.Watermarks, other.Watermarks) && Suffix == other.Suffix)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// Function returns empty watermelon project.
        /// </summary>
        /// <returns>Object of WatermelonProject type</returns>
        public static WatermelonProject GetEmptyProject()
        {
            return new WatermelonProject(new string[] { }, new Watermark[] { }, "_watermelon", "New Project");
        }

        private bool WatermarksEqual(ICollection<Watermark> watermark1, ICollection<Watermark> watermark2)
        {
            if (watermark1.Count != watermark2.Count)
            {
                return false;
            }
            for (int i = 0; i < watermark1.Count; i++)
            {
                if (watermark1.ElementAt(i).CompareTo(watermark2.ElementAt(i)) != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsImagesEqual(string[] images1, string[] images2)
        {
            if (images1.Length != images2.Length)
                return false;

            for (int i = 0; i < images1.Length; i++)
            {
                if (images1[i] != images2[i])
                {
                    return false;
                }
            }
            return true;
        }

    }

}
