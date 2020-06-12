using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHInspect.Classes;
using SHInspect.Constants;
using SHInspect.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;

namespace SHInspect.Classes
{
    public class ElementBO : BaseNotify
    {
        public ElementBO(SHAutomationElement automationElement)
        {
            AutomationElement = automationElement;
            Children = new List<ElementBO>();
            ItemDetails = new List<DetailBO>();
        }

        public SHAutomationElement AutomationElement { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value && _isSelected != true)
                {
                    _isSelected = value;
                }
                else if (!value)
                {
                    _isSelected = value;
                }
            }
        }

        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {

                if (value)
                {
                    LoadChildren(true);
                }
                else
                {
                    Children.Clear();
                }
                _IsExpanded = value;
                RaisePropertyChanged();
            }
        }

        private bool _isRoot;
        public bool IsRoot
        {
            get { return _isRoot; }
            set
            {
                _isRoot = value;
                RaisePropertyChanged();
            }
        }
        public static ElementBO Find(ElementBO node, string name, string searchMode)
        {
            if (node == null)
                return null;


            node.IsExpanded = true;
            if (searchMode == SHInspectConstants.AutomationId)
            {
                if (node.AutomationId != null && node.AutomationId.StartsWith(name))
                    return node;
                else
                {
                    foreach (var child in node.Children)
                    {
                        if (child.AutomationId != null && child.AutomationId.StartsWith(name))
                            return child;

                        var found = Find(child, name, searchMode);
                        if (found != null)
                            return found;
                    }
                }
            }
            else
            {
                if (node.Name != null && node.Name.StartsWith(name))
                    return node;
                else
                {
                    foreach (var child in node.Children)
                    {
                        if (child.Name != null && child.Name.StartsWith(name))
                            return child;

                        var found = Find(child, name, searchMode);
                        if (found != null)
                            return found;
                    }
                }
            }


            return null;

        }

        public string Text => StringExtensions.NormalizeString(AutomationElement.AsTextBox().Text);
        public string Name => StringExtensions.NormalizeString(AutomationElement.Properties.Name.ValueOrDefault);
        public bool IsGridRecord => StringExtensions.NormalizeString(AutomationElement.Properties.Name.ValueOrDefault).Contains("Item:");
        public string AutomationId => StringExtensions.NormalizeString(AutomationElement.Properties.AutomationId.ValueOrDefault);
        public string HelpText => StringExtensions.NormalizeString(AutomationElement.Properties.HelpText.ValueOrDefault);
        public ControlType ControlType => AutomationElement.Properties.ControlType.TryGetValue(out ControlType value) ? value : SHAutomation.Core.Definitions.ControlType.Custom;

        private List<ElementBO> _children;

        public List<ElementBO> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                RaisePropertyChanged();
            }
        }


        private List<DetailBO> _itemDetails;

        public List<DetailBO> ItemDetails
        {
            get { return _itemDetails; }
            set
            {
                _itemDetails = value;
                RaisePropertyChanged();
            }
        }

        public string GetXPath()
        {
            return Debug.GetXPathToElement(AutomationElement);
        }
        public bool IsStillActive()
        {
            try
            {
                string xpath = GetXPath();
                return xpath != string.Empty;
            }
            catch(COMException)
            {
                //thrown when item no longer exists
                return false;
            }
        }

        public void LoadChildren(bool loadInnerChildren)
        {
            if (AutomationElement != null)
            {
               
                var childrenViewModels = new List<ElementBO>();

                foreach (var child in AutomationElement.FindAll(TreeScope.Children, new BoolCondition(true), timeout: 0))
                {
                    var childViewModel = new ElementBO((SHAutomationElement)child);
                    //childViewModel.SelectionChanged += SelectionChanged;
                    childrenViewModels.Add(childViewModel);
                    if (loadInnerChildren)
                    {
                        childViewModel.LoadChildren(false);
                    }
                }
                Children = childrenViewModels;
            }
        }

    }
}
