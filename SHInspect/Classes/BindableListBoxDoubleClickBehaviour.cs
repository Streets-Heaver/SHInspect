using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;
using System.Windows.Input;

namespace SHInspect.Classes
{
    public class BindableListBoxDoubleClickBehaviour : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDoubleClick += OnDoubleClickItem;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.MouseDoubleClick -= OnDoubleClickItem;
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
                            if (src.Name == "ActiveWindows")
                                (src.DataContext as ViewModels.MainViewModel).AddWindow((WindowBO)src.SelectedItem);
                            else if (src.Name == "WindowsToDisplay")
                                (src.DataContext as ViewModels.MainViewModel).RemoveWindow((WindowBO)src.SelectedItem);

                        }
                        break;
                }
            }
        }
    }
}
