using Prism.Commands;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using SHInspect.Enums;
using ModernWpf;

namespace SHInspect.ViewModels
{
    class CrashWindowViewModel : ViewModelBase
    {
        public DelegateCommand RestartCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        private ThemeType? _selectedThemeType;
        public ThemeType? SelectedThemeType
        {
            get { return _selectedThemeType; }
            set
            {
                _selectedThemeType = value;
                Settings.Default.Theme = value != null ? (int)value : 0;
                switch (value)
                {
                    case 0: ThemeManager.Current.ApplicationTheme = null; break;
                    default:
                        ThemeManager.Current.ApplicationTheme = (ApplicationTheme)(value) - 1;
                        break;
                }
                Settings.Default.Save();
                RaisePropertyChanged();
            }
        }
        public CrashWindowViewModel()
        {
            Loaded();
        }
        public void Loaded()
        {
            SelectedThemeType = (ThemeType)Settings.Default.Theme;
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
