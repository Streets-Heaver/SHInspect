using SHInspect.ViewModels;
using System.Windows;

namespace SHInspect.Views
{
    /// <summary>
    /// Interaction logic for CrashWindow.xaml
    /// </summary>
    public partial class CrashWindow : Window
    {
        public CrashWindow(string message)
        {
            DataContext = new CrashWindowViewModel();
            InitializeComponent();
            ErrorTextBox.Text = message;
        }
    }
}
