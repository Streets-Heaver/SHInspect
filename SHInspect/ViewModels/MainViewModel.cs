using Prism.Commands;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.Exceptions;
using SHAutomation.Core.StaticClasses;
using SHAutomation.UIA3;
using SHInspect.Classes;
using SHInspect.Constants;
using SHInspect.Extensions;
using SHInspect.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.XPath;

namespace SHInspect.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {

        private ITreeWalker _treeWalker;
        public DelegateCommand AddWindowCommand { get; private set; }
        public DelegateCommand CrashWindowCommand { get; private set; }
        public DelegateCommand DeleteWindowCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }
        public DelegateCommand RefreshGridCommand { get; private set; }
        public DelegateCommand RefreshDetailsCommand { get; private set; }
        public DelegateCommand IsLiveCommand { get; private set; }
        public DelegateCommand CopyXPathCommand { get; private set; }
        public DelegateCommand NextResultCommand { get; private set; }
        public DelegateCommand PreviousResultCommand { get; private set; }
        public DelegateCommand GoToParentCommand { get; private set; }
        public DelegateCommand GoToRootCommand { get; private set; }
        public DelegateCommand RemoveWindowCommand { get; private set; }
        public DelegateCommand<string> CopyValueCommand { get; private set; }

        public MainViewModel()
        {
            SearchTerms = new List<string>() { SHInspectConstants.AutomationId, SHInspectConstants.Name, SHInspectConstants.ClassName, SHInspectConstants.ControlType, SHInspectConstants.XPath };
            AddWindowCommand = new DelegateCommand(AddWindow, CanAddWindow);
            CrashWindowCommand = new DelegateCommand(CrashWindow, CanCrashWindow);
            DeleteWindowCommand = new DelegateCommand(DeleteWindow);
            RefreshGridCommand = new DelegateCommand(RefreshGrid);
            RefreshDetailsCommand = new DelegateCommand(RefreshItemDetails, () => SelectedItemInTree != null);
            IsLiveCommand = new DelegateCommand(ChangeLiveState, () => SelectedItemInTree != null);
            SearchCommand = new DelegateCommand(Search, CanSearch);
            NextResultCommand = new DelegateCommand(() => ChangeResult(true), CanChangeResult);
            PreviousResultCommand = new DelegateCommand(() => ChangeResult(false), CanChangeResult);
            CopyXPathCommand = new DelegateCommand(CopyXPath, () => SelectedItemInTree != null);
            GoToParentCommand = new DelegateCommand(GoToParent, CanGoToParent);
            GoToRootCommand = new DelegateCommand(GoToRoot, CanGoToRoot);
            RemoveWindowCommand = new DelegateCommand(RemoveWindow, CanRemoveWindow);
            CopyValueCommand = new DelegateCommand<string>(CopyValue, CanCopyValue);
            SelectedSearchTerm = Settings.Default.SearchMode;
            Loaded();
        }


        public void Loaded()
        {
            SearchResults = new ISHAutomationElement[0];
            SavedSettingsWindows = new ObservableCollection<WindowBO>();
            var savedWins = Settings.Default.Windows;
            var inspectColour = Settings.Default.InspectionColour;
            HoverSelect = Settings.Default.HoverSelect;

            SelectedColour = new System.Windows.Media.Color()
            {
                A = inspectColour.A,
                R = inspectColour.R,
                G = inspectColour.G,
                B = inspectColour.B
            };

            IsLive = Settings.Default.IsLive;
            if (savedWins != null)
            {
                foreach (string win in savedWins)
                {
                    SavedSettingsWindows.Add(new WindowBO(win, false));
                }
            }
            Elements = new ObservableCollection<ElementBO>();
            Automation = new UIA3Automation();
            Automation.TransactionTimeout = new TimeSpan(0, 0, 0, 5);
            GetDesktop();
            PropertyChangedEventManager.AddHandler(this, SelectedItemChanged, nameof(SelectedItemInTree));
        }
        public void GetDesktop()
        {
            Properties = null;
            Patterns = null;
            Image = null;
            Elements = new ObservableCollection<ElementBO>();
            var item = _automation.GetDesktop();
            SearchResults = new ISHAutomationElement[0];
            CurrentSearchIndex = 0;
            _treeWalker = _automation.TreeWalkerFactory.GetControlViewWalker();
            DesktopItem = item != null ? item : null;
            var items = DesktopItem.FindAllByXPath("*").Select(x => new ElementBO(new SHAutomationElement(x.FrameworkAutomationElement),false));
            foreach (var win in items)
            {
                if (SavedSettingsWindows.Any(x => (!string.IsNullOrEmpty(win.AutomationId?.Trim()) && win.AutomationId.StartsWith(x.Identifier)) || (!string.IsNullOrEmpty(win.Name?.Trim()) && win.Name.StartsWith(x.Identifier))))
                {
                    var savedItem = SavedSettingsWindows.First(x => (!string.IsNullOrEmpty(win.AutomationId?.Trim()) && win.AutomationId.StartsWith(x.Identifier)) || (!string.IsNullOrEmpty(win.Name?.Trim()) && win.Name.StartsWith(x.Identifier)));
                    if (!Elements.Any(x => x.AutomationElement.Equals(win.AutomationElement)))
                    {
                        win.IsTemporary = savedItem.IsTemporary;
                        Elements.Add(win);
                        Elements.ToList().ForEach(x => x.IsExpanded = true);
                    }
                }
            }
            if (Elements.Any())
            {
                SelectedItem = Elements.First();
            }

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += DispatcherTimerTick;
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            _dispatcherTimer.Start();
        }
        ~MainViewModel()
        {
            PropertyChangedEventManager.RemoveHandler(this, SelectedItemChanged, nameof(SelectedItem));
        }

        void SelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SelectedItemInTree == null)
            {
                return;
            }
            else if (!SelectedItemInTree.IsStillActive() || GetRootFromElement(SelectedItemInTree) == null)
            {
                GetDesktop();
            }

            RefreshItemDetails();
        }

        void RefreshItemDetails()
        {
            if (!Elements.Any() || SelectedItemInTree == null)
            {
                Properties = null;
                Patterns = null;
                Image = null;
                return;
            }
            ImageSource img = null;
            var newProperties = SelectedItemInTree != null ? AutomationHelpers.GetElementDetailViewModelProperties(SelectedItemInTree.AutomationElement, out img) : null;

            Image = img;

            if (Properties != null && newProperties != null)
            {
                var output = Properties.Intersect(newProperties, new DetailBOComparer()).Count();
                if (output != Properties.Count)
                {
                    Properties = newProperties;
                    Patterns = SelectedItemInTree != null ? AutomationHelpers.GetElementDetailViewModelPatterns(SelectedItemInTree.AutomationElement) : null;
                }
            }
            else
            {
                Properties = newProperties;
                Patterns = SelectedItemInTree != null ? AutomationHelpers.GetElementDetailViewModelPatterns(SelectedItemInTree.AutomationElement) : null;
            }
        }

        bool CanCopyValue(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        void CopyValue(string value)
        {
            Clipboard.SetText(value);
        }

        public void ChangeLiveState()
        {
            IsLive = !IsLive;
            Settings.Default.IsLive = IsLive;
            Settings.Default.Save();
        }

        [STAThread]
        public void ElementToSelectChanged(ISHAutomationElement obj)
        {
            var tempObject = obj;
            // Build a stack from the root to the hovered item
            if (tempObject.Equals(SelectedItemInTree?.AutomationElement))
            {
                return;
            }
            var pathToRoot = new Stack<SHAutomationElement>();
            while (tempObject != null)
            {
                // Break on circular relationship (should not happen?)
                foreach (var root in Elements)
                {
                    if (pathToRoot.Contains(tempObject) || tempObject.Equals(root)) { break; }
                }

                pathToRoot.Push(tempObject as SHAutomationElement);

                var treeItem = _treeWalker?.GetParent(tempObject as SHAutomationElement);
                tempObject = treeItem != null ? treeItem : null;

            }

            if (Elements.Any() && !Elements.Any(x => !x.IsExpanded))
            {
                foreach (var item in Elements.Where(x => !x.IsExpanded))
                {
                    item.IsExpanded = true;
                }
            }
            string id = null;
            string helpText = null;
            if (pathToRoot.Any() && pathToRoot.Count > 1)
            {
                id = pathToRoot.ToArray()[1].Properties.AutomationId.IsSupported && !string.IsNullOrEmpty(pathToRoot.ToArray()[1].AutomationId) ? pathToRoot.ToArray()[1].AutomationId : pathToRoot.ToArray()[1].Properties.Name.IsSupported ? pathToRoot.ToArray()[1].Name : null;
                helpText = pathToRoot.ToArray()[1].Properties.HelpText.IsSupported && !string.IsNullOrEmpty(pathToRoot.ToArray()[1].HelpText) ? pathToRoot.ToArray()[1].HelpText : null;
            }

            ElementBO elementVm = null;
            if (id != null && helpText != null)
            {
                elementVm = Elements.FirstOrDefault(x => x.AutomationElement != null && (x.AutomationElement.SupportsAutomationId && x.AutomationId == id && x.AutomationElement.SupportsHelpText && x.HelpText == helpText));
            }
            else
            {
                elementVm = id != null ? Elements.FirstOrDefault(x => x.AutomationElement != null && (x.AutomationElement.SupportsAutomationId && x.AutomationId == id) || (x.AutomationElement.SupportsName && x.Name == id)) : Elements.First();
            }

            if (elementVm != null) { elementVm.IsExpanded = true; }
            List<string> list = new List<string>();
            if (id != null)
            {
                pathToRoot.Pop();
                list = pathToRoot.Where(x => x.SupportsName && !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).ToList();
            }
            else
            {
                list = pathToRoot.Where(x => x.SupportsName && !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).ToList();
            }

            while (pathToRoot.Count > 0)
            {
                var elementOnPath = pathToRoot.Pop();
                var nextElementVm = FindElement(elementVm, elementOnPath);
                if (pathToRoot.Any() && nextElementVm == null)
                {
                    elementOnPath = pathToRoot.Pop();
                    nextElementVm = FindElement(elementVm, elementOnPath);
                }
                if (nextElementVm == null && elementVm != null)
                {
                    // Could not find next element, try reloading the parent
                    elementVm.IsExpanded = true;
                    elementVm.LoadChildren(true);
                    // Now search again
                    nextElementVm = FindElement(elementVm, elementOnPath);
                    if (nextElementVm == null)
                    {
                        break;
                    }
                }
                elementVm = nextElementVm;
                if (elementVm != null && !elementVm.IsExpanded)
                {
                    elementVm.IsExpanded = true;
                }
            }
            if (elementVm != null)
            {
                elementVm.IsSelected = true;
            }
        }
        public ElementBO FindElement(ElementBO parent, SHAutomationElement element)
        {
            return parent != null && parent.Children != null && parent.Children.Any() ? parent.Children.FirstOrDefault(child => child.AutomationElement.Equals(element)) : null;
        }
        public void UpdateSettingsWindowList()
        {
            IdentifierToAdd = null;
            if (DesktopItem != null && IsSettings)
            {
                SettingsWindowList = DesktopItem.FindAllByXPath("*").Where(x => !string.IsNullOrEmpty(x.Name?.Trim()) || !string.IsNullOrEmpty(x.AutomationId?.Trim())).Select(x => new WindowBO(x, true)).ToList();
            }
            else
            {
                SettingsWindowList = new List<WindowBO>();
                GetDesktop();
                UpdateSettings();
            }
        }
        public void UpdateSettings()
        {
            var newCollection = new StringCollection();
            string[] wins = SavedSettingsWindows.Where(x => !x.IsTemporary).Select(x => x.Identifier).ToArray();
            newCollection.AddRange(wins);
            Settings.Default.HoverSelect = HoverSelect;
            Settings.Default.Windows = newCollection;
            Settings.Default.InspectionColour = System.Drawing.Color.FromArgb(SelectedColour.A, SelectedColour.R, SelectedColour.G, SelectedColour.B);
            Settings.Default.Save();
        }
        public bool CanAddWindow()
        {
            return IdentifierToAdd != null && !string.IsNullOrEmpty(IdentifierToAdd.Trim()) && !SavedSettingsWindows.Any(x => x.Identifier == IdentifierToAdd);
        }
        public bool CanCrashWindow()
        {
#if DEBUG
            return true;
#else
            return false;
#endif

        }
        public void CrashWindow()
        {
            throw new TestCrashException("You pressed the 'Test Crash' button so I crashed!");
        }
        public void AddWindow()
        {
            var item = new WindowBO(IdentifierToAdd, false);
            if (item.IsCurrent)
            {
                item.IsCurrent = false;
                SavedSettingsWindows.Add(item);
                SelectedCurrentWindowItem = null;
            }
            else
            {
                SavedSettingsWindows.Remove(SelectedWindowItem);
                SavedSettingsWindows.Add(item);
                SelectedCurrentWindowItem = null;
            }
            IdentifierToAdd = null;
        }
        public bool CanSearch()
        {
            return !IsSearching && !string.IsNullOrEmpty(SearchText) && SelectedItemInTree != null;
        }
        public bool CanChangeResult()
        {
            return !IsSearching && SearchResults.Count() > 1;
        }
        public void ChangeResult(bool moveForward)
        {
            CurrentSearchIndex += moveForward ? 1 : -1;
            if (CurrentSearchIndex > SearchResults.Count() - 1)
            {
                CurrentSearchIndex = 0;
            }
            else if (CurrentSearchIndex < 0)
            {
                CurrentSearchIndex = SearchResults.Count() - 1;
            }
            SelectedItem = new ElementBO(SearchResults[CurrentSearchIndex] as SHAutomationElement, SelectedItemInTree.IsTemporary);
        }
        public async void Search()
        {
            SearchResults = new ISHAutomationElement[0];
            CurrentSearchIndex = 0;
            IsSearching = true;
            await Task.Run(() =>
            {
                try
                {
                    string xpath = string.Empty;
                    if (SelectedItemInTree != null)
                    {
                        if (SelectedSearchTerm == SHInspectConstants.ControlType)
                        {
                            xpath = $"//{SearchText}";
                        }
                        else if (SelectedSearchTerm == "XPath")
                        {
                            xpath = SearchText;
                        }
                        else
                        {
                            xpath = $"//*[@{SelectedSearchTerm}='{SearchText}']";
                        }
                    }
                    SearchResults = SelectedItemInTree.AutomationElement.FindAllByXPathBase(xpath);
                    if (SearchResults.Any())
                    {
                        SelectedItem = new ElementBO(SearchResults[0] as SHAutomationElement, SelectedItemInTree.IsTemporary);
                    }

                }
                catch (XPathException)
                {
                    IsSearching = false;
                }
            });
            IsSearching = false;
        }



        public void CopyXPath()
        {
            string copy = SelectedItemInTree.GetXPath();
            Clipboard.SetText(copy);
        }
        public void GoToParent()
        {
            SelectedItem = new ElementBO(SelectedItemInTree.AutomationElement.Parent as SHAutomationElement, SelectedItemInTree.IsTemporary);
        }
        public bool CanGoToParent()
        {
            return SelectedItemInTree != null && SelectedItemInTree.AutomationElement.Parent != null && !SelectedItemInTree.AutomationElement.Parent.Equals(DesktopItem);
        }
        public void GoToRoot()
        {
            if (SelectedItemInTree?.AutomationElement == null)
            {
                return;
            }
            SelectedItem = new ElementBO(GetRootFromElement(SelectedItemInTree) as SHAutomationElement, SelectedItemInTree.IsTemporary);
        }
        public bool CanGoToRoot()
        {
            if (SelectedItemInTree != null)
            {
                var rootItem = GetRootFromElement(SelectedItemInTree);
                var newWindowBO = new WindowBO(rootItem, false);
                return SavedSettingsWindows.Any(x => x.Identifier == newWindowBO.Identifier) && SelectedItemInTree.AutomationElement?.Parent != null && !SelectedItemInTree.AutomationElement.Parent.Equals(DesktopItem);
            }
            else
            {
                return false;
            }
        }
        public void RemoveWindow()
        {
            var win = new WindowBO(SelectedItemInTree.AutomationElement, false);
            var savedWindows = SavedSettingsWindows.Where(x => x.Identifier == win.Identifier).ToList();
            foreach (var delWindow in savedWindows)
            {
                SavedSettingsWindows.Remove(delWindow);
            }
            UpdateSettingsWindowList();
            GetDesktop();
        }
        public bool CanRemoveWindow()
        {
            return SelectedItemInTree != null && SelectedItemInTree.AutomationElement != null && (SelectedItemInTree.AutomationElement.Parent == null || SelectedItemInTree.AutomationElement.Parent.Equals(DesktopItem));
        }
        public void DeleteWindow()
        {
            SavedSettingsWindows.Remove(SelectedExistingWindowItem);
            SelectedExistingWindowItem = null;
            IdentifierToAdd = null;
        }
        public void RefreshGrid()
        {
            GetDesktop();
        }
        public ISHAutomationElement GetRootFromElement(ElementBO element)
        {
            try
            {
                ISHAutomationElement win = Automation.GetParent(element.AutomationElement, x => x.ByControlType(ControlType.Window).And(x.ByClassName(SHInspectConstants.Popup).Not()));
                if (win?.Parent != null && !win.Parent.Equals(DesktopItem))
                {
                    win = win.Parent;
                }
                return win;
            }
            catch (COMException)
            {
                return null;
            }
        }
    }
}
