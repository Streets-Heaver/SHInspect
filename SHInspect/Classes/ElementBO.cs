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
        public ElementBO(SHAutomationElement automationElement, bool isTemporary, SHAutomationElement rootElement)
        {
            AutomationElement = automationElement;
            IsTemporary = isTemporary;
            Children = new List<ElementBO>();
            ItemDetails = new List<DetailBO>();
            RootElement = rootElement;
        }
        public SHAutomationElement RootElement;
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

        private bool _isTemporary;
        public bool IsTemporary
        {
            get { return _isTemporary; }
            set
            {
                _isTemporary = value;
                RaisePropertyChanged();
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

       
        public string Text => StringExtensions.NormalizeString(AutomationElement.AsTextBox().Text);
        public string Name => StringExtensions.NormalizeString(AutomationElement.Properties.Name.ValueOrDefault);
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

        public string GetXPath(SHAutomationElement root = null)
        {
            return XPathHelper.GetXPathToElement(AutomationElement,root);
        }

        public void LoadChildren(bool loadInnerChildren)
        {
            if (AutomationElement != null)
            {
                var childrenViewModels = new List<ElementBO>();


                foreach (var child in AutomationElement.FindAll(TreeScope.Children, new BoolCondition(true), TimeSpan.Zero))
                {
                    var childViewModel = new ElementBO((SHAutomationElement)child,IsTemporary,RootElement);
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
