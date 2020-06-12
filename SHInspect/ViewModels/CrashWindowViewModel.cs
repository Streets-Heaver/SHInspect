using Prism.Commands;
using SHInspect.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;

namespace SHInspect.ViewModels
{
    class CrashWindowViewModel : ViewModelBase
    {
        public DelegateCommand RestartCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public CrashWindowViewModel()
        {
            Loaded();
        }
        public void Loaded()
        {
            RestartCommand = new DelegateCommand(Restart);
            CloseCommand = new DelegateCommand(Close);
        }
        public void Restart()
        {
            string path = Application.ResourceAssembly.Location;
            List<string> pathSegments = path.Split("\\").ToList();
            pathSegments.RemoveAt(pathSegments.Count - 1);
            pathSegments.Add("SHInspect.exe");
            string finalPath = string.Join("\\", pathSegments);
            System.Diagnostics.Process.Start(finalPath);
            Application.Current.Shutdown();
        }

        public void Close()
        {
            Application.Current.Shutdown();
        }
    }
}
