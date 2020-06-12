using Prism.Commands;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SHInspect.Controls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class ImageButton : UserControl
    {
        public ImageButton()
        {
            InitializeComponent();
        }
        public DelegateCommand Command
        {
            get { return (DelegateCommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(DelegateCommand), typeof(ImageButton), new PropertyMetadata(null));

        public DelegateCommand Image
        {
            get { return (DelegateCommand)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public DelegateCommand ImageHeight
        {
            get { return (DelegateCommand)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register(nameof(ImageHeight), typeof(float), typeof(ImageButton), new PropertyMetadata(50.0f));

        public DelegateCommand ImageWidth
        {
            get { return (DelegateCommand)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register(nameof(ImageWidth), typeof(float), typeof(ImageButton), new PropertyMetadata(50.0f));
    }
}
