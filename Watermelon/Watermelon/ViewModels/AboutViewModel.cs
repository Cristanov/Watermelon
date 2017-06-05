using Actualization;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Watermelon.ViewModels
{
    class AboutViewModel : PropertyChangedBase
    {
        public AboutViewModel()
        {
            Url = "www.mwlodarz.cba.pl";
        }

        public string Url { get; set; }

        public void ClickLink()
        {
            Process.Start(new ProcessStartInfo(Url));
        }

        public string AppVersion
        {
            get
            {
                return Actualizer.GetCurrentVersion(Path.Combine(Path.GetDirectoryName( Environment.GetCommandLineArgs()[0]), "version.txt"));
            }
        }
    }
}
