using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SHInspect.Controls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SearchView_Loaded);
        }
        void SearchView_Loaded(object sender, RoutedEventArgs e)
        {
            CommandBinding binding = new CommandBinding(ApplicationCommands.Find);
            binding.Executed += FindCommand_Executed;
            (this.Parent as UIElement).CommandBindings.Add(binding);
        }

        private void FindCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SearchTermTextBox.Focus();
        }
        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty));

        public string PlaceHolderText
        {
            get { return (string)GetValue(PlaceHolderTextProperty); }
            set { SetValue(PlaceHolderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderTextProperty =
            DependencyProperty.Register(nameof(PlaceHolderText), typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty));
        public object SearchTerms
        {
            get { return GetValue(SearchTermsProperty); }
            set { SetValue(SearchTermsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTermsProperty =
            DependencyProperty.Register(nameof(SearchTerms), typeof(object), typeof(SearchBox), new PropertyMetadata(string.Empty));

        public string SelectedSearchTerm
        {
            get { return (string)GetValue(SelectedSearchTermProperty); }
            set { SetValue(SelectedSearchTermProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSearchTermProperty =
            DependencyProperty.Register(nameof(SelectedSearchTerm), typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty));



        public DelegateCommand SearchCommand
        {
            get { return (DelegateCommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register(nameof(SearchCommand), typeof(DelegateCommand), typeof(SearchBox), new PropertyMetadata(null));

    }
}
