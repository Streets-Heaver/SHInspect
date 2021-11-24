using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using System.Windows.Input;
using System.Windows.Media;

namespace SHInspect.Classes
{
    public class BindableListBoxDoubleClickBehaviour : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.MouseDoubleClick += OnDoubleClickItem;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.MouseDoubleClick -= OnDoubleClickItem;
            }
        }

        private void OnDoubleClickItem(object sender, MouseButtonEventArgs e)
        {
            ListBox src = e.Source as ListBox;
            
            if (src != null)
            {
                switch (e.ChangedButton)
                {
                    case MouseButton.Left:
                        {
                            (src.DataContext as ViewModels.MainViewModel).AddWindow();
                        }
                        break;
                }
            }
        }
    }
}
