using SHInspect.ViewModels;
using SHInspect.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    }
}
