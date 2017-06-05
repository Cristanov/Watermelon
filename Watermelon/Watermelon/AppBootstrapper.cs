
using Caliburn.Micro;
using System;
using System.Threading;
using System.Windows;
using Watermelon.ViewModels;
using Watermelon.Properties;
using System.Diagnostics;
using Watermelon.Models;
namespace Watermelon
{
    class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            SetLanguage();
            DisplayRootViewFor<MainWindowViewModel>();
        }

        private static void SetLanguage()
        {
            if (Settings.Default.Language != String.Empty)
            {
                switch (Settings.Default.Language)
                {
                    case "pl-PL":
                        {
                            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pl-PL");
                            break;
                        }
                    case "en-US":
                        {
                            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                            break;
                        }
                    default:
                        {
                            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                            break;
                        }
                }
            }
        }
    }
}
