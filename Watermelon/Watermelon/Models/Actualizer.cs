using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using Watermelon.Services;

namespace Actualization
{
    public class Actualizer
    {
       
        public static void RunActualizer(string actualizerPath)
        {
            string currentProcessId = Process.GetCurrentProcess().Id.ToString();
            string currentDirPath = Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]));
            string currentExecPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string server = "www.mwlodarz.cba.pl";
            string username = "watermelon@mwlodarz.cba.pl";
            string password = "watermelon";
            string path = "Watermelon";

            Process actualizer = new Process();
            actualizer.StartInfo.FileName = actualizerPath;
            actualizer.StartInfo.Arguments = String.Format("{0} {1} {2} {3} {4} {5} {6}", currentProcessId, currentDirPath.Replace(' ', '*'),
                currentExecPath.Replace(' ', '*'), server, username, password, path);
            try
            {
                actualizer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static bool IsActual(string currentVersion, string newVersion)
        {
            string[] currentVersionTab = currentVersion.Split('.');
            string[] newVersionTab = newVersion.Split('.');
            bool isActual = true;
            for (int i = 0; i < currentVersionTab.Length; i++)
            {
                if (int.Parse(newVersionTab[i]) > int.Parse(currentVersionTab[i]))
                {
                    isActual = false;
                }
            }
            return isActual;
        }

        public static string GetCurrentVersion(string versionFilePath)
        {
            string version = "";
            try
            {
                using (StreamReader sr = new StreamReader(versionFilePath))
                {
                    version = sr.ReadLine();
                }
            }
            catch (Exception)
            {
            }
            return version;
        }

        public static string GetNewVersion(string newVersionFilePath)
        {
            WebClient webClient = new WebClient();
            return webClient.DownloadString(newVersionFilePath);
        }
    }
}
