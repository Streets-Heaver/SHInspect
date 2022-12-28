using SHInspect.Classes;
using SHInspect.ViewModels;
using System.Windows;

namespace SHInspect.Views
{
    /// <summary>
    /// Interaction logic for AddWindowPopup.xaml
    /// </summary>
    public partial class AddWindowPopup : Window
    {
        public AddWindowPopup(WindowBO selectedWindow)
        {
            DataContext = new AddWindowPopupViewModel() { SelectedWindow = selectedWindow };
            InitializeComponent();
        }
    }
}
