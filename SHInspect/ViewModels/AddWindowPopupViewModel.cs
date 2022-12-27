using Prism.Commands;
using SHInspect.Classes;
using System.Windows;

namespace SHInspect.ViewModels
{
    class AddWindowPopupViewModel : ViewModelBase
    {
        public DelegateCommand<Window> AddWindowCommand { get; private set; }
        private WindowBO _selectedWindow;

        public WindowBO SelectedWindow
        {
            get { return _selectedWindow; }
            set
            {
                _selectedWindow = value;
                RaisePropertyChanged();
            }
        }

        public AddWindowPopupViewModel()
        {
            Loaded();
        }
        public void Loaded()
        {
            AddWindowCommand = new DelegateCommand<Window>(AddWindow);
        }
        public void AddWindow(Window window)
        {
            SelectedWindow.IsCurrent = false;
            SelectedWindow.AutomationId = null;
            SelectedWindow.Name = null;
            SelectedWindow.AutomationElement = null;
            (Application.Current.MainWindow.DataContext as MainViewModel).SavedSettingsWindows.Add(SelectedWindow);
            (Application.Current.MainWindow.DataContext as MainViewModel).UpdateSettingsWindowList();
            (Application.Current.MainWindow.DataContext as MainViewModel).GetDesktop();
            window.Close();
        }
    }
}
