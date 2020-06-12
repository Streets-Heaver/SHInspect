using SHInspect.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
