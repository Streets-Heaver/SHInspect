using ModernWpf;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.UIA3;
using SHInspect.Classes;
using SHInspect.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;

namespace SHInspect.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private string _searchText;
        private bool _isSettings;
        private bool _isSearching;
        private List<string> _searchTerms;
        private List<ThemeType> _ThemeTypes;
        private string _selectedSearchTerm;
        private ThemeType? _selectedThemeType;
        private ElementBO _selectedItem;
        private ElementBO _selectedItemInTree;
        private WindowBO _selectedCurrentWindowItem;
        private WindowBO _selectedExistingWindowItem;
        private ISHAutomationElement _desktopItem;
        private List<DetailBO> _properties;
        private ObservableCollection<WindowBO> _savedSettingsWindow;
        private List<PatternBO> _patterns;
        private ObservableCollection<ElementBO> _elements;
        private ISHAutomationElement[] _searchResults;
        private int _currentSearchIndex;
        private Color _selectedColour;
        private bool _hoverSelect;
        private int _hoverSelectTime;
        private UIA3Automation _automation;
        private ImageSource _image;
        private bool _isLive;
        private bool _isInspecting;
        public DispatcherTimer _dispatcherTimer;
        private DateTime _previousRefresh;
        private ITreeWalker _treeWalker;

        public bool Inspecting
        {
            get { return _isInspecting; }
            set { _isInspecting = value; }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                SearchResults = new ISHAutomationElement[0];
                RaisePropertyChanged();
            }
        }

        public bool IsSettings
        {
            get { return _isSettings; }
            set
            {
                if (_isSettings != value)
                {
                    _isSettings = value;
                    UpdateSettingsWindowList();
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsSearching
        {
            get { return _isSearching; }
            set
            {
                if (_isSearching != value)
                {
                    _isSearching = value;
                    RaisePropertyChanged();
                }
            }
        }
        public List<string> SearchTerms
        {
            get { return _searchTerms; }
            set
            {
                if (_searchTerms != value)
                {
                    _searchTerms = value;
                    RaisePropertyChanged();
                }
            }
        }
        public List<ThemeType> ThemeTypes
        {
            get { return _ThemeTypes; }
            set
            {
                if (_ThemeTypes != value)
                {
                    _ThemeTypes = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string SelectedSearchTerm
        {
            get { return _selectedSearchTerm; }
            set
            {
                _selectedSearchTerm = value;
                Settings.Default.SearchMode = value;
                Settings.Default.Save();
                RaisePropertyChanged();
            }
        }
        public ThemeType? SelectedThemeType
        {
            get { return _selectedThemeType; }
            set
            {
                _selectedThemeType = value;
                Settings.Default.Theme = value != null ? (int)value : 0;
                switch (value)
                {
                    case 0: ThemeManager.Current.ApplicationTheme = null; break;
                    default:
                        ThemeManager.Current.ApplicationTheme = (ApplicationTheme)(value) - 1;
                        break;
                }
                Settings.Default.Save();
                RaisePropertyChanged();
            }
        }
        public ElementBO SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged();
            }
        }

        public ElementBO SelectedItemInTree
        {
            get { return _selectedItemInTree; }
            set
            {
                _selectedItemInTree = value;
                if (SelectedItemInTree?.AutomationElement != null && SelectedItem?.AutomationElement != null && SelectedItemInTree.AutomationElement.Equals(SelectedItem.AutomationElement) && SelectedItemInTree.AutomationElement.SupportsBoundingRectangle && !SelectedItemInTree.AutomationElement.BoundingRectangle.IsEmpty)
                {
                    DrawHighlight(_selectedItemInTree.AutomationElement);
                    SelectedItem = null;
                }
                else if (SelectedItemInTree?.AutomationElement != null && SelectedItem == null)
                {
                    DrawHighlight(_selectedItemInTree.AutomationElement);
                }
                RaisePropertyChanged();
            }
        }
        public WindowBO SelectedWindowItem
        {
            get { return SelectedCurrentWindowItem ?? SelectedExistingWindowItem; }
        }

        public WindowBO SelectedCurrentWindowItem
        {
            get { return _selectedCurrentWindowItem; }
            set
            {
                if (value != null)
                {
                    SelectedExistingWindowItem = null;
                    IdentifierToAdd = value.Identifier;
                }
                _selectedCurrentWindowItem = value;
                _selectedCurrentWindowItem?.UpdateProperties();
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectedWindowItem));
            }
        }

        public WindowBO SelectedExistingWindowItem
        {
            get { return _selectedExistingWindowItem; }
            set
            {
                if (value != null)
                {
                    SelectedCurrentWindowItem = null;
                    IdentifierToAdd = value.Identifier;
                }
                _selectedExistingWindowItem = value;
                _selectedExistingWindowItem?.UpdateProperties();
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectedWindowItem));
            }
        }

        public ISHAutomationElement DesktopItem
        {
            get { return _desktopItem; }
            set
            {
                _desktopItem = value;
                RaisePropertyChanged();
            }
        }

        public List<DetailBO> Properties
        {
            get { return _properties; }
            set
            {
                _properties = value;
                RaisePropertyChanged();
            }
        }

        public List<PatternBO> Patterns
        {
            get { return _patterns; }
            set
            {
                _patterns = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ElementBO> Elements
        {
            get { return _elements; }
            set
            {
                _elements = value;
                RaisePropertyChanged();
            }
        }
        private List<WindowBO> _settingsWindowList;

        public List<WindowBO> SettingsWindowList
        {
            get { return _settingsWindowList; }
            set
            {
                _settingsWindowList = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<WindowBO> SavedSettingsWindows
        {
            get { return _savedSettingsWindow; }
            set
            {
                _savedSettingsWindow = value;
                RaisePropertyChanged();
            }
        }
        public ISHAutomationElement[] SearchResults
        {
            get { return _searchResults; }
            set
            {
                _searchResults = value;
                RaisePropertyChanged(nameof(SearchResultText));
                RaisePropertyChanged();
            }
        }
        public int CurrentSearchIndex
        {
            get { return _currentSearchIndex; }
            set
            {
                _currentSearchIndex = value;
                RaisePropertyChanged(nameof(SearchResultText));
                RaisePropertyChanged();
            }
        }
        public System.Windows.Media.Color SelectedColour
        {
            get { return _selectedColour; }
            set
            {
                _selectedColour = value;
                RaisePropertyChanged();
            }
        }
        public bool HoverSelect
        {
            get { return _hoverSelect; }
            set
            {
                _hoverSelect = value;
                RaisePropertyChanged();
            }
        }


        public int HoverSelectTime
        {
            get { return _hoverSelectTime; }
            set { _hoverSelectTime = value; }
        }

        public string SearchResultText
        {
            get { return SearchResults.Any() ? $"Search Results: {CurrentSearchIndex + 1}/{SearchResults.Count()}" : null; }
        }
        public UIA3Automation Automation
        {
            get { return _automation; }
            set
            {
                _automation = value;
                RaisePropertyChanged();
            }
        }

        public ImageSource Image
        {
            get { return _image; }
            set
            {
                _image = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLive
        {
            get { return _isLive; }
            set
            {
                _isLive = value;
                RaisePropertyChanged();
            }
        }
        private string _identifierToAdd;

        public string IdentifierToAdd
        {
            get { return _identifierToAdd; }
            set
            {
                _identifierToAdd = value;
                RaisePropertyChanged();
            }
        }
    }
}
