﻿using Prism.Commands;
using SHAutomation.Core.AutomationElements;
using SHInspect.Classes;
using SHInspect.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SHInspect.Views
{
    /// <summary>
    /// Interaction logic for AutomationTreeView.xaml
    /// </summary>
    public partial class AutomationTreeView : UserControl
    {
        DispatcherTimer _hoverTimer;
        private int _hoverCount;
        private ItemsControl _hoveredItem;
        private int _hoverSelectTime;
        public AutomationTreeView()
        {
            InitializeComponent();
           

        }
        private void TreeViewSelectedHandler(object sender, RoutedEventArgs e)
        {
            var item = sender as ItemsControl;
            Tag = item;
            item.Focus();
            if (item != null)
            {
                item.BringIntoView();
                e.Handled = true;
            }
        }

        private void OnHoverTimerTick(object sender, EventArgs e)
        {
            _hoverCount++;
            if (_hoverCount >= 2)
            {
                // Select the item
                _hoverTimer.Stop();
                _hoverCount = 0;

                if ((DataContext as MainViewModel).HoverSelect)
                {
                    if (_hoveredItem != null)
                    {
                        Tag = _hoveredItem;
                        _hoveredItem.Focus();

                        _hoveredItem.BringIntoView();
                        TreeViewItem treeItem = (_hoveredItem as TreeViewItem);
                        if (!treeItem.IsExpanded)
                        {
                            treeItem.IsExpanded = true;
                        }

                    }
                }
            }
        }

        private void TreeViewMouseLeaveSelectedHandler(object sender, RoutedEventArgs e)
        {
            _hoverTimer.Stop();
            _hoverCount = 0;
            _hoveredItem = null;
        }

        private void TreeViewMouseEnterSelectedHandler(object sender, RoutedEventArgs e)
        {
            if (_hoverTimer == null)
            {
                _hoverTimer = new DispatcherTimer();
                _hoverSelectTime = (DataContext as MainViewModel).HoverSelectTime;
                _hoverTimer.Interval = TimeSpan.FromMilliseconds(_hoverSelectTime);
                _hoverTimer.Tick += OnHoverTimerTick;
            }

            if ((DataContext as MainViewModel).HoverSelectTime != _hoverSelectTime)
            {
                _hoverSelectTime = (DataContext as MainViewModel).HoverSelectTime;
                _hoverTimer.Stop();
                _hoverTimer.Interval = TimeSpan.FromMilliseconds(_hoverSelectTime);
                _hoverTimer.Start();
            }

            _hoverTimer.Start();
            _hoverCount = 0;
            _hoveredItem = sender as ItemsControl;

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
