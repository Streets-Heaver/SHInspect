using Prism.Commands;
using SHAutomation.Core.AutomationElements;
using SHInspect.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SHInspect.Views
{
    /// <summary>
    /// Interaction logic for AutomationTreeView.xaml
    /// </summary>
    public partial class AutomationTreeView : UserControl
    {
        public AutomationTreeView()
        {
            InitializeComponent();
        }
        private void TreeViewSelectedHandler(object sender, RoutedEventArgs e)
        {
            var item = sender as ItemsControl;
            this.Tag = item;
            item.Focus();
            if (item != null)
            {
                item.BringIntoView();
                e.Handled = true;
            }
        }



        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }



        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutomationTreeView), new UIPropertyMetadata(null, OnSelectedItemChanged));

        public static bool IsCalling = false;
        public static bool Reset = true;
        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as ElementBO;
            if (item != null)
            {
                OnSelectedItemChanged(sender, item.AutomationElement);
            }
        }
        private static void OnSelectedItemChanged(DependencyObject sender, ISHAutomationElement element)
        {
            AutomationTreeView mainTreeView = (AutomationTreeView)sender;
            (mainTreeView.DataContext as MainViewModel).ElementToSelectChanged(element);
        }






        public DelegateCommand CopyXPathCommand
        {
            get { return (DelegateCommand)GetValue(CopyXPathCommandProperty); }
            set { SetValue(CopyXPathCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CopyXPathCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CopyXPathCommandProperty =
            DependencyProperty.Register("CopyXPathCommand", typeof(DelegateCommand), typeof(AutomationTreeView), new PropertyMetadata(null));






    }
}
