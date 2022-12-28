using SHInspect.ViewModels;
using System.Reflection;
using System.Windows;

namespace SHInspect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
            var assembly = Assembly.GetExecutingAssembly();
            var name = assembly.GetName();
            Title = $"{name.Name} {name.Version.Major}.{name.Version.Minor}.{name.Version.Build}";
        }

       

    }
}
