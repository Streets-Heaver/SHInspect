using SHInspect.ViewModels;
using SHInspect.Views;
using Squirrel;
using Squirrel.Sources;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace SHInspect
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (!(e.Exception is COMException))
            {
                e.Handled = true;
                if (!Application.Current.Windows.OfType<CrashWindow>().Any())
                {
                    Application.Current.MainWindow.IsEnabled = false;
                    CrashWindow crashWindow = new CrashWindow($"Error thrown of type: {e.Exception.GetType().ToString()}. {e.Exception.Message} {Environment.NewLine} {e.Exception.StackTrace}");
                    crashWindow.Owner = Application.Current.MainWindow;
                    (Application.Current.MainWindow.DataContext as MainViewModel).Stop();
                    crashWindow.ShowDialog();
                    crashWindow.BringIntoView();
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SquirrelAwareApp.HandleEvents(
       onInitialInstall: OnAppInstall,
       onAppUninstall: OnAppUninstall,
       onEveryRun: OnAppRun);
            await CheckForUpdates();
        }

        private static void OnAppInstall(SemanticVersion version, IAppTools tools)
        {
            tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        }

        private static void OnAppUninstall(SemanticVersion version, IAppTools tools)
        {
            tools.RemoveShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        }

        private static void OnAppRun(SemanticVersion version, IAppTools tools, bool firstRun)
        {
            tools.SetProcessAppUserModelId();
        }

        private static async Task CheckForUpdates()
        {
            try
            {
                using var mgr = new UpdateManager(new GithubSource("https://github.com/Streets-Heaver/SHInspect", "", false));
                await mgr.UpdateApp();

            }
            catch (InvalidOperationException)
            {
                //Error with squirrel update, carry on...
            }


        }
    }
}
